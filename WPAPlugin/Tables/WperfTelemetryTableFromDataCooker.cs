﻿// BSD 3-Clause License
//
// Copyright (c) 2024, Arm Limited
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
//
// 3. Neither the name of the copyright holder nor the names of its
//    contributors may be used to endorse or promote products derived from
//    this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using Microsoft.Performance.SDK;
using Microsoft.Performance.SDK.Extensibility;
using Microsoft.Performance.SDK.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using WPAPlugin.Constants;
using WPAPlugin.DataCookers;
using WPAPlugin.Events;
using WPAPlugin.Utils;

namespace WPAPlugin.Tables
{
    [Table]
    public static class WperfTelemetryTableFromDataCooker
    {
        public static TableDescriptor TableDescriptor =>
            new TableDescriptor(
                Guid.Parse("{F116F7E5-FBED-46F7-B1BD-AC034CAE3544}"),
                "Telemetry timeline",
                "Telemetry timeline parsed from wperf JSON output",
                requiredDataCookers: new List<DataCookerPath> { WperfPluginConstants.TelemetryCookerPath },
                defaultLayout: TableLayoutStyle.Graph
            );



        private static void BuildTableFilteredByKey(string key, IReadOnlyList<WperfEventWithRelativeTimestamp> lineItems, ITableBuilder tableBuilder)
        {

            var filteredLineItems = lineItems.Where(x => x.Unit == key).ToList();

            if (filteredLineItems.Count == 0)
            {
                return;
            }

            List<WperfEventWithRelativeTimestamp> filledList = new List<WperfEventWithRelativeTimestamp>();

            for (int i = 0; i < lineItems.Count; ++i)
            {
                if (i < filteredLineItems.Count)
                {
                    filledList.Add(filteredLineItems[i]);
                }
                else
                {
                    filledList.Add(new WperfEventWithRelativeTimestamp());
                }
            }

            ColumnConfiguration CoreColumn = new ColumnConfiguration(
                new ColumnMetadata(
                    Guid.NewGuid(),
                    Helpers.GenerateColumnName(key, "Core"),
                    "Core Number"
                )
            );
            ColumnConfiguration ValueColumn = new ColumnConfiguration(
                new ColumnMetadata(
                    Guid.NewGuid(),
                    Helpers.GenerateColumnName(key, "Value"),
                    "Value Number"
                ),
                new UIHints { AggregationMode = AggregationMode.Sum }
            );

            ColumnConfiguration EventNameColumn = new ColumnConfiguration(
                new ColumnMetadata(
                    Guid.NewGuid(),
                    Helpers.GenerateColumnName(key, "Name"),
                    "Event Name"
                )
            );
            ColumnConfiguration UnitColumn = new ColumnConfiguration(
                new ColumnMetadata(
                    Guid.NewGuid(),
                    Helpers.GenerateColumnName(key, "Unit"),
                    "Telemetry Unit"
                )
            );
            ColumnConfiguration ProductNameColumn = new ColumnConfiguration(
                new ColumnMetadata(
                    Guid.NewGuid(),
                    Helpers.GenerateColumnName(key, "Product Name"),
                    "Telemtry Product Name"
                )
            );
            ColumnConfiguration RelativeStartTimestampColumn =
                new ColumnConfiguration(
                    new ColumnMetadata(
                        Guid.NewGuid(),
                        Helpers.GenerateColumnName(key, "Start"),
                        "Start Time"
                    )
                );

            ColumnConfiguration RelativeEndTimestampColumn =
                new ColumnConfiguration(
                    new ColumnMetadata(
                        Guid.NewGuid(),
                        Helpers.GenerateColumnName(key, "End"),
                        "End Time"
                    )
                );

            IProjection<int, WperfEventWithRelativeTimestamp> baseProjection = Projection.Index(
                filledList
            );
            IProjection<int, int> coreProjection = baseProjection.Compose(el => el.CoreNumber);
            IProjection<int, string> nameProjection = baseProjection.Compose(el => el.Name);
            IProjection<int, double> valueProjection = baseProjection.Compose(el => el.Value);
            IProjection<int, string> productNameProjection = baseProjection.Compose(el => el.ProductName);
            IProjection<int, string> unitProjection = baseProjection.Compose(el => el.Unit);
            IProjection<int, Timestamp> relativeStartTimeProjection = baseProjection.Compose(
                el => el.RelativeStartTimestamp
            );
            IProjection<int, Timestamp> relativeEndTimeProjection = baseProjection.Compose(
                el => el.RelativeEndTimestamp
            );
            TableConfiguration groupByEventConfig = new TableConfiguration(key)
            {
                Columns = new[]
                {
                    EventNameColumn,
                    CoreColumn,
                    TableConfiguration.PivotColumn,
                    UnitColumn,
                    ProductNameColumn,
                    RelativeStartTimestampColumn,
                    RelativeEndTimestampColumn,
                    TableConfiguration.GraphColumn,
                    ValueColumn,
                },
                InitialFilterShouldKeep = false,
                InitialFilterQuery = $@"[{RelativeStartTimestampColumn.Metadata.Name}]:={"0"} AND [{RelativeEndTimestampColumn.Metadata.Name}]:={"0"}"
            };

            groupByEventConfig.AddColumnRole(ColumnRole.StartTime, RelativeStartTimestampColumn);
            groupByEventConfig.AddColumnRole(ColumnRole.EndTime, RelativeEndTimestampColumn);

            _ = tableBuilder
                .AddTableConfiguration(groupByEventConfig)
                .SetDefaultTableConfiguration(groupByEventConfig)
                .SetRowCount(filledList.Count)
                .AddColumn(CoreColumn, coreProjection)
                .AddColumn(EventNameColumn, nameProjection)
                .AddColumn(ValueColumn, valueProjection)
                .AddColumn(UnitColumn, unitProjection)
                .AddColumn(ProductNameColumn, productNameProjection)
                .AddColumn(RelativeStartTimestampColumn, relativeStartTimeProjection)
                .AddColumn(RelativeEndTimestampColumn, relativeEndTimeProjection);
        }

        public static void BuildTable(ITableBuilder tableBuilder, IDataExtensionRetrieval tableData)
        {
            IReadOnlyList<WperfEventWithRelativeTimestamp> lineItems = tableData.QueryOutput<
                IReadOnlyList<WperfEventWithRelativeTimestamp>
            >(
                new DataOutputPath(
                    WperfPluginConstants.TelemetryCookerPath,
                    nameof(WperfTimelineDataCooker.WperfEventWithRelativeTimestamps)
                )
            );


            if (lineItems.Count == 0)
                return;

            foreach (string metric in WperfPluginConstants.WperfPresetMetrics)
            {
                BuildTableFilteredByKey(metric, lineItems, tableBuilder);
            }
        }
    }
}
