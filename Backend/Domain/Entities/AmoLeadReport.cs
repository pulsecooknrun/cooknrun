using System;
using System.Collections.Generic;

namespace CookRun.Domain.Entities
{
    public class AmoLeadReport
    {
        public Guid Id { get; set; }
        public Guid JobReportId { get; set; }
        public string ParentName { get; set; }
        public int LeadId { get; set; }
        public string NameText { get; set; }
        public string NameUrl { get; set; }
        public int Status { get; set; }
        public int Budget { get; set; }
        public bool CanEdit { get; set; }
        public int MainContactId { get; set; }
        public string MainContactName { get; set; }
        public string MainContactUrl { get; set; }
        public int CompanyNameId { get; set; }
        public string CompanyNameName { get; set; }
        public string CompanyNameUrl { get; set; }
        public string SourceDataName { get; set; }
        public DateTime DateCreate { get; set; }
        public string LeadMark { get; set; }
        public List<AmoLeadReportTag> Tags { get; set; } = new List<AmoLeadReportTag>();
    }


    public class AmoLeadReportTag
    {
        public Guid Id { get; set; }
        public int TagId { get; set; }
        public string Label { get; set; }
    }
}