using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.ServiceInterfaces;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Data;
using TikiSoft.UniversalPaymentGateway.Domain.CustomAttributes;
using TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Service
{
    [Authorizer("lapos")]
    public class LaPosAuthorizer : IAuthorizer
    {
        private string _merchantName="";
        private string _merchantCuit="";
        private string _comport = "";
        private ICommLayer _commLayer;        
        private IAuthorizerConfigReader<LaPosAuthorizer> _merchantConfig;
        private CardTypeRepository _repo;        
                               
        private bool AuthorizerReady { get; set; } = false;
        
        
        private IList<string> _errorList;

        private async Task<bool> GetConfigAsync()
        {
            _merchantName = await _merchantConfig.GetValueAsync("nombre_comercio");

            if (_merchantName == "")
            {

                _errorList.Add("No se pudo econtrar 'nombre_comercio' en la configuración de LaPos");
                return false;
            }

            _merchantCuit = await _merchantConfig.GetValueAsync("cuit_comercio");

            if (_merchantCuit == "")
            {
                _errorList.Add("No se pudo econtrar 'CuitComercio' en la configuración de lapos");
                return false;
            }

            _comport = await _merchantConfig.GetValueAsync("serial_port");

            if (_comport == "")
            {
                _errorList.Add("No se pudo econtrar 'serial_port' en la configuración de lapos");
                return false;
            }

            return true;
        }
        private async Task<bool> InitializeAsync()
        {
            if (AuthorizerReady)
                return true;
            
            try
            {
                _errorList = new List<string>();

                if (!await GetConfigAsync())
                {
                    AuthorizerReady = false;
                    return false;
                }

                _commLayer.Initialize(_comport);
                await RefreshAcceptedCardsAsync();

                AuthorizerReady = true;
                return true;
            }
            catch (Exception ex)
            {
                _errorList.Add("El autorizador LaPos no pudo ser inicializado: " + ex.Message);
                AuthorizerReady = false;
                return false;
            }

            
            
        }

        public LaPosAuthorizer(IAuthorizerConfigReader<LaPosAuthorizer> merchantConfig, ICommLayer commLayer, CardTypeRepository repo)
        {            
            _merchantConfig = merchantConfig;
            _commLayer = commLayer;
            _repo = repo;
            InitializeAsync().Wait();
        }

        //private void ConfigureServices()
        //{
        //    var services = new ServiceCollection();

        //    services.AddOptions();
        //    services.Configure<AppConfig>(_config.GetSection("Authorizers").GetSection("LaPosAuthorizer"));
        //    services.AddTransient<CommLayer>();

        //    services.AddDbContext<LaPosDbContext>(options =>
        //        options.UseSqlite(_config.GetConnectionString("LaPosDbContext")));

        //    services.AddTransient<CardTypeRepository>();            

        //    _serviceProvider = services.BuildServiceProvider();

                
        //}
        private async Task RefreshAcceptedCardsAsync()
        {
            #region "conditional update"
            //var mustUpdate=false;

            //var configSection = _config.GetSection("TarjetasAceptadas");
            //if(configSection==null) 
            //{                 
            //    throw new ApplicationException("La sección 'TarjetasAceptadas' no se ecunentra en el archivo de configuración"); 
            //}

            //var lastUpdateString = configSection.GetValue<string>("UltimoUpdate");

            //if (DateTime.TryParse(lastUpdateString, out DateTime lastUpdate))
            //{
            //    if (lastUpdate.Date != DateTime.Today) { mustUpdate = true; }
            //}
            //else { mustUpdate = true; }

            //if(mustUpdate)
            //{
            //    var cardTypes=await _commLayer.GetCardTypesAsync();
            //}
            #endregion

            var cardTypesFromLapos = await _commLayer.GetCardTypesAsync();
            var storedCardTypes = await _repo.GetAll();
            
            //Delete records in local DB for card not recognized by LaPos
            foreach(var cardType in storedCardTypes)
            {
                var laPosCardType = cardTypesFromLapos.Single(p => p.CardCode == cardType.CardCode);

                if (laPosCardType is null)                    
                {
                    await _repo.Delete(cardType.CardCode);                
                }
                else
                {                    
                    cardType.Name = laPosCardType.Name;
                    cardType.ProcessorCode = laPosCardType.ProcessorCode;
                    await _repo.Update(cardType);
                }
            }
            //////
                foreach(var cardType in cardTypesFromLapos)
            {
                if (storedCardTypes.First(p=>p.CardCode==cardType.CardCode) is null)
                {
                    await _repo.Add(cardType);
                }
            }
        }
        public async Task<AdminCommandResponse<IList<CardType>>> GetAcceptedCardTypesAsync()
        {
            if(!await InitializeAsync())
            {
                return new AdminCommandResponse<IList<CardType>>(BaseResponse.ResultCodesEnum.NotReady,_errorList);
            }
            
            await RefreshAcceptedCardsAsync();
            
            var cardTypes = await _repo.GetAll();
            
            //Aca falta convertir cardTypes en una Ilist de cardType DE LA APP
            //cardTypes es un iList de ESTE authorizer
            var responseContent = cardTypes.Select(x => new CardType() {Name = x.Name}).ToList();

            return new AdminCommandResponse<IList<CardType>>(BaseResponse.ResultCodesEnum.Approved, responseContent);
        }
        private bool IsValidTransaction(TransactionRequest transaction, out IList<string> errorList)
        {
            errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(transaction.CardCode))
            {                
                errorList.Add("Falta el 'CardCode'");
            }

            if(transaction.Amount<=0)
            {
                errorList.Add("'Amount' debe ser un número mayor que 0");
            }

            return (errorList.Count == 0);
        }
        public async Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest transDef)
        {
            if (transDef == null)
            {
                throw (new ArgumentNullException("transDef"));
            }

            if (!await InitializeAsync())
            {
                return new TransactionResponse(BaseResponse.ResultCodesEnum.NotReady, _errorList);
            }            

            if (!IsValidTransaction(transDef, out var errorList))
            {
                return new TransactionResponse(TransactionResponse.ResultCodesEnum.InvalidParametersError, errorList);
            }
            
            var cardType = await _repo.Get(transDef.CardCode);
            if (cardType is null || !cardType.MerchantNumber.HasValue)
                return new TransactionResponse(BaseResponse.ResultCodesEnum.InvalidParametersError, "El cardCode [" + transDef.CardCode + "] no tiene un 'Código de Comercio' asignado.");

            //Map TransDef to SellRequest, then ProcessSell
            var commandResponse= await _commLayer.ProcessTransaction(
                new SellRequest
                {
                    Amount = transDef.Amount,
                    Installments = transDef.Installments,
                    CardCode = transDef.CardCode,
                    IsOnlineTransaction = true,
                    InvoiceNumber = transDef.InvoiceNumber,
                    MerchantNumber = cardType.MerchantNumber.Value,
                    MerchantCuit = _merchantCuit,
                    MerchantName=_merchantName,
                    TipAmount=transDef.TipAmount,
                    PlanCode=0
                }
                );

            //Map result back to TransResponse
            switch (commandResponse.ResultCode)
            {
                case CommandResponse.ResultCodesEnum.Error:
                    return new TransactionResponse(BaseResponse.ResultCodesEnum.ProcessorError, commandResponse.ResultText);
                case CommandResponse.ResultCodesEnum.Timeout:
                    return new TransactionResponse(BaseResponse.ResultCodesEnum.Timeout, commandResponse.ResultText);
                case CommandResponse.ResultCodesEnum.UserCancel:
                    return new TransactionResponse(BaseResponse.ResultCodesEnum.UserCancel, commandResponse.ResultText);
                case CommandResponse.ResultCodesEnum.Success:
                    var sellResponse = (SellResponse)commandResponse.Content;
                    var retVal = new TransactionResponse()
                    {
                        AuthCode = sellResponse.AuthCode.ToString(),
                        PayerInfo = new PayerInfo
                        {
                            FirstName = sellResponse.CustomerName
                        },
                        ProcessorResponse=sellResponse.ResponseMessage,
                        ResultDescription = sellResponse.ResponseText,
                        ResultCode = sellResponse.ResponseCode switch
                        {
                            "00" => BaseResponse.ResultCodesEnum.Approved,
                            "08" => BaseResponse.ResultCodesEnum.Approved,
                            "85" => BaseResponse.ResultCodesEnum.Approved,
                            "88" => BaseResponse.ResultCodesEnum.Approved,
                            _ => BaseResponse.ResultCodesEnum.Rejected
                        },
                        CardInfo=new CardInfo
                        {
                            CardType=cardType.Name,
                            FirstSix=sellResponse.CardFirst6.ToString(),
                            LastFour=sellResponse.CardLast4.ToString()
                        },
                        VoucherCode=sellResponse.VoucherCode.ToString(),
                        TerminalId=sellResponse.TerminalId.ToString()
                        
                    };
                    retVal.TotalPaid = retVal.ResultCode == BaseResponse.ResultCodesEnum.Approved ? transDef.Amount : 0;
                    retVal.NetReceived = retVal.TotalPaid;
                    return retVal;                
            }
            return new TransactionResponse(BaseResponse.ResultCodesEnum.ProcessorError, "Unexpected response from commLayer: " + commandResponse.ResultText);
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
        // ~LaPosAuthorizer()
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
