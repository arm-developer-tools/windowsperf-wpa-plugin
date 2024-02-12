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

namespace WPAPlugin.Schemas
{
    static class JsonSchemas
    {
        public const string CountSchema = "{\"type\":\"object\",\"required\":[\"core\",\"dsu\",\"dmc\"],\"properties\":{\"core\":{\"type\":\"object\",\"required\":[\"Multiplexing\",\"Kernel_mode\",\"overall\",\"cores\"],\"minProperties\":4,\"additionalProperties\":false,\"properties\":{\"cores\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"core_number\",\"Performance_counter\"],\"properties\":{\"core_number\":{\"type\":\"integer\"},\"Performance_counter\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"counter_value\",\"event_idx\",\"event_name\",\"event_note\"],\"additionalProperties\":false,\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}}},\"Multiplexing\":{\"type\":\"boolean\"},\"Kernel_mode\":{\"type\":\"boolean\"},\"overall\":{\"type\":\"object\",\"properties\":{\"Systemwide_Overall_Performance_Counters\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}},\"ts_metric\":{\"type\":\"object\",\"properties\":{\"telemetry_solution_metrics\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"properties\":{\"core\":{\"type\":\"string\"},\"product_name\":{\"type\":\"string\"},\"metric_name\":{\"type\":\"string\"},\"value\":{\"type\":\"string\"},\"unit\":{\"type\":\"string\"}}}}}}}},\"dsu\":{\"type\":\"object\",\"required\":[\"l3metric\",\"overall\"],\"additionalProperties\":false,\"properties\":{\"cores\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"core_number\",\"Performance_counter\"],\"properties\":{\"core_number\":{\"type\":\"integer\"},\"Performance_counter\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"counter_value\",\"event_idx\",\"event_name\",\"event_note\"],\"additionalProperties\":false,\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}}},\"l3metric\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"L3_cache_metrics\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"cluster\":{\"type\":\"string\"},\"miss_rate\":{\"type\":\"string\"},\"read_bandwidth\":{\"type\":\"string\"},\"cores\":{\"type\":\"string\"}}}}}},\"overall\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"Systemwide_Overall_Performance_Counters\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}}}},\"dmc\":{\"type\":\"object\",\"required\":[\"pmu\",\"ddr\"],\"additionalProperties\":false,\"properties\":{\"pmu\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"PMU_performance_counters\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"pmu_id\":{\"type\":\"string\"}}}}}},\"ddr\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"DDR_metrics\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"channel\":{\"type\":\"string\"},\"rw_bandwidth\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"pmu_id\":{\"type\":\"string\"}}}}}}}},\"time_elapsed\":{\"type\":\"number\"}}}";
        public const string TimelineSchema = "{\"type\":\"object\",\"required\":[\"timeline\"],\"properties\":{\"timeline\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"core\",\"dsu\",\"dmc\"],\"properties\":{\"core\":{\"type\":\"object\",\"required\":[\"Multiplexing\",\"Kernel_mode\",\"overall\",\"cores\"],\"minProperties\":4,\"additionalProperties\":false,\"properties\":{\"cores\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"core_number\",\"Performance_counter\"],\"properties\":{\"core_number\":{\"type\":\"integer\"},\"Performance_counter\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"counter_value\",\"event_idx\",\"event_name\",\"event_note\"],\"additionalProperties\":false,\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}}},\"Multiplexing\":{\"type\":\"boolean\"},\"Kernel_mode\":{\"type\":\"boolean\"},\"overall\":{\"type\":\"object\",\"properties\":{\"Systemwide_Overall_Performance_Counters\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}},\"ts_metric\":{\"type\":\"object\",\"properties\":{\"telemetry_solution_metrics\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"properties\":{\"core\":{\"type\":\"string\"},\"product_name\":{\"type\":\"string\"},\"metric_name\":{\"type\":\"string\"},\"value\":{\"type\":\"string\"},\"unit\":{\"type\":\"string\"}}}}}}}},\"dsu\":{\"type\":\"object\",\"required\":[\"l3metric\",\"overall\"],\"additionalProperties\":false,\"properties\":{\"cores\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"core_number\",\"Performance_counter\"],\"properties\":{\"core_number\":{\"type\":\"integer\"},\"Performance_counter\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"required\":[\"counter_value\",\"event_idx\",\"event_name\",\"event_note\"],\"additionalProperties\":false,\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}}},\"l3metric\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"L3_cache_metrics\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"cluster\":{\"type\":\"string\"},\"miss_rate\":{\"type\":\"string\"},\"read_bandwidth\":{\"type\":\"string\"},\"cores\":{\"type\":\"string\"}}}}}},\"overall\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"Systemwide_Overall_Performance_Counters\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"multiplexed\":{\"type\":\"string\"},\"scaled_value\":{\"type\":\"integer\"}}}}}}}},\"dmc\":{\"type\":\"object\",\"required\":[\"pmu\",\"ddr\"],\"additionalProperties\":false,\"properties\":{\"pmu\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"PMU_performance_counters\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"counter_value\":{\"type\":\"integer\"},\"event_idx\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"pmu_id\":{\"type\":\"string\"}}}}}},\"ddr\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"DDR_metrics\":{\"type\":\"array\",\"items\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"channel\":{\"type\":\"string\"},\"rw_bandwidth\":{\"type\":\"string\"},\"event_name\":{\"type\":\"string\"},\"event_note\":{\"type\":\"string\"},\"pmu_id\":{\"type\":\"string\"}}}}}}}},\"time_elapsed\":{\"type\":\"number\"}}}}}}";
        public enum Schemas
        {
            CountSchema,
            TimelineSchema
        }

        public static string GetSchemaByKey(Schemas key)
        {
            switch (key)
            {
                case Schemas.CountSchema:
                    return CountSchema;
                case Schemas.TimelineSchema:
                    return TimelineSchema;

                default:
                    throw new Exception($"Case {key} not found.");
            }
        }
    }
}
