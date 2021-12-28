using System;

namespace CookRun.Domain.Entities
{
    public class JobReport
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }
}