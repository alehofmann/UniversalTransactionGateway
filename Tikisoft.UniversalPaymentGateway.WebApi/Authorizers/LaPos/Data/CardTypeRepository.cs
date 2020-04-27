using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model;
using TikiSoft.UniversalPaymentGateway.Persistence.Repositories;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Data
{
    public class CardTypeRepository : EfCoreRepository<CardType,LaPosDbContext,string>
    {
        public CardTypeRepository(LaPosDbContext context) : base(context) { }
       

    }
}
