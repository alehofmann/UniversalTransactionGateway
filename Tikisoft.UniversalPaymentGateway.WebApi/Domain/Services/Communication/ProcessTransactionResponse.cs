using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Models;

namespace TikiSoft.UniversalPaymentGateway.Domain.Services.Communication
{
    public class ProcessTransactionResponse : BaseResponse
    {
        public TransResponse TransResponse { get; private set; }
        //public string ErrorSource { get; set; } = null;

        //Private
        private ProcessTransactionResponse(bool success, string message, TransResponse response) : base(success, message)
        {
            TransResponse = response;
        }

        //SuccessResponse
        public ProcessTransactionResponse(TransResponse response) : this(true, string.Empty, response)
        { }

        //Fail Response
        public ProcessTransactionResponse(string message) : this(false, message, null)
        { }
    }
}
