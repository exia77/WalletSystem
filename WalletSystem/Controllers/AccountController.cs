using BusinessLayer;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WalletSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccounts _accounts;

        public AccountController(IAccounts accounts)
        {
            _accounts = accounts;
        }

        [Authorize]
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] AccountsModelObject accountObjects)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountObjects.AccountNumber))
                {
                    return BadRequest(new { Message = "Account number is required." });
                }

                if (accountObjects.Amount <= 0.00M)
                {
                    return BadRequest(new { Message = "Amount is required." });
                }

                accountObjects.TransactionType = "Deposit";

                AccountsModelObject responseAccountObject = await _accounts.UpdateBalance(accountObjects);
                if(!string.IsNullOrEmpty(responseAccountObject.ErrorMessage))
                {
                    return BadRequest( new { Message = responseAccountObject.ErrorMessage } );
                }

                return Ok(ResultMessage(responseAccountObject));

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] AccountsModelObject accountObjects)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountObjects.AccountNumber))
                {
                    return BadRequest(new { Message = "Account number is required." });
                }

                if (accountObjects.Amount <= 0.00M)
                {
                    return BadRequest(new { Message = "Amount is required." });
                }

                accountObjects.TransactionType = "Withdraw";

                AccountsModelObject responseAccountObject = await _accounts.UpdateBalance(accountObjects);
                if (!string.IsNullOrEmpty(responseAccountObject.ErrorMessage))
                {
                    return BadRequest(new { Message = responseAccountObject.ErrorMessage });
                }

                return Ok(ResultMessage(responseAccountObject));

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] AccountsModelObject accountObjects)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountObjects.AccountNumber))
                {
                    return BadRequest(new { Message = "Account number is required." });
                }

                if (string.IsNullOrWhiteSpace(accountObjects.AccountNumberTo))
                {
                    return BadRequest(new { Message = "Account number to is required." });
                }

                if (accountObjects.Amount <= 0.00M)
                {
                    return BadRequest(new { Message = "Amount is required." });
                }

                accountObjects.TransactionType = "Transfer";

                AccountsModelObject responseAccountObject = await _accounts.UpdateBalance(accountObjects);
                if (!string.IsNullOrEmpty(responseAccountObject.ErrorMessage))
                {
                    return BadRequest(new { Message = responseAccountObject.ErrorMessage });
                }

                return Ok(ResultMessage(responseAccountObject));

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetTransactionHistory/{accountNumber}")]
        public async Task<IActionResult> GetTransactionHistory([FromRoute] AccountsModelObject accountObjects)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountObjects.AccountNumber))
                {
                    return BadRequest(new { Message = "Account number is required." });
                }

                List<TransactionHistoryModelObjects> responseListAccountObject = await _accounts.GetTransactionHistory(accountObjects);
                if (!string.IsNullOrEmpty(responseListAccountObject[0].ErrorMessage))
                {
                    return BadRequest(new { Message = responseListAccountObject[0].ErrorMessage });
                }

                return Ok(responseListAccountObject);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        private static object ResultMessage(AccountsModelObject accountsObjects)
        {
            return new
            {
                Message = $"{accountsObjects.TransactionType} money account number {accountsObjects.AccountNumber} successful.",
                Balance = accountsObjects.Amount,
                accountsObjects.TransactionDate,
                accountsObjects.EndBalance
            };
        }
    }
}
