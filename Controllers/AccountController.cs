using OvulaeDashboard.Helpers.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OvulaeShared.ViewModel.Account;
using OvulaeShared.Models.WebApi;
using OvulaeShared.ViewModel.User;

namespace OvulaeDashboard.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {

        }

        public IActionResult Login()
        {
            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        public async Task<IActionResult> AccountSettings()
        {
            var settingsVm = new SettingsViewModel { BankDetails = new() }; 
            try
            {
                
            }
            catch (Exception ex)
            {

            }
            return PartialView(AppPagesViewsUrl.AccountSettingsPageLink, settingsVm);
        }

        [Authorize]
        public async Task<IActionResult> RequestPasswordReset()
        {
            try
            {
                var emailSendStatus = new GenericResult { Success = true };

                if (emailSendStatus.Success)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, error = "Failed to send email. Please try again!" });
                }
            }
            catch
            {
                return BadRequest(new { success = false, error = "Failed to send email. Please try again!" });
            }
        }

        [Authorize]
        public async Task<IActionResult> RequestPasswordResetSave()
        {
            try
            {
                var emailSendStatus = new GenericResult { Success = true };

                if (emailSendStatus.Success)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, error = "Failed to send email. Please try again!" });
                }
            }
            catch
            {
                return BadRequest(new { success = false, error = "Failed to send email. Please try again!" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBankDetails(UserBankDetailsViewModel bankDetilas)
        {
            try
            {
                if (bankDetilas != null && !string.IsNullOrEmpty(bankDetilas.BankName) && !string.IsNullOrEmpty(bankDetilas.AccountNumber))
                {
                    var success = true;

                    if (success)
                    {
                        return Ok(new { success = true });
                    }
                    else
                    {
                        return BadRequest(new { success = false, error = "Failed to change bank details." });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, error = "You did not enter all bank details." });
                }
            }
            catch
            {
                return BadRequest(new { success = false, error = "Something went wrong. Contact support at catalyst.fx.dynamics@gmail.com" });
            }
        }

        [Authorize]
        public IActionResult UserProfile()
        {
            return PartialView();
        }

        

        [Authorize]
        public IActionResult PrivacyPolicy()
        {
            return PartialView();
        }

        [Authorize]
        public IActionResult Subscription()
        {
            return PartialView();
        }
    }
}
