using OvulaeDashboard.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using OvulaeShared.ViewModel.Support;

namespace OvulaeDashboard.Controllers
{
    public class SupportController : Controller
    {
        public SupportController()
        {

        }

        public IActionResult Support()
        {
            return PartialView(AppPagesViewsUrl.SupportLink);
        }

        public async Task<IActionResult> SendSupportEmail(SupportEmailViewModel supportEmailType)
        {
            try
            {
                return Ok(new { success = true });
            }
            catch
            {
                return BadRequest(new { success = false, error = "Something went wrong. Email us directly at support@ovulae.com" });
            }
        }
    }
}
