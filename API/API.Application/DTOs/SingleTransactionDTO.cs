using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class SingleTransactionDTO
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public float Amount { get; set; }
        public UserDTO? User { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public float BTCPrice { get; set; }
    }
}