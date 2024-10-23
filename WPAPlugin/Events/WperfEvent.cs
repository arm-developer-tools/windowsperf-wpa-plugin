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

using Microsoft.Performance.SDK.Extensibility;

namespace WPAPlugin.Events
{
    /// <summary>
    /// WperfEvent is a class that stores WindowsPerf events.
    /// In order to keep the plugin structure straight forward and not resorting to multiple
    /// SourceParsers, counting events and timeline events are stored in WperfEvent instances
    /// by sharing their common properties and ignoring unrelated fields.
    /// </summary>
    public class WperfEvent : IKeyedDataType<string>
    {
        // Common fields

        /// <summary>
        /// int representing the core number of said event.
        /// </summary>
        public int CoreNumber { get; set; }

        /// <summary>
        /// double represeting the value of the count/telemetry.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// string representing name of the event/metric.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// double representing the start time of the event.
        /// </summary>
        public double StartTime { get; set; }

        /// <summary>
        /// double representing the end time of the event.
        /// </summary>
        public double EndTime { get; set; }

        /// <summary>
        /// string used in filtering the events by the DataCookers.
        /// </summary>
        public string Key { get; set; }

        // Counting fields
        /// <summary>
        /// string representing the counting event index.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// string representing the counting event note.
        /// </summary>
        public string Note { get; set; }

        // Telemetry Fields

        /// <summary>
        /// string representing the metric unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// string representing the metric product name.
        /// </summary>
        public string ProductName { get; set; }

        public WperfEvent() { }

        public WperfEvent(WperfEvent countingEvent)
        {
            CoreNumber = countingEvent.CoreNumber;
            Value = countingEvent.Value;
            Name = countingEvent.Name;
            StartTime = countingEvent.StartTime;
            EndTime = countingEvent.EndTime;
            Key = countingEvent.Key;
            Index = countingEvent.Index;
            Note = countingEvent.Note;
            Unit = countingEvent.Unit;
            ProductName = countingEvent.ProductName;
        }

        public string GetKey()
        {
            return Key;
        }
    }
}
