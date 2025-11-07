using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace OvulaeDashboard.Controllers
{
    public class PaymentsController : Controller
    {
        public PaymentsController()
        {

        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult PaystackBridge() => View();

    }
}
