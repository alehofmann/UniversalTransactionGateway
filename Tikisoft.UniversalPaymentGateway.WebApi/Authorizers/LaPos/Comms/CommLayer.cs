using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text;
using System.Threading;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model;
using Microsoft.Extensions.Options;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms
{
    public class CommLayer : ICommLayer
    {
        //private LowLevelSerialLayer _lowLevelSerial;
        private string _comport;

        //public CommLayer(IOptions<AppConfig> appConfig)
        //{            
        //    _comport=appConfig.Value.Comport;                        
        //}

        //public CommLayer(string comport)
        //{

        //    _comport = comport;
        //}


        //public async Task<bool> TestPinpad()
        //{
        //    var command = new List<byte>();

        //    command.AddRange(StringToByteArray("02"));
        //    command.AddRange(StringToByteArray(AsciiToHex("TES")));
        //    command.AddRange(StringToByteArray("0400"));            
        //    command.AddRange(StringToByteArray("03"));
        //    command.Add(GetCrc(command.ToArray()));

        //    var responseBytes = await _lowLevelSerial.SendCommandAsync(command.ToArray());
            
        //    return responseBytes == "060254455330303100000370";

        //}
        
        public async Task<IList<Model.CardType>> GetCardTypesAsync()
        {            
            var resultCode = "";
            var cardTypesList = new List<CardType>();
            int cardTypeIndex = 0;

            using (var serial=new LowLevelSerialLayer(_comport))
            {
                do
                {

                    if (!await SendEnqAsync(serial))
                    {
                        throw new ApplicationException("El Pinpad no respondió al comando 'Enq'");
                    }

                    var commandResponse = await serial.SendCommandAsync
                        (CommandFactory.GetCardTypes(cardTypeIndex).ToArray(),10);

                    await serial.SendCommandAsync(new byte[] { 6 }, 0);

                    resultCode = commandResponse.Substring(2, 6);                

                    cardTypesList.Add(new CardType 
                    { 
                        ProcessorCode = commandResponse.Substring(14, 3).Trim(),
                        CardCode = commandResponse.Substring(17, 3).Trim(),
                        Name = commandResponse.Substring(20, 16).Trim()
                });
                
                    cardTypeIndex += 1;                
                
                } while (resultCode == "TAR001");
            }

            return cardTypesList;


        }

        public void Initialize(string comport)
        {
            _comport = comport;
        }

        public async Task<CommandResponse> ProcessTransaction(BaseRequest request)
        {
            //var response=new SellResponse();

            using (var serial = new LowLevelSerialLayer(_comport))
            {

                if (!await SendEnqAsync(serial))
                {
                    return new CommandResponse(
                        CommandResponse.ResultCodesEnum.Timeout, "Error enviando comando 'Enq'"
                        );
                }

                
                var commandResponse = await serial.SendCommandAsync
                    (CommandFactory.GetRequest(request).ToArray(), 10);
                

                if(ASCIIEncoding.ASCII.GetBytes(commandResponse)[0] != 6)
                {
                    return new CommandResponse(
                        CommandResponse.ResultCodesEnum.Error, "El comando no devolvió ACK"
                        );                    
                }
                
                var sellResult = await serial.WaitForResponse(TimeSpan.FromSeconds(120));

                if (!sellResult.success)
                {
                    return new CommandResponse(
                       CommandResponse.ResultCodesEnum.Timeout, "Timeout esperando el procesamiento del comando"
                       );                    
                }

                //send ack
                await serial.SendCommandAsync(new byte[] { 6 }, 0);

                if (sellResult.responseBytes[0] != 6)
                {
                    return new CommandResponse(
                        CommandResponse.ResultCodesEnum.Error, "El comando no devolvió ACK"
                        );
                }

                var sellResponse = CommandFactory.BuildSellResponse(sellResult.responseBytes);

                if (sellResponse.HostCode=="201")
                {
                    return new CommandResponse(
                    CommandResponse.ResultCodesEnum.UserCancel, "Transacción cancelada o timeout desde el Pinpad");
                }
                else
                {
                    return new CommandResponse(
                    CommandResponse.ResultCodesEnum.Success,
                    "",
                    sellResponse);
                }

                
            }

        }

        private async Task<bool> SendEnqAsync(LowLevelSerialLayer serial)
        {
            //var command= StringToByteArray("05");
            var command = new byte[] { 0, 5 };
            string response;

            try
            {
                response = await serial.SendCommandAsync(command,10);
            }
            catch (TimeoutException)
            {
                return false;
            }

            return ASCIIEncoding.ASCII.GetBytes(response)[0] == 6;
        }

    
    }

}
