using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model
{
    public class VoidRequest
    {
        public enum VoidTypeEnum
        {
            VoidSell = 1,
            VoidRefund = 2
        }

        public VoidTypeEnum VoidType { get; set; }
        public int CouponCode { get; set; }
        public string CardCode { get; set; }

    }
}
