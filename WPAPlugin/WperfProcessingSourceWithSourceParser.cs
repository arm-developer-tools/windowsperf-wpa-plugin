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


using Json.Schema;
using Microsoft.Performance.SDK.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;

namespace WPAPlugin
{
    [ProcessingSource(
        "{05D8A372-EC56-43DE-83FF-02BDD9042727}",
        "wperf Counting Data Source",
        "Processes wperf counting trace files exported as JSON"
    )]
    [FileDataSource(".json", "JSON file output from wperf")]
    public class WperfProcessingSourceWithSourceParser : ProcessingSource
    {
        private IApplicationEnvironment applicationEnvironment;

        public override ProcessingSourceInfo GetAboutInfo()
        {
            return new ProcessingSourceInfo
            {
                CopyrightNotice = "Copyright 2021 Microsoft Corporation. All Rights Reserved.",
                LicenseInfo = new LicenseInfo
                {
                    Name = "BSD 3-Clause License",
                    Text = "Please see the link for the full license text.",
                    Uri = "https://gitlab.com/Linaro/WindowsPerf/wpa-plugin/-/blob/main/LICENSE",
                },
                Owners = new[]
                {
                    new ContactInfo
                    {
                        // TODO: add appropriate contact information
                    },
                },
                ProjectInfo = new ProjectInfo
                {
                    Uri = "https://gitlab.com/Linaro/WindowsPerf/wpa-plugin",
                },
            };
        }

        // IsDataSourceSupportedCore is being triggered 3 times per file for an unknown reason
        // So resorted to using a HashSet instead of a List to deduplicate the entries
        private readonly HashSet<string> countingPathList = new HashSet<string>();
        private readonly HashSet<string> timelinePathList = new HashSet<string>();

        protected override ICustomDataProcessor CreateProcessorCore(
            IEnumerable<IDataSource> dataSources,
            IProcessorEnvironment processorEnvironment,
            ProcessorOptions options
        )
        {
            WperfSourceParser parser = new WperfSourceParser(
                timelinePathList.ToArray(),
                countingPathList.ToArray()
            );

            timelinePathList.Clear();
            countingPathList.Clear();
            return new WperfCustomDataProcessorWithSourceParser(
                parser,
                options,
                applicationEnvironment,
                processorEnvironment
            );
        }

        private static bool ValidateJson(string sourcePath, string schemaName)
        {
            string schemaPath = Path.Combine(Environment.CurrentDirectory, "Schemas", schemaName);
            string schemaContent = File.ReadAllText(schemaPath);
            JsonSchema schema = JsonSchema.FromText(schemaContent);
            string jsonContent = File.ReadAllText(sourcePath);
            JsonNode json = JsonNode.Parse(jsonContent);
            return schema.Evaluate(json).IsValid;
        }

        protected override bool IsDataSourceSupportedCore(IDataSource source)
        {
            string sourcePath = source.Uri.LocalPath;
            bool isValidTimeline = ValidateJson(sourcePath, "wperf-timeline.stat.schema");
            bool isValidCount = false;

            if (isValidTimeline)
            {
                _ = timelinePathList.Add(sourcePath);
            }
            else
            {
                isValidCount = ValidateJson(sourcePath, "wperf.stat.schema");
                if (isValidCount)
                {
                    _ = countingPathList.Add(sourcePath);
                }
            }

            return isValidCount || isValidTimeline;
        }

        protected override void SetApplicationEnvironmentCore(
            IApplicationEnvironment applicationEnvironment
        )
        {
            this.applicationEnvironment = applicationEnvironment;
        }
    }
}
