using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model
{
    public class CommandResponse
    {
        public enum ResultCodesEnum
        {
            Success=1,
            Error=2,
            Timeout=3,
            UserCancel=4
        }

        public ResultCodesEnum ResultCode { get; set; }
        public string ResultText { get; set; }
        public object Content { get; set; }

        public CommandResponse(ResultCodesEnum resultCode,string resultText="",object content=null)
        {
            ResultCode = resultCode;
            ResultText = resultText;
            Content = content;
        }
    }
}
