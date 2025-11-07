using OvulaeShared.Enums.User;
using OvulaeShared.ViewModel.Affiliates;

namespace OvulaeDashboard.ViewModels.Affiliate
{
    public class AffiliateDetailsViewModel
    {
        // Basic Info
        public string UserName { get; set; }
        public DateTime JoinDate { get; set; }

        // Referrals
        public int TotalRecruited { get; set; }
        public int RecruitedThisMonth { get; set; }
        public int TotalClicks { get; set; }

        // Earnings
        public string TotalEarned { get; set; }
        public string TotalWithdrawn { get; set; }
        public string TotalPending { get; set; }

        // Referral Links
        public string PlayStoreRefLink { get; set; }
        public string AppleStoreRefLink { get; set; }

        // Source Breakdown
        public int PlayStoreJoins { get; set; }
        public int AppleStoreJoins { get; set; }

        // Leveling
        public int CurrentLevel { get; set; }
        public int CurrentPoints { get; set; } // e.g. 250 out of 1000

        // Activity Status
        public int RequiredToStayActive { get; set; } // e.g. 85 more needed
        public double MonthlyProgressPercent
        {
            get
            {
                if (RecruitedThisMonth + RequiredToStayActive == 0) return 0;
                return Math.Min(100, (double)RecruitedThisMonth / (RecruitedThisMonth + RequiredToStayActive) * 100);
            }
        }

        public bool IsActiveNextMonth => RequiredToStayActive <= 0;

        public DateTime? LastReferralDate { get; set; }

        public string RankBadgeImage => $"/images/affiliates/level_{CurrentLevel}.png";

        public List<UserAffiliateReferalDetails> RecentReferrals { get; set; }

        public int UniqueClicks7d { get; set; }

        public bool IsiOS { get; set; }
        public MobileDeviceType MostUsedDevice { get; set; }
        public string TopCountryCode { get; set; }
        public int NextLevelCount { get; set; }
    }
}
