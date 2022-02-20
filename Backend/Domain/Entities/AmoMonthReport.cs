using System;

namespace CookRun.Domain.Entities
{
    public class AmoMonthReport
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime Date { get; set; }
        public int Leads { get; set; }
        public int Closed { get; set; }
        public int Sales { get; set; }
        public int CorrectLeads { get; set; }
    }
}