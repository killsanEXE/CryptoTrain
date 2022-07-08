using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class TransactionsDTO
    {
        public List<SingleTransactionDTO>? Transactions { get; set; }
    }
}