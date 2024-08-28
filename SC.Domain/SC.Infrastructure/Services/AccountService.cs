using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SC.Application.Common.Interfaces;
using SC.Application.Common.ViewModels;
using SC.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace SC.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AccountService(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }
        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            var existingUser = await _accountRepository.FindByUsernameAsync(model.Username);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Username already exists" });
            }

            var newUser = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            return await _accountRepository.CreateUserAsync(newUser, model.Password);
        }

        public async Task<string?> LoginAsync(LoginViewModel request)
        {
            var user = await _accountRepository.FindByUsernameAsync(request.username);
            if (user == null || !await _accountRepository.CheckPasswordAsync(user, request.password))
            {
                return null;
            }
            var tokenString = await GenerateJwtTokenAsync(user);
            return tokenString;
        }

        public Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("UserName", user.UserName),
            new Claim("Email", user.Email)
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
        public async Task<UserViewModel> GetGoogleUserInfoAsync(string token)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={token}");
            return JsonConvert.DeserializeObject<UserViewModel>(response);
        }

        public async Task<UserViewModel> GetFacebookUserInfoAsync(string token)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"https://graph.facebook.com/me?fields=id,name,email&access_token={token}");
            if (response == null)
            {
                throw new BadHttpRequestException("Cannot get user info from token");
            }

            return JsonConvert.DeserializeObject<UserViewModel>(response);
        }
        public async Task<string?> HandleLoginWithGoogleAsync(TokenViewModel request)
        {
            var token = request.Token;
            var userInfo = await GetGoogleUserInfoAsync(token);

            if (userInfo == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            var existingUser = await _accountRepository.FindByUsernameAsync(userInfo.Email);

            if (existingUser == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = userInfo.Email,
                    Email = userInfo.Email
                };

                var createResult = await _accountRepository.CreateUserAsync(newUser);

                if (!createResult.Succeeded)
                {
                    throw new ApplicationException("User creation failed");
                }

                existingUser = newUser;
            }

            var tokenString = await GenerateJwtTokenAsync(existingUser);
            return tokenString;
        }

        public async Task<string?> HandleLoginWithFacebookAsync(TokenViewModel request)
        {
            var token = request.Token;
            var userInfo = await GetFacebookUserInfoAsync(token);

            if (userInfo == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            var existingUser = await _accountRepository.FindByUsernameAsync(userInfo.Email);

            if (existingUser == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = userInfo.Email,
                    Email = userInfo.Email
                };

                var createResult = await _accountRepository.CreateUserAsync(newUser);

                if (!createResult.Succeeded)
                {
                    throw new ApplicationException("User creation failed");
                }

                existingUser = newUser;
            }

            var tokenString = await GenerateJwtTokenAsync(existingUser);
            return tokenString;
        }
    }
}
