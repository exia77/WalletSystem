using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class TransactionHistoryModelObjects
    {
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string AccountNumberFrom { get; set; } = string.Empty;
        public string AccountNumberTo { get; set;} = string.Empty;
        public DateTime TransactionDate { get; set; }
        public decimal EndBalance { get; set; }
        [JsonIgnore]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
