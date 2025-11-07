using OvulaeDashboard.Helpers.Constants;
using OvulaeDashboard.ViewModels.Admin;
using OvulaeDashboard.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OvulaeShared.Services.APIs.Users;
using OvulaeShared.ViewModel.User;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OvulaeShared.Services.Email;
using OvulaeShared.Enums.User;
using OvulaeDashboard.Helpers.ViewHelper;

namespace OvulaeDashboard.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUsersApi _usersApi;
        private readonly IOvulaeEmailService _emailSev;

        public AdminController(IUsersApi usersApi, IOvulaeEmailService emailSev)
        {
            _usersApi = usersApi;
            _emailSev = emailSev;
        }

        public async Task<IActionResult> Dashboard()
        {
            var adminDashboard = new AdminDashViewModel();
            try
            {
                var apiData = await _usersApi.GetAdminDashboardData();

                adminDashboard = AdminViewHelper.GetAdminDashboardData(apiData);
            }
            catch (Exception ex)
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return PartialView(AppPagesViewsUrl.AdminDashboardLink, adminDashboard);
        }

        public async Task<IActionResult> AdminHome(string userId)
        {
            try
            {
                return PartialView(AppPagesViewsUrl.AdminHomePageLink, new DashboardViewModel());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Logout", "Authentication");
            }
        }

        public async Task<IActionResult> AdminAllTransactions(string userId)
        {
            var adminVm = new AdminGeneralViewModel();
            try
            {

            }
            catch (Exception ex)
            {

            }
            return PartialView(AppPagesViewsUrl.AdminAllTransactionsPageLink, adminVm);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveUser(ApproveRejectUserViewModel data)
        {
            try
            {
                var approveUser = await _usersApi.ApproveRejectUser(data);

                if (approveUser.Success)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, error = approveUser.Message });
                }
            }
            catch (Exception ex)
            {

            }
            return BadRequest(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> RejectUser(ApproveRejectUserViewModel data)
        {
            try
            {
                var approveUser = await _usersApi.ApproveRejectUser(data);
                
                if (approveUser.Success)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, error = approveUser.Message });
                }
            }
            catch (Exception ex)
            {

            }
            return BadRequest(new { success = false });
        }
        
    }
}
