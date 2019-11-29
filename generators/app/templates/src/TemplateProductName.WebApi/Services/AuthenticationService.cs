using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TemplateProductName.Domain.Model;
using TemplateProductName.Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace TemplateProductName.WebApi.Services
{
    public class AuthenticationService : Domain.Services.IAuthenticationService
    {
        private IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// See: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-3.0
        /// </summary>
        /// <param name="user"></param>
        public void HttpSignIn(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {

            };

            httpContextAccessor
                .HttpContext
                .SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
                .Wait();

            // Send a cookie without HttpOnly so we can access it client side.
            // This is just a token used for optimising authentication stuff on
            // first page load. If this cookie is not present client-side, there
            // is absolutely no chance that the user is logged in. So we won't
            // hit the backend to get the user details - saves pounding the
            // backend by guest traffic.
            httpContextAccessor
                .HttpContext
                .Response
                .Cookies
                .Append("IsSignedIn", "true");
        }

        public void HttpSignOut()
        {
            httpContextAccessor
                .HttpContext
                .SignOutAsync();

            httpContextAccessor
                .HttpContext
                .Response
                .Cookies
                .Delete("IsSignedIn");
        }

        public bool IsAuthenticated()
        {
            var user = httpContextAccessor.HttpContext.User;
            var claims = user.Claims;
            return user.Identity.IsAuthenticated && user.HasClaim(x => x.Type == ClaimTypes.NameIdentifier);
        }

        public UserModel GetCurrentUser()
        {
            if(!IsAuthenticated())
            {
                return null;
            }

            var user = httpContextAccessor.HttpContext.User;

            
            return new UserModel
            {
                Id = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)),
                Email = user.FindFirstValue(ClaimTypes.Email)
            };
        }

        private static string GetValue(IEnumerable<Claim> claims, string type) => claims.Single(x => x.Type == type).Value;
    }
}
