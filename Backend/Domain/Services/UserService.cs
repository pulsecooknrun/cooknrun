using CookRun.Domain.Entities;
using CookRun.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CookRun.Domain.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        List<UserModel> GetUsers();
        Task<Guid> CreateUser(CreateUserModel createUserModel);
        Task DeleteUser(string userName);
        //AuthenticateResponse RefreshToken(string token, string ipAddress);
        //bool RevokeToken(string token, string ipAddress);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;

        public UserService(
            ApplicationContext applicationContext,
            UserManager<ApplicationUser> userManager,
            IOptions<AppSettings> appSettings)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = _applicationContext.Users.SingleOrDefault(x => x.UserName == model.Username);
            if (user == null) return null;

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return null;

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

            // save refresh token
            //user.RefreshTokens.Add(refreshToken);
            //_context.Update(user);
            //_context.SaveChanges();

            //return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
            return new AuthenticateResponse(user, jwtToken, null);
        }

        public List<UserModel> GetUsers()
        {
            return _userManager.Users
                .ToList()
                .Select(x => new UserModel
                {
                    Username = x.UserName,
                    Email = x.Email
                })
                .ToList();
        }

        public async Task<Guid> CreateUser(CreateUserModel createUserModel)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email
            };
            var identityResult = await _userManager.CreateAsync(applicationUser, createUserModel.Password);
            if (!identityResult.Succeeded)
                return Guid.Empty;

            return applicationUser.Id;
        }

        public async Task DeleteUser(string userName)
        {
            var applicationUser = await _userManager.FindByNameAsync(userName);
            if (applicationUser == null)
                throw new Exception("User not found");
            await _userManager.DeleteAsync(applicationUser);
        }

        //        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        //        {
        //            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        //            // return null if no user found with token
        //            if (user == null) return null;

        //            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        //            // return null if token is no longer active
        //            if (!refreshToken.IsActive) return null;

        //            // replace old refresh token with a new one and save
        //            var newRefreshToken = generateRefreshToken(ipAddress);
        //            refreshToken.Revoked = DateTime.UtcNow;
        //            refreshToken.RevokedByIp = ipAddress;
        //            refreshToken.ReplacedByToken = newRefreshToken.Token;
        //            user.RefreshTokens.Add(newRefreshToken);
        //            _context.Update(user);
        //            _context.SaveChanges();

        //            // generate new jwt
        //            var jwtToken = generateJwtToken(user);

        //            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        //        }

        //        public bool RevokeToken(string token, string ipAddress)
        //        {
        //            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        //            // return false if no user found with token
        //            if (user == null) return false;

        //            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        //            // return false if token is not active
        //            if (!refreshToken.IsActive) return false;

        //            // revoke token and save
        //            refreshToken.Revoked = DateTime.UtcNow;
        //            refreshToken.RevokedByIp = ipAddress;
        //            _context.Update(user);
        //            _context.SaveChanges();

        //            return true;
        //        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}