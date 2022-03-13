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
        public int GamesSoldThisMonth {get; set; }
        public int AmountOfSalesThisMonth { get; set; }
        public int GameCompletedThisMonth { get; set; }
        public int GamesSoldNextMonth { get; set; }
        public int AmountOfSalesNextMonth { get; set; }
    }
}