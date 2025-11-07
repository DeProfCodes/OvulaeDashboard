using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using OvulaeDashboard.Helpers.Enums;

using OvulaeDashboard.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;
using OvulaeShared.ViewModel.Transactions;
using OvulaeShared.Enums.Transactions;

namespace OvulaeDashboard.Controllers
{
    //[Authorize]
    public class TransactionsController : Controller
    {
        public TransactionsController()
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> AllTransactions()
        {
            List<TransactionHistoryViewModel> allTransactions = new();
            try
            {
                
            }
            catch (Exception ex)
            {

            }
            allTransactions = allTransactions == null ? new() : allTransactions;

            return PartialView(AppPagesViewsUrl.AllTransactionsPageLink, allTransactions);
        }

        [HttpPost]
        public async Task<IActionResult> DepositFund(TransactViewModel depositFundsVM)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/yoco/webhook")]
        public async Task<IActionResult> YocoWebhookListener()
        {
            using var reader = new StreamReader(Request.Body);
            var rawBody = await reader.ReadToEndAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> PaySuccess(string email, TransactionType type, int tp, int pId)
        {
            try
            {
                return Ok();
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PayFail(string email, TransactionType type, int pId, int fp = 16)
        {
            try
            {
                return Ok();
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PayCancel(string email, TransactionType type, int pId, int fp = 16)
        {
            try
            {
                return Ok();
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }

        #region Withdrawals

        [HttpGet]
        public async Task<IActionResult> WithdrawHistory()
        {
            List<TransactionHistoryViewModel> withdrawHistory = null;
            try
            {
                
            }
            catch (Exception ex)
            {

            }
            withdrawHistory = withdrawHistory == null ? new() : withdrawHistory.OrderByDescending(t => t.TransactionDate).ToList();
            return PartialView(AppPagesViewsUrl.WithdrawHistoryPageLink, withdrawHistory);
        }

        [HttpGet]
        public async Task<IActionResult> WithdrawRequest()
        {
            var investWithdrawPlans = new WithdrawalFundsRequestViewModel() 
            {
                BankDetailsModel = new(),
                BankDetails = new(),
                PlansAvailableFunds = new(),
            };

            try
            {
                
            }
            catch
            {

            }

            return PartialView(AppPagesViewsUrl.WithdrawRequestPageLink, investWithdrawPlans);
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawRequest(WithdrawalFundsRequestViewModel withdrawalRequest)
        {
            try
            {
                return Ok(new { success = true, page = AppPageType.WithdrawHistory.GetDisplayName() });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, page = AppPageType.WithdrawHistory.GetDisplayName() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelWithdrawRequest(int withdrawalId)
        {
            try
            {
                return Ok(new { success = true, page = AppPageType.WithdrawHistory.GetDisplayName() });
            }
            catch
            {

            }
            return BadRequest(new { success = false, page = AppPageType.WithdrawHistory });
        }

        #endregion
    }
}
