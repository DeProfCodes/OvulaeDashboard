using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using OvulaeDashboard.Helpers.Constants;
using OvulaeShared.ViewModel.Transactions;

namespace OvulaeDashboard.Controllers
{
    //[Authorize]
    public class EarningsController : Controller
    {
        public EarningsController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> AllEarnings()
        {
            List<EarningsViewModel> allEarnings = null;
            try
            {

            }
            catch
            {
                
            }
            allEarnings = allEarnings == null ? new() : allEarnings.OrderByDescending(r => r.EarningsDate).ToList();

            return PartialView(AppPagesViewsUrl.AllEarningsPageLink, allEarnings);
        }

        [HttpGet]
        public async Task<IActionResult> ReferalEarnings()
        {
            List<EarningsViewModel> referalEarnings = null;
            try
            {

            }
            catch
            {

            }

            referalEarnings = referalEarnings == null ? new() : referalEarnings.OrderByDescending(r => r.EarningsDate).ToList();

            return PartialView(AppPagesViewsUrl.ReferalEarningsPageLink, referalEarnings);
        }

        [HttpGet]
        public async Task<IActionResult> OtherEarnings()
        {
            return PartialView(AppPagesViewsUrl.OtherEarningsPageLink, new List<EarningsViewModel>());
        }

    }
}
