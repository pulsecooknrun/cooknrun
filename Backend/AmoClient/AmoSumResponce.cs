using System.Collections.Generic;

namespace AmoClient
{
    public class LeadsByStatus
    {
        public int status { get; set; }
        public int count { get; set; }
        public int total_price { get; set; }
        public string leads_numeral { get; set; }
        public string price_formated { get; set; }
        public int type { get; set; }
    }

    public class AmoSumResponce
    {
        public List<LeadsByStatus> leads_by_status { get; set; }
        public int all_count { get; set; }
    }
}