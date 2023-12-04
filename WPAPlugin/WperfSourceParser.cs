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
using Microsoft.Performance.SDK.Extensibility.SourceParsing;
using Microsoft.Performance.SDK.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using WPAPlugin.Constants;
using WPAPlugin.Events;
using WPAPlugin.Parsers;
using WPAPlugin.Utils;

namespace WPAPlugin
{
    public class WperfSourceParser : ISourceParser<CountingEvent, WperfSourceParser, string>
    {
        private readonly string[] filePathList;

        public WperfSourceParser(string[] filePathList)
        {
            this.filePathList = filePathList;
        }

        public DataSourceInfo DataSourceInfo { get; private set; }
        public Type DataElementType => typeof(CountingEvent);

        public Type DataContextType => typeof(WperfSourceParser);

        public Type DataKeyType => typeof(string);

        public int MaxSourceParseCount => 1;

        public DateTime StartWallClockUtc { get; private set; }

        public string Id => WperfPluginConstants.ParserId;

        public void PrepareForProcessing(
            bool allEventsConsumed,
            IReadOnlyCollection<string> requestedDataKeys
        )
        {
            // NOOP
        }

        public void ProcessSource(
            ISourceDataProcessor<CountingEvent, WperfSourceParser, string> dataProcessor,
            ILogger logger,
            IProgress<int> progress,
            CancellationToken cancellationToken
        )
        {
            int currentFile = 0;
            int filesCount = filePathList.Length;

            long minTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            StartWallClockUtc = new DateTime(minTimestamp).ToUniversalTime();

            double totalTimeElapsed = 0;
            double previousTimeElapsed = 0;
            foreach (string file in filePathList)
            {
                string jsonContent = File.ReadAllText(file);
                WperfTimeline wperfTimeline = WperfTimeline.FromJson(jsonContent);

                int totalCount = wperfTimeline.Timeline.Length;
                int currentCount = 0;

                foreach (WperfStats count in wperfTimeline.Timeline)
                {
                    int coreCount = count.Core.PerformanceCounters.Length;
                    int currentCore = 0;
                    if (currentCount > 0)
                    {
                        previousTimeElapsed = totalTimeElapsed;
                    }
                    totalTimeElapsed += count.TimeElapsed;

                    foreach (CorePerformanceCounter core in count.Core.PerformanceCounters)
                    {
                        int eventCount = core.PerformanceCounter.Length;
                        int currentEvent = 0;
                        foreach (
                            CorePerformanceCounterItem rawCountingEvent in core.PerformanceCounter
                        )
                        {
                            CountingEvent countingEvent = new CountingEvent(
                                core.CoreNumber,
                                rawCountingEvent.CounterValue,
                                rawCountingEvent.EventName,
                                rawCountingEvent.EventIdx,
                                rawCountingEvent.EventNote,
                                previousTimeElapsed,
                                totalTimeElapsed
                            );
                            _ = dataProcessor.ProcessDataElement(
                                countingEvent,
                                this,
                                cancellationToken
                            );
                            progress.Report(
                                Calc.CalculateProgress(
                                    currentFile,
                                    currentCore,
                                    currentEvent,
                                    currentCount,
                                    filesCount,
                                    coreCount,
                                    eventCount,
                                    totalCount
                                )
                            );
                            currentEvent++;
                        }
                        currentCore++;
                    }
                    currentCount++;
                }
                currentFile++;
            }
            Timestamp traceStartTimestamp = Timestamp.FromMilliseconds(minTimestamp);
            Timestamp traceEndTimestamp = Timestamp.FromMilliseconds(
                minTimestamp + (totalTimeElapsed * 1000)
            );

            DataSourceInfo = new DataSourceInfo(
                0,
                (traceEndTimestamp - traceStartTimestamp).ToNanoseconds,
                StartWallClockUtc
            );
            ;
        }
    }
}
