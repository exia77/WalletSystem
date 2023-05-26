using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DatabaseLayer
{
    public class AccountsDb
    {
        private readonly string _connectionString;
        public AccountsDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        private async Task<AccountsModelObject> CheckAccount(AccountsModelObject accounts)
        {

            try
            {
                using var connection = new SqlConnection(_connectionString);

                if (accounts.TransactionType == "Transfer")
                {
                    await connection.OpenAsync();

                    var command = new SqlCommand("Transaction_CheckAccount", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AccountNo", accounts.AccountNumberTo);

                    using var reader = await command.ExecuteReaderAsync();

                    AccountsModelObject validateAccount = new();

                    while (await reader.ReadAsync())
                    {
                        validateAccount.UpdatedDate = reader.GetDateTime(0);
                        validateAccount.Balance = reader.GetDecimal(1);
                    }
                    if (!reader.HasRows)
                    {
                        accounts.ErrorMessage = "Invalid account number to.";
                        return accounts;
                    }

                    await connection.CloseAsync();
                }

                await connection.OpenAsync();

                var command2 = new SqlCommand("Transaction_CheckAccount", connection);
                command2.CommandType = CommandType.StoredProcedure;
                command2.Parameters.AddWithValue("@AccountNo", accounts.AccountNumber);

                using var reader2 = await command2.ExecuteReaderAsync();

                AccountsModelObject validateAccount2 = new();

                while (await reader2.ReadAsync())
                {
                    validateAccount2.UpdatedDate = reader2.GetDateTime(0);
                    validateAccount2.Balance = reader2.GetDecimal(1);
                }
                if (!reader2.HasRows)
                {
                    accounts.ErrorMessage = "Invalid account number.";
                    return accounts;
                }

                await connection.CloseAsync();
                return validateAccount2;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CheckAccount {ex.Message}");
            }
        }

        public async Task<AccountsModelObject> UpdateBalance(AccountsModelObject accountsObject)
        {
            try
            {
                AccountsModelObject checkAccount = new();

                checkAccount = await CheckAccount(accountsObject);
                if (!string.IsNullOrEmpty(checkAccount.ErrorMessage))
                {
                    return checkAccount;
                }

                if (accountsObject.TransactionType != ((TransactionType)0).ToString())
                {
                    if (checkAccount.Balance < accountsObject.Balance)
                    {
                        accountsObject.ErrorMessage = $"Not enough balance. Current Balance: {checkAccount.Balance}, Amount Deducted: {accountsObject.Balance}";
                        return accountsObject;
                    }
                }

                using var connection = new SqlConnection(_connectionString);

                await connection.OpenAsync();

                var command = new SqlCommand("Transaction_SetUpdateTransaction", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountNoFrom", accountsObject.AccountNumber);
                command.Parameters.AddWithValue("@AccountNoTo", accountsObject.AccountNumberTo);
                command.Parameters.AddWithValue("@TransactionType", (TransactionType)Enum.Parse(typeof(TransactionType), accountsObject.TransactionType));
                command.Parameters.AddWithValue("@Amount", accountsObject.Balance);
                command.Parameters.AddWithValue("@UpdatedDate", checkAccount.UpdatedDate);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    accountsObject.Balance = reader.GetDecimal(0);
                    accountsObject.EndBalance = reader.GetDecimal(1);
                    accountsObject.TransactionDate = reader.GetDateTime(2);
                    accountsObject.TransactionType = ((TransactionType)reader.GetInt32(3)).ToString();
                }

                await connection.CloseAsync();

                return accountsObject;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in UpdateBalance {ex.Message}");
            }

        }

        private enum TransactionType
        {
            Deposit = 0,
            Withdraw = 1,
            Transfer = 2
        }

        public async Task<List<TransactionHistoryModelObjects>> GetTransactionHistory(AccountsModelObject accounts)
        {
            List<TransactionHistoryModelObjects> transHistoryList = new();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var command = new SqlCommand("Users_GetTransactionHistory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountNo", accounts.AccountNumber);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    TransactionHistoryModelObjects transHistoryObjects = new();
                    transHistoryObjects.TransactionType = reader.GetString(0);
                    transHistoryObjects.Amount = reader.GetDecimal(1);
                    transHistoryObjects.AccountNumberFrom = reader.GetString(2);
                    transHistoryObjects.AccountNumberTo = reader.GetString(3);
                    transHistoryObjects.TransactionDate = reader.GetDateTime(4);
                    transHistoryObjects.EndBalance = reader.GetDecimal(5);
                    transHistoryList.Add(transHistoryObjects);
                }

                if (!reader.HasRows)
                {
                    transHistoryList.Add(new TransactionHistoryModelObjects(){ ErrorMessage = "Invalid account number." });
                    return transHistoryList;
                }

                await connection.CloseAsync();

                return transHistoryList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in TransactionHistory {ex.Message}");
            }

        }
    }
}
