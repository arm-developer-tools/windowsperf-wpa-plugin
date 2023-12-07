﻿// BSD 3-Clause License
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
    public class CountingEvent : IKeyedDataType<string>
    {
        public int CoreNumber { get; private set; }
        public long Value { get; private set; }
        public string EventName { get; private set; }
        public string EventIndex { get; private set; }
        public string EventNote { get; private set; }
        public double StartTime { get; private set; }
        public double EndTime { get; private set; }
        public string Key { get; private set; }

        public CountingEvent(
            string Key,
            int CoreNumber,
            long Value,
            string EventName,
            string EventIndex,
            string EventNote,
            double StartTime,
            double EndTime
        )
        {
            (
                this.Key,
                this.CoreNumber,
                this.Value,
                this.EventName,
                this.EventIndex,
                this.EventNote,
                this.StartTime,
                this.EndTime
            ) = (Key, CoreNumber, Value, EventName, EventIndex, EventNote, StartTime, EndTime);
        }

        public CountingEvent(
            string Key,
            int CoreNumber,
            long Value,
            string EventName,
            string EventIndex,
            string EventNote
        )
        {
            (
                this.Key,
                this.CoreNumber,
                this.Value,
                this.EventName,
                this.EventIndex,
                this.EventNote
            ) = (Key, CoreNumber, Value, EventName, EventIndex, EventNote);
        }

        public CountingEvent(CountingEvent countingEvent)
        {
            (Key, CoreNumber, Value, EventName, EventIndex, EventNote, StartTime, EndTime) = (
                countingEvent.Key,
                countingEvent.CoreNumber,
                countingEvent.Value,
                countingEvent.EventName,
                countingEvent.EventIndex,
                countingEvent.EventNote,
                countingEvent.StartTime,
                countingEvent.EndTime
            );
        }

        public string GetKey()
        {
            return Key;
        }
    }
}
