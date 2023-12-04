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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace WPAPlugin.Parsers
{
    public partial class WperfStats
    {
        [JsonProperty("core", Required = Required.Always)]
        public Core Core { get; set; }

        [JsonProperty("dmc", Required = Required.Always)]
        public Dmc Dmc { get; set; }

        [JsonProperty("dsu", Required = Required.Always)]
        public Dsu Dsu { get; set; }

        [JsonProperty(
            "time_elapsed",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public double TimeElapsed { get; set; }
    }

    public partial class Core
    {
        [JsonProperty("kernel_mode", Required = Required.Always)]
        public bool KernelMode { get; set; }

        [JsonProperty("multiplexing", Required = Required.Always)]
        public bool Multiplexing { get; set; }

        [JsonProperty("overall", Required = Required.Always)]
        public CoreOverall Overall { get; set; }

        [JsonProperty("cores", Required = Required.Always)]
        public CorePerformanceCounter[] PerformanceCounters { get; set; }

        [JsonProperty(
            "ts_metric",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public TsMetric TsMetric { get; set; }
    }

    public partial class CoreOverall
    {
        [JsonProperty(
            "Systemwide_Overall_Performance_Counters",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public CoreSystemwideOverallPerformanceCounter[] SystemwideOverallPerformanceCounters { get; set; }
    }

    public partial class CoreSystemwideOverallPerformanceCounter
    {
        [JsonProperty(
            "counter_value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? CounterValue { get; set; }

        [JsonProperty(
            "event_idx",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventIdx { get; set; }

        [JsonProperty(
            "event_name",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventName { get; set; }

        [JsonProperty(
            "event_note",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventNote { get; set; }

        [JsonProperty(
            "multiplexed",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Multiplexed { get; set; }

        [JsonProperty(
            "scaled_value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? ScaledValue { get; set; }
    }

    public partial class CorePerformanceCounter
    {
        [JsonProperty("core_number", Required = Required.Always)]
        public int CoreNumber { get; set; }

        [JsonProperty("Performance_counter", Required = Required.Always)]
        public CorePerformanceCounterItem[] PerformanceCounter { get; set; }
    }

    public partial class CorePerformanceCounterItem
    {
        [JsonProperty("counter_value", Required = Required.Always)]
        public long CounterValue { get; set; }

        [JsonProperty("event_idx", Required = Required.Always)]
        public string EventIdx { get; set; }

        [JsonProperty("event_name", Required = Required.Always)]
        public string EventName { get; set; }

        [JsonProperty("event_note", Required = Required.Always)]
        public string EventNote { get; set; }

        [JsonProperty(
            "multiplexed",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Multiplexed { get; set; }

        [JsonProperty(
            "scaled_value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? ScaledValue { get; set; }
    }

    public partial class TsMetric
    {
        [JsonProperty(
            "telemetry_solution_metrics",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public TelemetrySolutionMetric[] TelemetrySolutionMetrics { get; set; }
    }

    public partial class TelemetrySolutionMetric
    {
        [JsonProperty(
            "core",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Core { get; set; }

        [JsonProperty(
            "metric_name",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string MetricName { get; set; }

        [JsonProperty(
            "product_name",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string ProductName { get; set; }

        [JsonProperty(
            "unit",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Unit { get; set; }

        [JsonProperty(
            "value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Value { get; set; }
    }

    public partial class Dmc
    {
        [JsonProperty("ddr", Required = Required.Always)]
        public Ddr Ddr { get; set; }

        [JsonProperty("pmu", Required = Required.Always)]
        public Pmu Pmu { get; set; }
    }

    public partial class Ddr
    {
        [JsonProperty(
            "DDR_metrics",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public DdrMetric[] DdrMetrics { get; set; }
    }

    public partial class DdrMetric
    {
        [JsonProperty(
            "channel",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Channel { get; set; }

        [JsonProperty(
            "event_name",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventName { get; set; }

        [JsonProperty(
            "event_note",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventNote { get; set; }

        [JsonProperty(
            "pmu_id",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string PmuId { get; set; }

        [JsonProperty(
            "rw_bandwidth",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string RwBandwidth { get; set; }
    }

    public partial class Pmu
    {
        [JsonProperty(
            "PMU_performance_counters",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public PmuPerformanceCounter[] PmuPerformanceCounters { get; set; }
    }

    public partial class PmuPerformanceCounter
    {
        [JsonProperty(
            "counter_value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? CounterValue { get; set; }

        [JsonProperty(
            "event_idx",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventIdx { get; set; }

        [JsonProperty(
            "event_name",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventName { get; set; }

        [JsonProperty(
            "event_note",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventNote { get; set; }

        [JsonProperty(
            "pmu_id",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string PmuId { get; set; }
    }

    public partial class Dsu
    {
        [JsonProperty("l3metric", Required = Required.Always)]
        public L3Metric L3Metric { get; set; }

        [JsonProperty("overall", Required = Required.Always)]
        public DsuOverall Overall { get; set; }

        [JsonProperty(
            "cores",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public DsuPerformanceCounter[] PerformanceCounters { get; set; }
    }

    public partial class L3Metric
    {
        [JsonProperty(
            "L3_cache_metrics",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public L3CacheMetric[] L3CacheMetrics { get; set; }
    }

    public partial class L3CacheMetric
    {
        [JsonProperty(
            "cluster",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Cluster { get; set; }

        [JsonProperty(
            "cores",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Cores { get; set; }

        [JsonProperty(
            "miss_rate",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string MissRate { get; set; }

        [JsonProperty(
            "read_bandwidth",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string ReadBandwidth { get; set; }
    }

    public partial class DsuOverall
    {
        [JsonProperty(
            "Systemwide_Overall_Performance_Counters",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public DsuSystemwideOverallPerformanceCounter[] SystemwideOverallPerformanceCounters { get; set; }
    }

    public partial class DsuSystemwideOverallPerformanceCounter
    {
        [JsonProperty(
            "counter_value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? CounterValue { get; set; }

        [JsonProperty(
            "event_idx",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventIdx { get; set; }

        [JsonProperty(
            "event_name",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventName { get; set; }

        [JsonProperty(
            "event_note",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string EventNote { get; set; }

        [JsonProperty(
            "multiplexed",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Multiplexed { get; set; }

        [JsonProperty(
            "scaled_value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? ScaledValue { get; set; }
    }

    public partial class DsuPerformanceCounter
    {
        [JsonProperty("core_number", Required = Required.Always)]
        public int CoreNumber { get; set; }

        [JsonProperty("Performance_counter", Required = Required.Always)]
        public DsuPerformanceCounterItem[] PerformanceCounter { get; set; }
    }

    public partial class DsuPerformanceCounterItem
    {
        [JsonProperty("counter_value", Required = Required.Always)]
        public long CounterValue { get; set; }

        [JsonProperty("event_idx", Required = Required.Always)]
        public string EventIdx { get; set; }

        [JsonProperty("event_name", Required = Required.Always)]
        public string EventName { get; set; }

        [JsonProperty("event_note", Required = Required.Always)]
        public string EventNote { get; set; }

        [JsonProperty(
            "multiplexed",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Multiplexed { get; set; }

        [JsonProperty(
            "scaled_value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? ScaledValue { get; set; }
    }

    public partial class WperfStats
    {
        public static WperfStats FromJson(string json)
        {
            return JsonConvert.DeserializeObject<WperfStats>(
                json,
                WPAPlugin.Parsers.Converter.Settings
            );
        }
    }

    public static class Serialize
    {
        public static string ToJson(this WperfStats self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
