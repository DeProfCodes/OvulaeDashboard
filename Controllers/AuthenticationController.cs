using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OvulaeDashboard.Helpers;
using OvulaeDashboard.Services;
using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.User;
using OvulaeShared.Helpers.CommonFunctions;
using OvulaeShared.Services.APIs.Authentication;
using OvulaeShared.Services.APIs.Messaging;
using OvulaeShared.ViewModel.Account;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OvulaeDashboard.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationApi _authApi;
        private readonly IMessagingApi _messageApi;
        private readonly ISessionService _session;

        public AuthenticationController(IAuthenticationApi authAPI, IMessagingApi messageApi, ISessionService session)
        {
            _authApi = authAPI;
            _messageApi = messageApi;
            _session = session;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var data = await _authApi.Login(model.Email, model.Password);
                if (data != null)
                {
                    if (data.AccountStatus == AccountStatusType.Active)
                    {
                        _session.SetUserDetails($"{data.Firstname} {data.Lastname}", data.UserId, data.Email, data.UserRole);
                        _session.SetDashboardCurrency(data.CountryCode == "+27");

                        if (data.UserRole == UserRoleType.Affiliate || data.UserRole == UserRoleType.Doctor || data.UserRole == UserRoleType.Admin)
                        {
                            return Ok(new { success = true, loginURL = Url.Action("Index", "Dashboard") });
                        }
                    }
                    else
                    {
                        return Ok(new { success = true, loginURL = Url.Action("PendingApproval", "Authentication") });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, error = "Incorrect Logins" });
                }
            }
            catch
            {
                
            }
            return BadRequest(new { success = false, error = "Failed to login" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVm)
        {
            try
            {
                registerVm.AccountStatusType = AccountStatusType.PendingAffiliate;
                registerVm.UserRole = UserRoleType.Client;

                var registerRes = await _authApi.Register(registerVm);
                if (registerRes.Success)
                {
                    return Ok();
                }
                else
                {

                    return BadRequest(new { success = false, error = APIResponseHelper.GetMessageFromResponse(registerRes) });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { success = false, error = "Failed to create account." });
            }
        }

        [HttpGet]
        public IActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordResetCode(string email)
        {
            try
            {
                var passResetRes = await _messageApi.SendPasswordResetEmail(email);

                if (passResetRes.Success)
                {
                    HttpContext.Session.SetString("PasswordResetCode", passResetRes.Message);
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, error = APIResponseHelper.GetMessageFromResponse(passResetRes) });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { success = false, error = "Something went wrong. Contact support at support@ovulae.com" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PasswordResetCodeVerify(PasswordResetViewModel passwordResetVm)
        {
            try
            {
                var resetCode = HttpContext.Session.GetString("PasswordResetCode");
                if (resetCode == passwordResetVm.PasswordResetCode)
                {
                    var passwordResetRes = await _authApi.ChangePasswordEmail(passwordResetVm.Email, passwordResetVm.Password);
                    if (passwordResetRes.Success)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(new { success = false, error = APIResponseHelper.GetMessageFromResponse(passwordResetRes) });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, error = "Incorrect code" });
                }
            }
            catch
            {
                return BadRequest(new { success = false, error = "Something went wrong. Contact support at support@ovulae.com" });
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        public IActionResult PendingApproval()
        {
            return View();
        }
    }
}
