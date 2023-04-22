using BusinessObjects;
using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IAccounts
    {
        Task<AccountsModelObject> UpdateBalance(AccountsModelObject accounts);
        Task<List<TransactionHistoryModelObjects>> GetTransactionHistory(AccountsModelObject accounts);
    }

    public class Accounts : IAccounts
    {
        private readonly AccountsDb db;
        public Accounts(string connectionString) 
        {
            db = new AccountsDb(connectionString);
        }

        public async Task<List<TransactionHistoryModelObjects>> GetTransactionHistory(AccountsModelObject accounts)
        {
            return await db.GetTransactionHistory(accounts);
        }

        public async Task<AccountsModelObject> UpdateBalance(AccountsModelObject accounts)
        {
            return await db.UpdateBalance(accounts);
        }
    }
}
