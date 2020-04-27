using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model
{
    public class AdminCommandResponse<T> : BaseResponse
    {
        public AdminCommandResponse(ResultCodesEnum resultCode,string errorString="") : base(resultCode,errorString) { }
        public AdminCommandResponse(ResultCodesEnum resultCode, IList<string> errList): base(resultCode, errList) { }

        public AdminCommandResponse(ResultCodesEnum resultCode,T content) : base(resultCode)
        {
            Content = content;
        }

        public T Content { get; set; }
    }
}
