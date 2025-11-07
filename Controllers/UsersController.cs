using OvulaeDashboard.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using OvulaeShared.Services.APIs.Users;
using OvulaeShared.ViewModel.User;
using OvulaeShared.Enums.User;
using OvulaeShared.Enums.Status;

namespace OvulaeDashboard.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUsersApi _usersApi;
        
        public UsersController(IUsersApi usersApi)
        {
            _usersApi = usersApi;
        }

        public async Task<IActionResult> UsersManagement()
        {
            var result = new List<UserAdminDetailsViewModel>();
            try
            {
                var data = await _usersApi.GetAllUsersAdmin();

                foreach (var userInfo in data)
                {
                    var item = new UserAdminDetailsViewModel
                    {
                        Email = userInfo.UserDetails.Email,
                        FullName = $"{userInfo.UserDetails.Firstname} {userInfo.UserDetails.Lastname}",
                        UserRole = userInfo.UserDetails.UserRole,
                        CountryCode = userInfo.UserDetails.CountryCode,
                        PhoneNumber = $"({userInfo.UserDetails.CountryCode}) {userInfo.UserDetails.PhoneNumber}",
                        AccountStatus = userInfo.UserDetails.AccountStatus != AccountStatusType.None ? userInfo.UserDetails.AccountStatus : AccountStatusType.Active,
                        JoinDate = userInfo.UserDetails.CreateDate.ToString("dd-MM-yyyy"),
                        SubscriptionJoinDate = userInfo.UserSubscription != null ? userInfo.UserSubscription.ActiveDate.ToString("dd-MM-yyyy") : "n.a"
                    };
                    result.Add(item);   
                }
            }
            catch
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return PartialView(AppPagesViewsUrl.UsersManagementLink, result);
        }

        public async Task<IActionResult> PremiumUsers()
        {
            var result = new List<UserAdminDetailsViewModel>();
            try
            {
                var data = await _usersApi.GetAllRoleUsers(UserRoleType.Client);
                result = data;
            }
            catch
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return PartialView(AppPagesViewsUrl.PremiumUsersLink, result);
        }

        public async Task<IActionResult> AffiliateUsers()
        {
            var result = new List<UserAdminAffiliateDetailsViewModel>();
            try
            {
                var data = await _usersApi.GetAllAffiliateUsers(UserRoleType.Affiliate);
                result = data;
            }
            catch
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return PartialView(AppPagesViewsUrl.AffiliateUsersLink, result);
        }

        public async Task<IActionResult> DoctorUsers()
        {
            var result = new List<UserAdminAffiliateDetailsViewModel>();
            try
            {
                var data = await _usersApi.GetAllAffiliateUsers(UserRoleType.Doctor);
                result = data;
            }
            catch
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return PartialView(AppPagesViewsUrl.DoctorUsersLink, result);
        }

        public async Task<IActionResult> PendingApprovalUsers()
        {
            var result = new List<UserAdminDetailsViewModel>();
            try
            {
                var data = await _usersApi.GetAllPendingAffiliates();
                result = data;
            }
            catch
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return PartialView(AppPagesViewsUrl.PendingApprovalUsersLink, result);
        }

    }
}
