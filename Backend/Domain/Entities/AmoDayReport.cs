using System;

namespace CookRun.Domain.Entities
{
    public class AmoDayReport
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime Date { get; set; }
        public int Leads { get; set; }
        public int Closed { get; set; }
        public int LeadsWithoutTasks { get; set; }
        public int Overdue { get; set; }
        public int Forehead { get; set; }
        public int Sales { get; set; }
    }
}