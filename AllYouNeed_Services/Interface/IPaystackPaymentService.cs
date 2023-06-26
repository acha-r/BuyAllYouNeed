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
        TransactionVerifyResponse Verfiy(string reference);
        Task<TransactionInitializeResponse> InitializeTransaction(string productId, string buyerId, int numOfItems = 1);
    }
}
