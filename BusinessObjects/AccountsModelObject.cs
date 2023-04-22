using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AccountsModelObject
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountNumberTo { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public decimal EndBalance { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
