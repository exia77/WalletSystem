using BusinessObjects;
using DatabaseLayer.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer
{
    public class AccountsEF
    {
        private readonly DbLayerContext _context;
        public AccountsEF(DbLayerContext context) 
        { 
            _context = context;
        }

        private async Task<string> CheckAccount(AccountsModelObject account)
        {
            try
            {
                if(account.TransactionType == "Transfer")
                {
                    var accountNumberTo = await _context.Accounts!.AnyAsync(x=>x.AccountNumber == account.AccountNumberTo);
                    if (!accountNumberTo)
                    {
                        return "Invalid account number to.";
                    }
                }

                var accountNumberFrom = await _context.Accounts!.Where(x=>x.AccountNumber == account.AccountNumber).Select(x=>new { x.Balance,x.UpdatedDate }).FirstOrDefaultAsync();
                if (accountNumberFrom == null)
                {
                    return "Invalid account number.";
                }

                if(account.TransactionType != "Transfer")
                {
                    if(accountNumberFrom.Balance < account.Balance)
                    {
                        return $"Not enough balance. Current balance {accountNumberFrom.Balance}, amount deducted {account.Balance}.";
                    }
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CheckAccount. {ex.Message}");
            }
        }

        //public async Task<AccountsModelObject> UpdateBalance(AccountsModelObject account)
        //{
        //    //AccountsModelObject checkAccount = 
        //}
    }
}
