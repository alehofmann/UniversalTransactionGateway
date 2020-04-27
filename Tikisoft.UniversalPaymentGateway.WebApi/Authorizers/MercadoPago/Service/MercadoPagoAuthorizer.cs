using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
using TikiSoft.UniversalPaymentGateway.Domain.ServiceInterfaces;
using Microsoft.Extensions.Configuration;
using TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto;
using TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient;
using System.Diagnostics;
using System.Net.Http;
using TikiSoft.UniversalPaymentGateway.ApplicationServices;
using TikiSoft.UniversalPaymentGateway.Domain.CustomAttributes;
using TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.Service
{
    [Authorizer("mercadopago")]
    public class MercadoPagoAuthorizer : IAuthorizer 
        
    {
        //private NotifierDbContext _context;
        private string _token;
        private string _userId;
        //private IMercadoPagoApiClient _apiClient;
        //private IServiceProvider ServiceProvider { get; set; }        

        private int _orderTimeoutSeconds;
        private IList<string> _errorList;

        private IAuthorizerConfigReader<MercadoPagoAuthorizer> _merchantConfig;

        private async Task<bool> GetConfigAsync(int merchantId)
        {
            _errorList = new List<string>();
            bool result=true;

           _token = await _merchantConfig.GetValueAsync(merchantId,"token", "");
            
            if (_token == "")
            {
                _errorList.Add("Falta configuracion del procesador 'mercadopago': token");
                result = false;
            }
            
            _userId = await _merchantConfig.GetValueAsync(merchantId,"user_id", "");

            if (_userId == "")
            {
                _errorList.Add("Falta configuracion del procesador 'mercadopago': user_id");
                result = false;
            }
                
            if (!int.TryParse(_merchantConfig.GetValueAsync("order_timeout").Result, out _orderTimeoutSeconds))
            {
                _errorList.Add("Error de configuracion del procesador 'mercadopago': 'order_timeout' inválido o faltante");
                result = false;
            }

            return result;
        }

        private async Task<IMercadoPagoApiClient> InitApiClient(int merchantId)
        {
            if (!await GetConfigAsync(merchantId))
            {
                return null;
            }
            
            IMercadoPagoApiClient apiClient;
            try
            {
                apiClient = new MercadoPagoApiClient(_userId, _token, _orderTimeoutSeconds);
            }
            catch (Exception ex)
            {

                _errorList.Add("Error inicalizando el cliente API de MercadoPago: " + ex.Message);
                return null;
            }
            return apiClient;

        }

        public MercadoPagoAuthorizer(IConfiguration config,IAuthorizerConfigReader<MercadoPagoAuthorizer> merchantConfig)
        {
            _merchantConfig = merchantConfig;
            //_apiClient = new MercadoPagoApiClient(_userId, _token, _orderTimeoutSeconds);
        }
       
        private bool IsValidTransaction(TransactionRequest transaction, out IList<string> errorList)
        {
            errorList = new List<string>();

            if (transaction.TransType != TransactionType.Sell)
            {
                errorList.Add("Solo SELL (1) está habilitado por ahora");               
            }

            if (string.IsNullOrEmpty(transaction.PosId))
            {
                errorList.Add("El parametro PosId debe contener un valor");                
            }

            if (string.IsNullOrEmpty(transaction.TransactionReference))
            {
                errorList.Add("El parametro PosId debe contener un valor");
            }

            if (transaction.Items==null || transaction.Items.Count==0 )
            {
                errorList.Add("La colección de items debe tener al menos un elemento");
            }
            return (errorList.Count == 0);

        }

        public async Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest transaction)
        {
           
            if(transaction==null)
            {
                throw (new ArgumentNullException("transDef"));
            }

            if (!IsValidTransaction(transaction,out var errorList))
            {
                return new TransactionResponse(TransactionResponse.ResultCodesEnum.InvalidParametersError,errorList);
            }

            var request = new CreateOrderRequestDto(transaction);

            var apiClient = await InitApiClient(transaction.MerchantId);

            if (apiClient is null)
                return new TransactionResponse(BaseResponse.ResultCodesEnum.NotReady, _errorList);
            
            CreateOrderResponseDto createResponse;
            try
            {
                createResponse = await apiClient.CreateOrderAsync(request);
            }
            catch(HttpRequestException e)
            {
                return new TransactionResponse (TransactionResponse.ResultCodesEnum.CommunicationsError, "Error de comunicación con el host de MercardoPago: " + e.Message);
            }
            

            if(!createResponse.Success)
            {
                if (createResponse.Status == 500)
                {
                    return new TransactionResponse(TransactionResponse.ResultCodesEnum.InternalProcessorError, "Error interno de MercadoPago durante CreatePayment: " + createResponse.ErrorMessage);
                }
                else
                {
                    return new TransactionResponse(TransactionResponse.ResultCodesEnum.ProcessorError, "Error creando la orden: " + createResponse.ErrorMessage);
                }
                
            }

            var timer = new Stopwatch();
            SearchPaymentsResponseDto searchResponse=null;
            timer.Start();

            while (timer.Elapsed < TimeSpan.FromSeconds(_orderTimeoutSeconds + 5))
            {
                try
                {
                    searchResponse = await apiClient.SearchPaymentAsync(transaction.TransactionReference);
                }
                catch (HttpRequestException e)
                {
                    return new TransactionResponse(TransactionResponse.ResultCodesEnum.CommunicationsError, "Error de comunicación con el host de MercardoPago durante SearchPayment: " + e.Message);
                }                

                if(!searchResponse.Success)
                {
                    if (searchResponse.Status == 500)
                    {
                        return new TransactionResponse(TransactionResponse.ResultCodesEnum.InternalProcessorError, "Error interno de MercadoPago durante SearchPayment: " + createResponse.ErrorMessage);
                    }
                    else
                    {
                        return new TransactionResponse(TransactionResponse.ResultCodesEnum.ProcessorError, "Error buscando la orden: " + searchResponse.ErrorMessage);
                    }                    
                }

                foreach(var payment in searchResponse.Payments)
                {
                    if(payment.Payment_Status=="approved")
                    {
                        return new TransactionResponse(TransactionResponse.ResultCodesEnum.Approved) {
                            ResultDescription = "Transaccion Aprobada (" + payment.Payment_Status_Detail + ")",
                            TotalPaid = payment.TotalPaidAmount,
                            NetReceived = payment.NetReceivedAmount,
                            AuthCode = payment.AuthCode,
                            PaymentType= payment.PaymentType switch
                            {
                                "account_money" => PaymentType.AccountMoney,
                                "credit_card" => PaymentType.CreditCard,
                                "debit_card" => PaymentType.DebitCard,
                                "bank_transfer" => PaymentType.BankTransfer,
                                "atm" => PaymentType.Atm,
                                "ticket" => PaymentType.Ticket,
                                "prepaid_card" => PaymentType.PrepaidCard,
                                _ => throw new NotImplementedException()
                            }
                            ,
                            ExternalReference=payment.ExtReference,                            
                            MoneyReleaseDate=payment.MoneyReleaseDate,
                            CardInfo = new CardInfo
                            {
                                CardType = payment.Payment_MethodId,
                                FirstSix = payment.Card_FirstSix,
                                LastFour = payment.Card_LastFour
                            },
                            PayerInfo = new PayerInfo
                            {
                                email = payment.Payer_Email,
                                FirstName = payment.Payer_FirstName,
                                LastName = payment.Payer_LastName
                            },
                            FeeDetails = payment.FeeDetail.Select(s => new FeeDetailItem
                            {
                                Amount = s.Amount,
                                Description = s.FeeType
                            }
                            ).ToList()
                        };
                    }
                }

            } //while


            if (searchResponse.Payments.Count == 0)
            {
                return new TransactionResponse(TransactionResponse.ResultCodesEnum.Timeout)
                {                                        
                    ResultDescription="Timeout esperando el pago"
                };
            }
            else
            {
                var lastRejected = searchResponse.Payments.Last();
                return new TransactionResponse(TransactionResponse.ResultCodesEnum.Rejected)
                {
                    ResultDescription = "Pago rechazado (" + lastRejected.Payment_Status_Detail + ")"
                };
            }
                                 
        }

        public async Task<AdminCommandResponse<IList<CardType>>> GetAcceptedCardTypesAsync()
        {
            return await Task.FromResult(new AdminCommandResponse<IList<CardType>>(BaseResponse.ResultCodesEnum.OperationNotSupported,"GetAcceptedCards no es soportado por la plataforma 'mercadopago'"));
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
        // ~MercadoPagoAuthorizer()
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
