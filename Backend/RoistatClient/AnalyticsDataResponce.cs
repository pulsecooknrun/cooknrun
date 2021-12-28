using System;
using System.Collections.Generic;

namespace RoistatClient
{
    public class Metric
    {
        public double value { get; set; }
        public object formatted { get; set; }
        public string metric_name { get; set; }
        public string attribution_model_id { get; set; }
    }

    public class MarkerLevel1
    {
        public string value { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
    }

    public class Dimensions
    {
        public MarkerLevel1 marker_level_1 { get; set; }
    }

    public class Item
    {
        public List<Metric> metrics { get; set; }
        public Dimensions dimensions { get; set; }
        public int isHasChild { get; set; }
    }

    public class Mean
    {
        public List<Metric> metrics { get; set; }
        public List<object> dimensions { get; set; }
        public int isHasChild { get; set; }
    }

    public class Unprocessed
    {
        public int visits { get; set; }
        public int leads { get; set; }
    }

    public class Datum
    {
        public List<Item> items { get; set; }
        public Mean mean { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public Unprocessed unprocessed { get; set; }
    }

    public class AnalyticsDataResponce
    {
        public List<Datum> Data { get; set; }
        public string Status { get; set; }
    }
}