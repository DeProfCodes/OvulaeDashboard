using OvulaeShared.Enums.User;
using OvulaeShared.Models.WebApi;

namespace OvulaeDashboard.Services
{
    public interface ISessionService
    {
        public UserRoleType GetUserRole();
        public void SetUserRole(UserRoleType userRole);

        public string GetUserId();
        public void SetUserId(string userId);

        public string GetUserEmail();
        public void SetUserEmail(string email);

        public void SetUserDetails(string fullname, string userId, string email, UserRoleType userRole);

        public SecureApiRequest GetSecureApiRequestDto();

        public string GetUserFullname();
        public void SetUserFullname(string fullname);

        public void SetDashboardCurrency(bool isRand);
        public string GetDashboardCurrency();

    }
}
