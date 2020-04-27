using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Data
{
    interface ICardTypesRepo
    {
        public void UpdateAcceptedCards(IList<CardType> cardsList);
        public IList<CardType> GetAcceptedCards();
    }
}
