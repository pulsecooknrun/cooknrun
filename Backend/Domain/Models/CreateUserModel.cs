using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CookRun.Domain.Models
{
    public class CreateUserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public List<string> Projects { get; set; }
        public bool IsAdmin { get; set; }
    }
}