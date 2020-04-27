using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model
{
    public class CardType
    {
        
        public string Name { get; set; }
        public string ProcessorCode { get; set; }
        public long? MerchantNumber { get; set; }
        [Key]
        public string CardCode { get; set; }
    }
}
