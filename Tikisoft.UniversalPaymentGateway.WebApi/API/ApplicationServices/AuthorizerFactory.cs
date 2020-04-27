using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.API.ApplicationServices;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Service;
using TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.Service;
using TikiSoft.UniversalPaymentGateway.Domain.ServiceInterfaces;

namespace TikiSoft.UniversalPaymentGateway.API.ApplicationServices
{
    public interface IAuthorizerFactory : IDisposable
    {
        public IAuthorizer Create(string authName);

    }
}

namespace TikiSoft.UniversalPaymentGateway.ApplicationServices
{
    public class AuthorizerFactory : IAuthorizerFactory
    {
        private readonly Func<LaPosAuthorizer> _lapos;
        private readonly Func<MercadoPagoAuthorizer> _mercadoPago;

        public AuthorizerFactory(Func<LaPosAuthorizer> lapos, Func<MercadoPagoAuthorizer> mercadoPago)
        {
            _lapos = lapos;
            _mercadoPago = mercadoPago;
        }

        public IAuthorizer Create(string authName)
        {
            switch (authName)
            {
                case "lapos":
                    return _lapos();
                case "mercadopago":
                    return _mercadoPago();
                default:
                    //throw new InvalidOperationException();
                    return null;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AuthorizerFactory()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
