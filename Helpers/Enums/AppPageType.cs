using System.ComponentModel.DataAnnotations;

namespace OvulaeDashboard.Helpers.Enums
{
    public enum AppPageType
    {
        [Display(Name = "", GroupName = "", Description = "", ShortName = "")]
        None,

        //Admin
        [Display(Name = "Dashboard", Description = "Admin Dashboard", GroupName = "AdminDashboard", ShortName = "Home")]
        AdminDashboard,

        [Display(Name = "UsersManagement", Description = "Users Management", GroupName = "Users", ShortName = "All")]
        UsersManagement,

        [Display(Name = "PremiumUsers", Description = "Users Management", GroupName = "Users", ShortName = "Premium")]
        PremiumUsers,

        [Display(Name = "AffiliateUsers", Description = "Users Management", GroupName = "Users", ShortName = "Affiliates")]
        AffiliateUsers,

        [Display(Name = "DoctorUsers", Description = "Users Management", GroupName = "Users", ShortName = "Doctors")]
        DoctorUsers,

        [Display(Name = "PendingApprovalUsers", Description = "Users Management", GroupName = "Users", ShortName = "Pending")]
        PendingApprovalUsers,

        [Display(Name = "AdminHome", GroupName = "Dashboard", Description = "Admin Home", ShortName = "AdminHome")]
        AdminHome,

        [Display(Name = "AdminAllTransactions", GroupName = "AdminAllTransactions", Description = "Admin All Transactions", ShortName = "AdminAllTransactions")]
        AdminAllTransactions,

        [Display(Name = "AffiliateManagerDashboard", GroupName = "Dashboard", Description = "Affiliate Manager Dashboard", ShortName = "Home")]
        AffiliateManagerDashboard,

        //Affiliate
        [Display(Name = "AffiliateDashboard", GroupName = "Dashboard", Description = "Affiliate Dashboard", ShortName = "Home")]
        AffiliateDashboard,

        [Display(Name = "AffiliateDetails", Description = "Affiliate Details", GroupName = "Affiliate", ShortName = "Details")]
        AffiliateDetails,

        [Display(Name = "AffiliateTransactions", Description = "Affiliate Transactions", GroupName = "Affiliate", ShortName = "All")]
        AffiliateTransactions,

        [Display(Name = "AllEarnings", Description = "All Earning", GroupName = "Earnings", ShortName = "All")]
        AllEarnings,

        [Display(Name = "ReferalEarnings", Description = "Referal Earning", GroupName = "Earnings", ShortName = "Referal")]
        ReferalEarnings,

        [Display(Name = "OtherEarnings", Description = "Other Earnings", GroupName = "Earnings", ShortName = "Others")]
        OtherEarnings,

        //============================== Doctor ============================
        [Display(Name = "DoctorDashboard", GroupName = "Dashboard", Description = "Doctor's Dashboard", ShortName = "Home")]
        DoctorDashboard,

        [Display(Name = "DoctorDetails", Description = "Doctor's Details", GroupName = "Doctor", ShortName = "Details")]
        DoctorDetails,

        [Display(Name = "DoctorTransactions", Description = "Doctor's Transactions", GroupName = "Doctor", ShortName = "All")]
        DoctorTransactions,

        [Display(Name = "DoctorPatients", Description = "Patients", GroupName = "Doctor", ShortName = "All")]
        DoctorPatients,

        [Display(Name = "DoctorsPatientDetails", Description = "Patients", GroupName = "Doctor", ShortName = "All")]
        DoctorsPatientDetails,

        // Transact
        [Display(Name = "AllTransactions", Description = "All Transactions", GroupName = "Transactions", ShortName = "All")]
        AllTransactions,

        [Display(Name = "DepositHistory", Description = "Deposit History", GroupName = "Deposit", ShortName = "History")]
        DepositHistory,

        [Display(Name = "DepositTopUp", Description = "Deposit Top-Up", GroupName = "Deposit", ShortName = "Top-Up")]
        DepositTopUp,

        [Display(Name = "WithdrawHistory", Description = "Withdraw History", GroupName = "Withdraw", ShortName = "History")]
        WithdrawHistory,

        [Display(Name = "WithdrawRequest", Description = "Withdraw Request", GroupName = "Withdraw", ShortName = "Request")]
        WithdrawRequest,

        [Display(Name = "AccountSettings", Description = "Account Settings", GroupName = "Account", ShortName = "Settings")]
        AccountSettings,

        [Display(Name = "Support", Description = "Support", GroupName = "Support", ShortName = "Help")]
        Support
    }
}
