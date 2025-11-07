using OvulaeDashboard.ViewModels.Affiliate;
using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.User;
using OvulaeShared.Helpers.CommonFunctions;
using OvulaeShared.ViewModel.Affiliates;

namespace OvulaeDashboard.Helpers.ViewHelper
{
    public class AffiliateViewsHelper
    {
        public static AffiliateMainDashboardViewModel GetAffiliateDashboardViewModel(AffiliateDashboardViewModel apiData, string currency)
        {
            try
            {
                var result = new AffiliateMainDashboardViewModel();

                result.TotalEarnings = ConversionsHelpers.GetAmountInCurrency(apiData.Wallet.TotalRevenue, currency, 2);
                result.TotalPaid = ConversionsHelpers.GetAmountInCurrency(apiData.Wallet.TotalPaidOut, currency, 2);

                result.TotalPendingAmount = ConversionsHelpers.GetAmountInCurrency((apiData.Wallet.TotalRevenue - apiData.Wallet.TotalPaidOut), currency, 2);

                var totalClicks = apiData.Clicks.AndroidClicks + apiData.Clicks.IOSClicks;
                result.ClicksCount = ConversionsHelpers.GetFormatedThousands(totalClicks);
                result.JoinedTotal = apiData.ReferalsDetails.Where(x => x.ReferalStatus == StatusType.Active).Count();

                var rate = totalClicks > 0 ? ((double)result.JoinedTotal / totalClicks * 1.0) * 100 : 0;

                result.ConversionRate = ConversionsHelpers.GetFormatedPercentage(rate, 2);

                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                result.CurrentMonthJoins = apiData.ReferalsDetails.Count(x => x.JoinDate >= startOfMonth);
                result.OverallJoins = result.JoinedTotal;
                result.NextLevelCount = OvulaeLevelsHelper.GetAffiliateNextLevel(result.JoinedTotal);

                var link = apiData.Clicks.AndroidReferalLink.Split("ovandr-")[1];
                result.GooglePlayStoreLink = $"https://portal.ovulae.com/JoinOvulaeApp/{link}";
                result.AppleAppStoreLink = link;

                var activeReferals = apiData.ReferalsDetails.Where(x => x.ReferalStatus == StatusType.Active).ToList();
                result.EarningsOverTime = ChartsDataHelper.GetAffiliateEarningsOverTime(activeReferals, false);
                result.EarningsVsNet = ChartsDataHelper.GetAffiliateEarningsOverTime(activeReferals, true);
                result.CommissionBreakdown = activeReferals;

                DefaultValueHelper.SetDefaults(result);

                return result;
            }
            catch
            {
                return new();
            }
        }

        public static DoctorMainDashboardViewModel GetDoctorDashboardViewModel(DoctorDashboardViewModel apiData, string currency)
        {
            try
            {
                var result = new DoctorMainDashboardViewModel();

                result.TotalPatients = apiData.ReferalsDetails.Count();
                result.TotalEarnings = ConversionsHelpers.GetAmountInCurrency(apiData.Wallet.TotalRevenue, currency, 2);
                result.TotalPaid = ConversionsHelpers.GetAmountInCurrency(apiData.Wallet.TotalPaidOut, currency, 2);

                result.TotalPendingAmount = ConversionsHelpers.GetAmountInCurrency((apiData.Wallet.TotalRevenue - apiData.Wallet.TotalPaidOut), currency, 2);

                var totalClicks = apiData.Clicks.AndroidClicks + apiData.Clicks.IOSClicks;
                result.ClicksCount = ConversionsHelpers.GetFormatedThousands(totalClicks);
                result.JoinedTotal = apiData.ReferalsDetails.Count();
                result.JoinedActive = apiData.ReferalsDetails.Where(x => x.ReferalStatus == StatusType.Active).Count();
                result.JoinedFreeTrial = apiData.ReferalsDetails.Where(x => x.ReferalStatus == StatusType.Open).Count();
                result.JoinedOthers = result.JoinedTotal - result.JoinedActive - result.JoinedFreeTrial;
                var rate = totalClicks > 0 ? ((double)result.JoinedTotal / totalClicks * 1.0) * 100 : 0;

                result.ConversionRate = ConversionsHelpers.GetFormatedPercentage(rate, 2);

                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                result.CurrentMonthJoins = apiData.ReferalsDetails.Count(x => x.JoinDate >= startOfMonth);
                result.OverallJoins = result.JoinedTotal;
                result.NextLevelCount = OvulaeLevelsHelper.GetAffiliateNextLevel(result.JoinedTotal);

                var link = apiData.Clicks.AndroidReferalLink.Split("ovandr-")[1];
                result.GooglePlayStoreLink = $"https://portal.ovulae.com/JoinOvulaeApp/{link}";
                result.AppleAppStoreLink = link;

                var activeReferals = apiData.ReferalsDetails.Where(x => x.ReferalStatus == StatusType.Active).ToList();
                result.EarningsOverTime = ChartsDataHelper.GetAffiliateEarningsOverTime(activeReferals, false);
                result.EarningsVsNet = ChartsDataHelper.GetAffiliateEarningsOverTime(activeReferals, true);
                result.CommissionBreakdown = activeReferals;

                DefaultValueHelper.SetDefaults(result);

                return result;
            }
            catch
            {
                return new();
            }
        }

        public static AffiliateDetailsViewModel GetAffiliateDetailsViewModel(AffiliateDashboardViewModel apiData, string currency)
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var activeReferals = apiData.ReferalsDetails.Where(x => x.ReferalStatus == StatusType.Active).ToList();
            var topCountryCode = activeReferals.GroupBy(r => r.CountryCode).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
            var link = apiData.Clicks.AndroidReferalLink.Split("ovandr-")[1];

            var viewModel = new AffiliateDetailsViewModel
            {
                UserName = $"{apiData.User.Firstname} {apiData.User.Lastname}",
                JoinDate = apiData.User.CreateDate,

                TotalRecruited = activeReferals.Count,
                RecruitedThisMonth = activeReferals.Count(x => x.JoinDate >= startOfMonth),
                TotalClicks = apiData.Clicks.AndroidClicks + apiData.Clicks.IOSClicks,
                
                TotalEarned = ConversionsHelpers.GetAmountInCurrency(apiData.Wallet.TotalRevenue, currency),
                TotalWithdrawn = ConversionsHelpers.GetAmountInCurrency(apiData.Wallet.TotalPaidOut, currency),
                TotalPending = ConversionsHelpers.GetAmountInCurrency(apiData.Wallet.TotalRevenue - apiData.Wallet.TotalPaidOut, currency),

                PlayStoreRefLink = $"https://portal.ovulae.com/JoinOvulaeApp/{link}",
                AppleStoreRefLink = link,
                
                PlayStoreJoins = apiData.ReferalsDetails.Count(x => x.DeviceType == MobileDeviceType.Android),
                AppleStoreJoins = apiData.ReferalsDetails.Count(x => x.DeviceType == MobileDeviceType.IOS),
                
                CurrentLevel = OvulaeLevelsHelper.GetAffiliateCurrentLevel(apiData.ReferalsDetails.Count),
                CurrentPoints = activeReferals.Count,
                RequiredToStayActive = activeReferals.Count < 100 ? 100 - apiData.ReferalsDetails.Count : 0,
                
                RecentReferrals = activeReferals.OrderByDescending(x => x.JoinDate).ToList(),
                MostUsedDevice = apiData.Clicks.IOSClicks > apiData.Clicks.AndroidClicks ? MobileDeviceType.IOS : MobileDeviceType.Android,
                TopCountryCode = topCountryCode,
                NextLevelCount = OvulaeLevelsHelper.GetAffiliateNextLevel(apiData.ReferalsDetails.Count)
            };

            return viewModel;
        }
    }
}
