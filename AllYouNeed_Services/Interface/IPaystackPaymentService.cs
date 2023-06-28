using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Services.Interface
{
    public interface IPaystackPaymentService
    {
        Task<TransactionVerifyResponse> Verfiy(string reference, string cartId);
        Task<TransactionInitializeResponse> InitializeTransaction(string cartId);
    }
}
