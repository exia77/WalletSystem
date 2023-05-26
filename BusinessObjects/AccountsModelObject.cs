using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AccountsModelObject
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }

        public UsersModelObject? Users { get; set; }
        public decimal Balance { get; set; }
        [NotMapped]
        public string? TransactionType { get; set; } 
        public string? AccountNumber { get; set; }
        [NotMapped]
        public string? AccountNumberTo { get; set; }
        [NotMapped]
        public DateTime? TransactionDate { get; set; }
        [NotMapped]
        public decimal EndBalance { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedDate { get; set; }

        [NotMapped]
        public string? ErrorMessage { get; set; }
    }
}
