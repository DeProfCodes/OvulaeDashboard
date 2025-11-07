using OvulaeDashboard.Helpers.Enums;


using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.Transactions;
using OvulaeShared.Enums.User;
using OvulaeShared.ViewModel.Transactions;


namespace OvulaeDashboard.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public string Fullname { get; set; }

        public string UserId { get; set; }

        public AppPageType StartPage { get; set; }

        public TransactionType TransactionType { get; set; }

        public StatusType Status { get; set; }

        public int PlanId { get; set; }

        public double TotalInvested { get; set; }

        public double TotalReturns { get; set; }

        public int ActivePlans { get; set; }

        public double TotalWithdrawals { get; set; }

        public List<TransactionHistoryViewModel> Transactions { get; set; }

        public List<EarningsViewModel> Returns { get; set; }

        public UserRoleType UserRole { get; set; }
    }
}
