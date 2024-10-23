// BSD 3-Clause License
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Performance.SDK;
using Microsoft.Performance.SDK.Extensibility.SourceParsing;
using Microsoft.Performance.SDK.Processing;
using WPAPlugin.Constants;
using WPAPlugin.Events;
using WPAPlugin.Parsers;

namespace WPAPlugin
{
    /// <summary>
    /// WperfSourceParser is the class responsible of reading the content of the .json file and creating WperfEvent events.
    /// These events are then marked as ready to be processed and passed to the DataCookers.
    /// </summary>
    public class WperfSourceParser : ISourceParser<WperfEvent, WperfSourceParser, string>
    {
        private readonly string[] timelineFilesPathList;
        private readonly string[] countFilesPathList;

        /// <summary>
        /// Timeline and count files paths lists need to be passed seperately to the <c>constructor</c>
        /// </summary>
        /// <param name="timelineFilesPathList">List of timeline file paths</param>
        /// <param name="countFilesPathList">List of single count file paths</param>
        public WperfSourceParser(string[] timelineFilesPathList, string[] countFilesPathList)
        {
            this.timelineFilesPathList = timelineFilesPathList;
            this.countFilesPathList = countFilesPathList;
        }

        public DataSourceInfo DataSourceInfo { get; private set; }
        public Type DataElementType => typeof(WperfEvent);

        public Type DataContextType => typeof(WperfSourceParser);

        public Type DataKeyType => typeof(string);

        public int MaxSourceParseCount => 1;

        public DateTime StartWallClockUtc { get; private set; }

        /// <summary>
        /// A SourceParser requires a string Id field that identifies the SourceParser and allows mapping a DataCooker with its respecitve data source.
        /// </summary>
        public string Id => WperfPluginConstants.ParserId;

        public void PrepareForProcessing(
            bool allEventsConsumed,
            IReadOnlyCollection<string> requestedDataKeys
        )
        {
            // NOOP
        }

        /// <summary>
        /// Parses content of timeline json file.
        /// </summary>
        /// <param name="dataProcessor">Instance of the data processors passed from <c>ProcessSource</c> used to
        /// mark events as ready for process.</param>
        /// <param name="cancellationToken">Cancellation token required to be passed to the data cooker</param>
        private void ProcessTimelineFiles(
            ISourceDataProcessor<WperfEvent, WperfSourceParser, string> dataProcessor,
            CancellationToken cancellationToken
        )
        {
            int filesCount = timelineFilesPathList.Length;
            if (filesCount == 0)
            {
                return;
            }

            int currentFile = 0;

            long minTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            StartWallClockUtc = new DateTime(minTimestamp).ToUniversalTime();

            double totalTimeElapsed = 0;
            double previousTimeElapsed = 0;
            foreach (string file in timelineFilesPathList)
            {
                string jsonContent = File.ReadAllText(file);
                WperfTimeline wperfTimeline = WperfTimeline.FromJson(jsonContent);

                int totalCount = wperfTimeline.Timeline.Length;
                int currentCount = 0;

                foreach (WperfCount count in wperfTimeline.Timeline)
                {
                    int coreCount = count.Core.PerformanceCounters.Length;
                    int currentCore = 0;
                    if (currentCount > 0)
                    {
                        previousTimeElapsed = totalTimeElapsed;
                    }
                    totalTimeElapsed += count.TimeElapsed;

                    if (count.Core.TsMetric.TelemetrySolutionMetrics != null)
                    {
                        foreach (
                            TelemetrySolutionMetric metric in count
                                .Core
                                .TsMetric
                                .TelemetrySolutionMetrics
                        )
                        {
                            WperfEvent countingEvent = new WperfEvent
                            {
                                CoreNumber = Int32.Parse(metric.Core),
                                Name = metric.MetricName,
                                ProductName = metric.ProductName,
                                Value = double.Parse(metric.Value),
                                Unit = metric.Unit,
                                Key = WperfPluginConstants.TelemetryEventKey,
                                StartTime = previousTimeElapsed,
                                EndTime = totalTimeElapsed,
                            };
                            _ = dataProcessor.ProcessDataElement(
                                countingEvent,
                                this,
                                cancellationToken
                            );
                        }
                    }

                    foreach (CorePerformanceCounter core in count.Core.PerformanceCounters)
                    {
                        int eventCount = core.PerformanceCounter.Length;
                        int currentEvent = 0;
                        foreach (
                            CorePerformanceCounterItem rawCountingEvent in core.PerformanceCounter
                        )
                        {
                            WperfEvent countingEvent = new WperfEvent
                            {
                                CoreNumber = core.CoreNumber,
                                Value = rawCountingEvent.CounterValue,
                                Name = rawCountingEvent.EventName,
                                Index = rawCountingEvent.EventIdx,
                                Note = rawCountingEvent.EventNote,
                                Key = WperfPluginConstants.PerformanceCounterTimelineEventKey,
                                StartTime = previousTimeElapsed,
                                EndTime = totalTimeElapsed,
                            };
                            _ = dataProcessor.ProcessDataElement(
                                countingEvent,
                                this,
                                cancellationToken
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

        /// <summary>
        /// Parses content of single count json file.
        /// </summary>
        /// <param name="dataProcessor">Instance of the data processors passed from <c>ProcessSource</c> used to
        /// mark events as ready for process.</param>
        /// <param name="cancellationToken">Cancellation token required to be passed to the data cooker</param>
        private void ProcessCountFiles(
            ISourceDataProcessor<WperfEvent, WperfSourceParser, string> dataProcessor,
            CancellationToken cancellationToken
        )
        {
            int filesCount = countFilesPathList.Length;
            if (filesCount == 0)
            {
                return;
            }

            int currentFile = 0;

            foreach (string file in countFilesPathList)
            {
                string jsonContent = File.ReadAllText(file);
                WperfCount wperfTimeline = WperfCount.FromJson(jsonContent);

                int coreCount = wperfTimeline.Core.PerformanceCounters.Length;
                int currentCore = 0;

                if (wperfTimeline.Core.TsMetric.TelemetrySolutionMetrics != null)
                {
                    foreach (var metric in wperfTimeline.Core.TsMetric.TelemetrySolutionMetrics)
                    {
                        WperfEvent countingEvent = new WperfEvent
                        {
                            CoreNumber = Int32.Parse(metric.Core),
                            Name = metric.MetricName,
                            ProductName = metric.ProductName,
                            Value = double.Parse(metric.Value),
                            Unit = metric.Unit,
                            Key = WperfPluginConstants.TelemetryEventKey
                        };
                        _ = dataProcessor.ProcessDataElement(
                            countingEvent,
                            this,
                            cancellationToken
                        );
                    }
                }

                foreach (CorePerformanceCounter core in wperfTimeline.Core.PerformanceCounters)
                {
                    int eventCount = core.PerformanceCounter.Length;
                    int currentEvent = 0;
                    foreach (CorePerformanceCounterItem rawCountingEvent in core.PerformanceCounter)
                    {
                        WperfEvent countingEvent = new WperfEvent
                        {
                            CoreNumber = core.CoreNumber,
                            Value = rawCountingEvent.CounterValue,
                            Name = rawCountingEvent.EventName,
                            Index = rawCountingEvent.EventIdx,
                            Note = rawCountingEvent.EventNote,
                            Key = WperfPluginConstants.PerformanceCounterEventKey
                        };
                        _ = dataProcessor.ProcessDataElement(
                            countingEvent,
                            this,
                            cancellationToken
                        );
                        currentEvent++;
                    }
                    currentCore++;
                }
                currentFile++;
            }
        }

        /// <summary>
        ///    A SourceParser requires a ProcessSource method to be one of its members, and is where the JSON parsing will occur.
        ///
        /// Parsed events should be instances of WperfEvent.
        /// </summary>
        /// <example>
        /// <code>
        /// public void ProcessSource(
        ///         ISourceDataProcessor<WperfEvent, WperfSourceParser, string> dataProcessor,
        ///         ILogger logger,
        ///         IProgress<int> progress,
        ///         CancellationToken cancellationToken
        ///         )
        /// {
        ///     WperfEvent countingEvent = new WperfEvent(...)
        ///
        ///     ...
        ///
        ///     _ = dataProcessor.ProcessDataElement(
        ///         countingEvent,
        ///         this,
        ///         cancellationToken
        ///         );
        ///
        /// }
        /// </code>
        /// </example>
        public void ProcessSource(
            ISourceDataProcessor<WperfEvent, WperfSourceParser, string> dataProcessor,
            ILogger logger,
            IProgress<int> progress,
            CancellationToken cancellationToken
        )
        {
            ProcessTimelineFiles(dataProcessor, cancellationToken);
            ProcessCountFiles(dataProcessor, cancellationToken);
        }
    }
}
