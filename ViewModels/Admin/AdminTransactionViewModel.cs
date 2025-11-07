using OvulaeDashboard.Helpers.Enums;
using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.Transactions;


namespace OvulaeDashboard.ViewModels.Admin
{
    public class AdminTransactionViewModel
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public int TransactionId { get; set; }

        public double Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public DateTime Date { get; set; }

        public StatusType Status { get; set; }
    }
}
