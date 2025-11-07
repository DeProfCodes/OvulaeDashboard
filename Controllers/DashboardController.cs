using OvulaeDashboard.Helpers.Constants;
using OvulaeDashboard.Helpers.Enums;
using OvulaeDashboard.ViewModels.Dashboard;
using OvulaeDashboard.ViewModels.Layouts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;
using OvulaeShared.Enums.User;
using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.Transactions;
using OvulaeShared.Enums;
using OvulaeShared.Services.APIs.Users;
using OvulaeDashboard.Services;

namespace OvulaeDashboard.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private readonly IUsersApi _usersApi;
        private readonly ISessionService _session;

        public DashboardController(IUsersApi usersApi, ISessionService session)
        {
            _usersApi = usersApi;
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userRole = _session.GetUserRole();

                AppPageType startPage = AppPageType.None;

                if (userRole == UserRoleType.Admin) startPage = AppPageType.AdminDashboard;
                if (userRole == UserRoleType.Affiliate) startPage = AppPageType.AffiliateDashboard;
                if (userRole == UserRoleType.AffiliateManager) startPage = AppPageType.AffiliateManagerDashboard;
                if (userRole == UserRoleType.Doctor) startPage = AppPageType.DoctorDashboard;

                var dashboardVM = new DashboardViewModel
                {
                    TransactionType = TransactionType.None,
                    Status = StatusType.None,
                    UserRole = userRole,
                    StartPage = startPage,
                };

                return View(dashboardVM);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Authentication");
            }
        }

        public async Task<IActionResult> LeftNavigation()
        {
            try
            {
                var userRole = _session.GetUserRole();

                var vmData = new LeftNavigationViewModel { UserRole = userRole };

                var data = await _usersApi.GetAllRoleUsers(UserRoleType.Client);

                var pendingUsers = data.Where(x => x.AccountStatus == AccountStatusType.PendingAffiliate).Count();
                var activeSubscriptions = data.Where(x => x.SubscriptionStatus == StatusType.Active).Count();
                var pendingSubscriptions = data.Where(x => x.SubscriptionStatus == StatusType.Pending).Count();
                var freeTrialCount = data.Where(x => x.SubscriptionStatus == StatusType.Open).Count();

                vmData.PendingUsersCount = pendingUsers;
                vmData.ActivePremiums = activeSubscriptions;
                vmData.FreeTrialCount = freeTrialCount;
                vmData.PendingPremiums = pendingSubscriptions;

                return PartialView(AppPagesViewsUrl.LeftNavPageLink, vmData);
            }
            catch (Exception ex)
            {
                return PartialView(AppPagesViewsUrl.LeftNavPageLink, new LeftNavigationViewModel());
            }
        }
    }
}
