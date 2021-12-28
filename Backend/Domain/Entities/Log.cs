using System;

namespace CookRun.Domain.Entities
{
    public class Log
    {
        public Log(string message)
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
            Message = message;
        }

        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
    }
}