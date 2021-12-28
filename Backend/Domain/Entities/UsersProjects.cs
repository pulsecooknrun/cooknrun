using System;

namespace CookRun.Domain.Entities
{
    public class UsersProjects
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}