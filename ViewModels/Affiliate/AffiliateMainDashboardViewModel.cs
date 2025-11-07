using OvulaeDashboard.ViewModels.Shared;
using OvulaeShared.ViewModel.Affiliates;

namespace OvulaeDashboard.ViewModels.Affiliate
{
    public class AffiliateMainDashboardViewModel
    {
        public string TotalEarnings { get; set; }

        public string TotalPaid { get; set; }

        public string TotalPendingAmount { get; set; }

        public string TotalNextPaymentAmountForMonth { get; set; }

        public string ConversionRate { get; set; }

        public string ClicksCount { get; set; }

        public int JoinedTotal { get; set; }
        public int JoinedActive { get; set; }
        public int JoinedFreeTrial { get; set; }
        public int JoinedOthers { get; set; }

        public int CurrentMonthJoins { get; set; }

        public int OverallJoins { get; set; }

        public int NextLevelCount { get; set; }

        public string GooglePlayStoreLink { get; set; }

        public string AppleAppStoreLink { get; set; }

        public List<List<ChartViewModel>> EarningsOverTime { get; set; }

        public List<List<ChartViewModel>> EarningsVsNet { get; set; }

        public List<UserAffiliateReferalDetails> CommissionBreakdown { get; set; }

    }
}
