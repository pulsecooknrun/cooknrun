using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RoistatClient
{
    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddThh:mm:ss.msZ";
        }
    }

    public class Period
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime from { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime to { get; set; }
    }

    public class MetricItem
    {
        public string metric { get; set; }
        public string attribution { get; set; }
    }

    public class AnalyticsDataRequest
    {
        public List<string> dimensions { get; set; }
        public Period period { get; set; }
        public List<object> filters { get; set; }
        public List<string> nextDimensions { get; set; }
        public List<MetricItem> metrics { get; set; }
    }
}