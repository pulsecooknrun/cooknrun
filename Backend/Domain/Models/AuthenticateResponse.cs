using CookRun.Domain.Entities;
using System;

namespace CookRun.Domain.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }

        public AuthenticateResponse(ApplicationUser user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}