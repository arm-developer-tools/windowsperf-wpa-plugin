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

using Microsoft.Performance.SDK.Extensibility;

namespace WPAPlugin.Events
{
    public class WperfEvent : IKeyedDataType<string>
    {
        // Common fields
        public int CoreNumber { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string Key { get; set; }

        // Counting fields
        public string Index { get; set; }
        public string Note { get; set; }

        // Telemetry Fields
        public string Unit { get; set; }
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
