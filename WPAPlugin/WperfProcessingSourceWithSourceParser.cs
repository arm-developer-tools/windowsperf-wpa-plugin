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

        protected override ICustomDataProcessor CreateProcessorCore(
            IEnumerable<IDataSource> dataSources,
            IProcessorEnvironment processorEnvironment,
            ProcessorOptions options
        )
        {
            string[] filePathList = dataSources.Select(el => el.Uri.LocalPath).ToArray();
            WperfSourceParser parser = new WperfSourceParser(filePathList);
            return new WperfCustomDataProcessorWithSourceParser(
                parser,
                options,
                applicationEnvironment,
                processorEnvironment
            );
        }

        protected override bool IsDataSourceSupportedCore(IDataSource source)
        {
            string sourcePath = source.Uri.LocalPath;
            string schemaPath = Path.Combine(
                Environment.CurrentDirectory,
                "Schemas",
                "wperf.stat.schema"
            );
            string schemaContent = File.ReadAllText(schemaPath);
            JsonSchema schema = JsonSchema.FromText(schemaContent);
            string jsonContent = File.ReadAllText(sourcePath);
            JsonNode json = JsonNode.Parse(jsonContent);
            bool isValid = schema.Evaluate(json).IsValid;

            return isValid;
        }

        protected override void SetApplicationEnvironmentCore(
            IApplicationEnvironment applicationEnvironment
        )
        {
            this.applicationEnvironment = applicationEnvironment;
        }
    }
}
