using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InsufficientBalanceException : BizStockException
    {
        public InsufficientBalanceException(decimal currentBalance, decimal required)
            : base($"Insufficient balance. Current: {currentBalance}, Required: {required}") { }
    }

}
