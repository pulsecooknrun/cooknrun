using System;
using System.Collections.Generic;

namespace AmoClient
{
    public class Lead
    {
        public string ParentName { get; set; }

        public int Id { get; set; }
        public string NameText { get; set; }
        public string NameUrl { get; set; }
        public int Status { get; set; }
        public int Budget { get; set; }
        public bool CanEdit { get; set; }

        public int main_contact_id { get; set; }
        public string main_contact_name { get; set; }
        public string main_contact_url { get; set; }

        public int company_name_id { get; set; }
        public string company_name_name { get; set; }
        public string company_name_url { get; set; }

        public string source_data_name { get; set; }

        public List<LeadTag> Tags { get; set; } = new List<LeadTag>();


        public DateTime date_create { get; set; }
        public string lead_mark { get; set; }
    }

    public class LeadTag
    {
        public int Id { get; set; }
        public string Label { get; set; }
    }
}