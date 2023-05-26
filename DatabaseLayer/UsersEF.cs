using BusinessObjects;
using DatabaseLayer.DatabaseContext;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DatabaseLayer
{
    public class UsersEF
    {
        private readonly DbLayerContext _context;
        public UsersEF(DbLayerContext context) 
        {
            _context = context;
        }

        public async Task<bool> Register(UsersModelObject users)
        {
            using var trans = _context.Database.BeginTransaction();
            try
            {
                users.RegisterDate = DateTime.Now;
                users.Id = Guid.NewGuid();
                users.Password = HashPassword(users.Password);
                await _context.Users!.AddAsync(users);
                await _context.SaveChangesAsync();
                
                await CreateAccount(users.Id);
                trans.Commit();

                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception($"Error in Registration. {ex.Message}");
            }
        }

        public async Task<bool> CheckUserExist(UsersModelObject users)
        {
            try
            {
                return await _context.Users!.AnyAsync(x => x.Username == users.Username);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in CheckUserExist. {ex.Message}");
            }
        
        }

        public async Task<bool> Verify(UsersModelObject users)
        {
            try
            {
                return await _context.Users!.AnyAsync(x =>               
                    x.Username == users.Username &&
                    x.Password == HashPassword(users.Password)
                );
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in Verify. {ex.Message}");
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }

        private async Task CreateAccount(Guid Id)
        {
            try
            {
                var _AccountNumber = CreateAccountNumber();

                while (await _context.Accounts!.AnyAsync(x => x.AccountNumber == _AccountNumber))
                {
                    _AccountNumber = CreateAccountNumber();
                }

                AccountsModelObject accounts = new()
                {
                    UserId = Id,
                    AccountNumber = _AccountNumber,
                    Balance = 0,
                    UpdatedDate=DateTime.Now
                };

                await _context.Accounts!.AddAsync(accounts);
                await _context.SaveChangesAsync();
            }
        catch(Exception ex)
            {
                throw new Exception($"Error in CreateAccount {ex.Message}");
            }
        }

        private static string CreateAccountNumber()
        {

            var random = new Random();
            string accountNumber = string.Empty;

            while (accountNumber.Length < 12)
            {
                accountNumber += random.Next(0, 9).ToString();
            }

            return accountNumber;

        }
    }
}
