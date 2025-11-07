using OvulaeDashboard.ViewModels.Dashboard;
using OvulaeShared.Models.User;
using OvulaeShared.ViewModel.Transactions;

namespace OvulaeDashboard.ViewModels.Admin
{
    public class AdminGeneralViewModel
    {
        public string UserId { get; set; }

        public string ClientFullname { get; set; }

        public List<TransactionHistoryViewModel> AllTransactions { get; set; }

        public List<UserModel> Users { get; set; }

        public DashboardViewModel AdminUserDashboard { get; set; }
    }
}
