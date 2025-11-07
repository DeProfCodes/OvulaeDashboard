using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OvulaeShared.Enums.User;
using OvulaeShared.Models.WebApi;

namespace OvulaeDashboard.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _context;

        public SessionService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public UserRoleType GetUserRole()
        {
            var userRole = _context.HttpContext?.Session.GetString("LoggedInUserRole");

            return EnumHelper.GetEnumValueFromName<UserRoleType>(userRole);
        }

        public void SetUserRole(UserRoleType userRole)
        {
            _context.HttpContext?.Session.SetString("LoggedInUserRole", userRole.GetDisplayName());
        }

        public string GetUserId()
        {
            return _context.HttpContext?.Session.GetString("LoggedInUserId");
        }

        public void SetUserId(string userId)
        {
            _context.HttpContext?.Session.SetString("LoggedInUserId", userId);
        }

        public string GetUserEmail()
        {
            return _context.HttpContext?.Session.GetString("LoggedInUserEmail");
        }

        public void SetUserEmail(string email)
        {
            _context.HttpContext?.Session.SetString("LoggedInUserEmail", email);
        }

        public string GetUserFullname()
        {
            return _context.HttpContext?.Session.GetString("LoggedInUserFullname");
        }

        public void SetUserFullname(string fullname)
        {
            _context.HttpContext?.Session.SetString("LoggedInUserFullname", fullname);
        }

        public void SetUserDetails(string fullname, string userId, string email, UserRoleType userRole)
        {
            SetUserFullname(fullname);
            SetUserEmail(email);
            SetUserId(userId);
            SetUserRole(userRole);
        }

        public void SetDashboardCurrency(bool isRand)
        {
            _context.HttpContext?.Session.SetString("LoggedInUserCurrency", isRand ? "R" : "$");
        }

        public string GetDashboardCurrency()
        {
            return _context.HttpContext?.Session.GetString("LoggedInUserCurrency");
        }

        public SecureApiRequest GetSecureApiRequestDto()
        {
            return new SecureApiRequest
            {
                UserId = GetUserId(),
                Email = GetUserEmail(),
                UserRole = GetUserRole()
            };
        }
    }
}
