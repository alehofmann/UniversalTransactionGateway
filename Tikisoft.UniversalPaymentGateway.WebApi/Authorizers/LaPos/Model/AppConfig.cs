using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model
{
    public class AppConfig
    {
        public CardType[] CardTypes { get; set; }
        public string NombreComercio { get; set; }
        public string CuitComercio { get; set; }
        public string Comport { get; set; }

    }
}
