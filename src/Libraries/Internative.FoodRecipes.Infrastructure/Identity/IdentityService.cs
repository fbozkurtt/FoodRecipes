using Internative.FoodRecipes.Application.Common.Exceptions;
using Internative.FoodRecipes.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration _configuration;

        public IdentityService(
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _configuration = configuration;
        }

        public async Task<string> GetUserNameAsync(int userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<int> CreateUserAsync(string userName, string password)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
                throw new AlreadyExistsException(nameof(IdentityUser), nameof(IdentityUser.UserName), userName);

            var user = new ApplicationUser
            {
                UserName = userName,
            };

            await _userManager.CreateAsync(user, password);

            return user.Id;
        }

        public async Task<bool> IsInRoleAsync(int userId, string role)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(int userId, string policyName)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return false;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return false;
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<SecurityToken> GetTokenAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();

                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

                foreach (var role in userRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["JWT:Expires"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(_configuration["JWT:Secret"])), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["JWT:Issuer"],
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return token;
            }

            throw new UnauthorizedAccessException();
        }


        public async Task<IdentityUser<int>> GetUserAsync(int userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<IdentityUser<int>> GetUserAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
    }
}
