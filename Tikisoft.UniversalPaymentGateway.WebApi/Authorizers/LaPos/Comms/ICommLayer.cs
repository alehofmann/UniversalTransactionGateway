using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms
{
    public interface ICommLayer
    {
        public Task<IList<Model.CardType>> GetCardTypesAsync();
        public Task<CommandResponse> ProcessTransaction(BaseRequest request);
        public void Initialize(string comport);
    }
}
