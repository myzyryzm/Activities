using System.Linq;
using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security {
    public class UserAccessor : IUserAccessor {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor (IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;

        }

        public string GetCurrentUsername()
        {
            //have obj user inside httpcontext want to get first or default user that matches the name identifier
            var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return username;
        }
    }
}