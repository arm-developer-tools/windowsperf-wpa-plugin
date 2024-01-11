// BSD 3-Clause License
//
// Copyright (c) 2022, Arm Limited
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
using WPAPlugin.Constants;
using WPAPlugin.DataCookers;
using WPAPlugin.Events;

namespace WPAPlugin.Tables
{
    [Table]
    public static class WperfTelemetryTableFromDataCooker
    {
        public static TableDescriptor TableDescriptor =>
            new TableDescriptor(
                Guid.Parse("{F116F7E5-FBED-46F7-B1BD-AC034CAE3544}"),
                "Telemetry events from Data Cooker",
                "Telemetry events parsed from wperf JSON output",
                requiredDataCookers: new List<DataCookerPath> { WperfPluginConstants.TelemetryCookerPath }
            );

        private static readonly ColumnConfiguration CoreColumn = new ColumnConfiguration(
            new ColumnMetadata(
                new Guid("{241FD1F7-0DA2-427A-836E-15FE5D2FFD74}"),
                "Core",
                "Core Number"
            )
        );
        private static readonly ColumnConfiguration ValueColumn = new ColumnConfiguration(
            new ColumnMetadata(
                new Guid("{E0D38242-EAEF-4CCC-B6C6-EEB196C35843}"),
                "Value",
                "Value Number"
            ),
            new UIHints { AggregationMode = AggregationMode.Sum }
        );

        private static readonly ColumnConfiguration EventNameColumn = new ColumnConfiguration(
            new ColumnMetadata(
                new Guid("{46E84068-2734-45ED-BF13-C66AA0C73184}"),
                "Name",
                "Event Name"
            )
        );
        private static readonly ColumnConfiguration UnitColumn = new ColumnConfiguration(
            new ColumnMetadata(
                new Guid("{682D6D47-5908-4E93-8AED-4F69153E663D}"),
                "Unit",
                "Telemetry Unit"
            )
        );
        private static readonly ColumnConfiguration ProductNameColumn = new ColumnConfiguration(
            new ColumnMetadata(
                new Guid("{5E1F7E50-3D54-445E-A1C8-34E4185F1DC8}"),
                "Product Name",
                "Telemtry Product Name"
            )
        );
        private static readonly ColumnConfiguration RelativeStartTimestampColumn =
            new ColumnConfiguration(
                new ColumnMetadata(
                    new Guid("{DEA2ECAD-EC75-41EF-A318-650FE02A1330}"),
                    "Start",
                    "Start Time"
                )
            );

        private static readonly ColumnConfiguration RelativeEndTimestampColumn =
            new ColumnConfiguration(
                new ColumnMetadata(
                    new Guid("{32AF3CE1-B97C-49F2-A1F9-EB1450EAB326}"),
                    "End",
                    "End Time"
                )
            );

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

            IProjection<int, WperfEventWithRelativeTimestamp> baseProjection = Projection.Index(
                lineItems
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

            TableConfiguration groupByCoreConfig = new TableConfiguration("Group by core")
            {
                Columns = new[]
                {
                    CoreColumn,
                    EventNameColumn,
                    TableConfiguration.PivotColumn,
                    UnitColumn,
                    ProductNameColumn,
                    RelativeStartTimestampColumn,
                    RelativeEndTimestampColumn,
                    TableConfiguration.GraphColumn,
                    ValueColumn,
                },
            };

            TableConfiguration groupByEventConfig = new TableConfiguration("Group by event")
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
            };

            groupByCoreConfig.AddColumnRole(ColumnRole.StartTime, RelativeStartTimestampColumn);
            groupByEventConfig.AddColumnRole(ColumnRole.StartTime, RelativeStartTimestampColumn);

            groupByCoreConfig.AddColumnRole(ColumnRole.EndTime, RelativeEndTimestampColumn);
            groupByEventConfig.AddColumnRole(ColumnRole.EndTime, RelativeEndTimestampColumn);

            _ = tableBuilder
                .AddTableConfiguration(groupByCoreConfig)
                .AddTableConfiguration(groupByEventConfig)
                .SetDefaultTableConfiguration(groupByEventConfig)
                .SetRowCount(lineItems.Count)
                .AddColumn(CoreColumn, coreProjection)
                .AddColumn(EventNameColumn, nameProjection)
                .AddColumn(ValueColumn, valueProjection)
                .AddColumn(UnitColumn, unitProjection)
                .AddColumn(ProductNameColumn, productNameProjection)
                .AddColumn(RelativeStartTimestampColumn, relativeStartTimeProjection)
                .AddColumn(RelativeEndTimestampColumn, relativeEndTimeProjection);
        }
    }
}
