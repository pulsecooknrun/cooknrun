using System;

namespace CookRun.Domain.Entities
{
    public class RoistatMeanReport
    {
        public Guid Id { get; set; }
        public Guid JobReportId { get; set; }
        public string ProjectName { get; set; }
        public double Visits { get; set; }
        public double VisitCount { get; set; }
        public double ConversionVisitsToLeads { get; set; }
        public double Leads { get; set; }
        public double McLeads { get; set; }
        public double ConversionLeadsToSales { get; set; }
        public double Sales { get; set; }
        public double Revenue { get; set; }
        public double AverageSale { get; set; }
        public double Profit { get; set; }
        public double MarketingCost { get; set; }
        public double Roi { get; set; }
        public double NetCost { get; set; }
    }
}