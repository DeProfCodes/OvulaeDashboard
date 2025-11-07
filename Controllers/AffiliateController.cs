using Microsoft.AspNetCore.Mvc;
using OvulaeDashboard.Helpers;
using OvulaeDashboard.Helpers.Constants;
using OvulaeDashboard.Helpers.ViewHelper;
using OvulaeDashboard.Services;
using OvulaeDashboard.ViewModels.Affiliate;
using OvulaeShared.Enums.Affiliate;
using OvulaeShared.Services.APIs.Affiliates;
using OvulaeShared.Services.APIs.Transactions;
using OvulaeShared.ViewModel.Affiliates;

namespace OvulaeDashboard.Controllers
{
    //[Authorize]
    public class AffiliateController : Controller
    {
        private readonly IAffiliatesApi _affApi;
        private readonly ISessionService _session;
        private readonly ITransactionsApi _transApi;

        public AffiliateController(IAffiliatesApi affApi, ISessionService session, ITransactionsApi transApi)
        {
            _affApi = affApi;
            _session = session;
            _transApi = transApi;
        }

        [HttpGet]
        public async Task<IActionResult> AffiliateDashboard()
        {
            try
            {
                var affiliateDashboardData = await _affApi.GetAffiliateDashboardDetails(_session.GetSecureApiRequestDto());
                if (_session.GetSecureApiRequestDto().Email == "test5000@gmail.com")
                {
                    affiliateDashboardData = AffiliateMockDataGenerator.GenerateSuccessfulAffiliateData("affiliate_123");
                }

                var affiliateDataVM = AffiliateViewsHelper.GetAffiliateDashboardViewModel(affiliateDashboardData, _session.GetDashboardCurrency());
                
                return PartialView(AppPagesViewsUrl.AffiliateDashboardPageLink, affiliateDataVM);
            }
            catch (Exception ex)
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return View(new AffiliateMainDashboardViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> AffiliateDetails()
        {
            try
            {
                var affiliateDashboardData = await _affApi.GetAffiliateDashboardDetails(_session.GetSecureApiRequestDto());
                if (_session.GetSecureApiRequestDto().Email == "test5000@gmail.com")
                {
                    affiliateDashboardData = AffiliateMockDataGenerator.GenerateSuccessfulAffiliateData("affiliate_123");
                }
                var viewModel = AffiliateViewsHelper.GetAffiliateDetailsViewModel(affiliateDashboardData, _session.GetDashboardCurrency());

                return PartialView(AppPagesViewsUrl.AffiliateDetailsPageLink, viewModel);
            }
            catch
            {
                return PartialView(AppPagesViewsUrl.AffiliateDetailsPageLink, new AffiliateDetailsViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> AffiliateTransactions()
        {
            try
            {
                var transactions = await _transApi.GetUserTransactions(_session.GetSecureApiRequestDto());
                transactions = transactions != null ? transactions : new();

                return PartialView(AppPagesViewsUrl.AffiliateTransactionsPageLink, transactions);
            }
            catch
            {
                return PartialView(AppPagesViewsUrl.AffiliateTransactionsPageLink, new AffiliateDetailsViewModel());
            }
        }


        [Route("JoinOvulaeApp/{refCode}")]
        public async Task<IActionResult> MobileReferalLinkClick(string refCode)
        {
            var userAgent = Request.Headers["User-Agent"].ToString().ToLower();
            var isIOSDevice = userAgent.Contains("iphone") || userAgent.Contains("ipad") || userAgent.Contains("ipod");
            var isHuaweiDevice = userAgent.Contains("huawei") || userAgent.Contains("honor");

            try
            {
                // Fire-and-forget background task
                Task.Run(async () =>
                {
                    var dto = new AffiliateUpdateViewModel
                    {
                        AffiliateUpdateType = isIOSDevice ? AffiliateUpdateType.IOSLinkClick : AffiliateUpdateType.AndroidLinkClick,
                        ReferalMobileLink = !isIOSDevice ? $"ovandr-{refCode}" : $"ovios-{refCode}"
                    };

                    try
                    {
                        await _affApi.UpdateAffiliateModel(dto);
                    }
                    catch (Exception ex)
                    {
                        // Optionally log the error
                    }
                });

                // Detect device from User-Agent
                
                string redirectUrl = $"https://play.google.com/store/apps/details?id=com.ovulae.org.ovulaeapp&hl=en&referrer=ovandr-{refCode}"; ;

                if (userAgent.Contains("android"))
                {
                    redirectUrl = $"https://play.google.com/store/apps/details?id=com.ovulae.org.ovulaeapp&hl=en&referrer=ovandr-{refCode}";
                    return Redirect(redirectUrl);
                }
                else if (isIOSDevice)
                {
                    var branchUrl = BuildBranchLink($"ovios-{refCode}");
                    return Redirect(branchUrl);
                }
                else if (isHuaweiDevice)
                {
                    redirectUrl = "https://appgallery.huawei.com/app/C114959577";
                    return Redirect(redirectUrl);
                }
                return Redirect("https://ovulae.com");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        private static string BuildBranchLink(string refCode)
        {
            // your Branch subdomain:
            const string domain = "https://7xncy.app.link/";

            // Build query with control params. $ -> %24, ~ -> %7E
            // These arrive in InitSessionComplete(data) just like API-created links.
            var qp = new Dictionary<string, string>
            {
                // custom payload you read from Branch data:
                ["refCode"] = refCode,

                // control params (must be URL-encoded keys):
                ["%24ios_deeplink_path"] = $"join?refCode={Uri.EscapeDataString(refCode)}",
                ["%24fallback_url"] = "https://apps.apple.com/us/app/ovulae/id6751113242",

                // (optional) analytics tags:
                ["%7Efeature"] = "referral",
                ["%7Echannel"] = "mobile",
                ["%7Ecampaign"] = "affiliate"
            };

            var qs = string.Join("&", qp.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{domain}?{qs}";
        }


        public IActionResult DownloadIOS()
        {
            return View();
        }

    }
}
