using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms
{
    public static class CommandFactory
    {
        public static IList<byte> Refund(RefundRequest request)
        {
            var command = new List<byte>();

            command.AddRange(StringToByteArray("02"));
            command.AddRange(Encoding.ASCII.GetBytes("DEV"));

            command.AddRange(StringToByteArray("6D00"));
            command.AddRange(
                Encoding.ASCII.GetBytes(
                    request.Amount.ToString("N2").Replace(".", "").PadLeft(12, '0')
                    ));

            command.AddRange(Encoding.ASCII.GetBytes(request.CardCode.PadLeft(3, '0')));
            command.AddRange(Encoding.ASCII.GetBytes(request.PlanCode.ToString().Substring(0, 1)));
            command.AddRange(Encoding.ASCII.GetBytes(request.Installments.ToString().PadLeft(2, '0')));
            command.AddRange(Encoding.ASCII.GetBytes(request.OriginalVoucher.ToString().PadLeft(7, '0')));
            command.AddRange(Encoding.ASCII.GetBytes(request.OriginalTransactionDate.ToString("dd/MM/yyyy")));
            command.AddRange(Encoding.ASCII.GetBytes(request.InvoiceNumber.ToString().PadLeft(12, '0')));

            command.AddRange(Encoding.ASCII.GetBytes(request.MerchantNumber.ToString().PadLeft(15, ' ')));
            command.AddRange(Encoding.ASCII.GetBytes(request.MerchantName.ToString().PadLeft(23, ' ')));
            command.AddRange(Encoding.ASCII.GetBytes(request.MerchantCuit.ToString().PadLeft(23, ' ')));
            command.AddRange(StringToByteArray(request.IsOnlineTransaction ? "01" : "00"));
            command.Add(3);
            command.Add(GetCrc(command.ToArray()));

            return command;
        }
        public static IList<byte> GetRequest(BaseRequest request)
        {
            if (request is SellRequest)
                return Sell((SellRequest) request);

            if (request is RefundRequest)
                return Refund((RefundRequest)request);

            return null;
        }
        public static IList<byte> Sell(SellRequest request)
        {
            var command = new List<byte>();
            
            command.AddRange(StringToByteArray("02"));            
                command.AddRange(Encoding.ASCII.GetBytes("VEN"));
            
            command.AddRange(StringToByteArray("6800"));
            command.AddRange(
                Encoding.ASCII.GetBytes(
                    request.Amount.ToString("N2").Replace(".", "").PadLeft(12, '0')
                    ));

            command.AddRange(Encoding.ASCII.GetBytes(request.InvoiceNumber.ToString().PadLeft(12, '0')));
            command.AddRange(Encoding.ASCII.GetBytes(request.Installments.ToString().PadLeft(2,'0')));
            command.AddRange(Encoding.ASCII.GetBytes(request.CardCode.PadLeft(3, '0')));
            command.AddRange(Encoding.ASCII.GetBytes(request.PlanCode.ToString().Substring(0,1)));

                var sellRequest = (SellRequest)request;

                command.AddRange(
                    Encoding.ASCII.GetBytes(
                        sellRequest.TipAmount.ToString("N2").Replace(".", "").PadLeft(12, '0')
                        ));
            
            command.AddRange(Encoding.ASCII.GetBytes(request.MerchantNumber.ToString().PadLeft(15,' ')));
            command.AddRange(Encoding.ASCII.GetBytes(request.MerchantName.ToString().PadLeft(23, ' ')));
            command.AddRange(Encoding.ASCII.GetBytes(request.MerchantCuit.ToString().PadLeft(23, ' ')));
            command.AddRange(StringToByteArray(request.IsOnlineTransaction ? "01": "00"));
            command.Add(3);
            command.Add(GetCrc(command.ToArray()));

            return command;
        }
        public static IList<byte> GetCardTypes(int index)
        {

            var command = new List<byte>();

            //STX
            command.AddRange(StringToByteArray("02"));
            //COMMAND            
            command.AddRange(Encoding.ASCII.GetBytes("TAR"));
            //LENGTH            
            command.AddRange(new byte[] { 4, 0 });
            //INDEX            
            command.AddRange(Encoding.ASCII.GetBytes(index.ToString().PadLeft(4, '0')));
            //ETX
            command.Add(3);
            //CRC
            command.Add(GetCrc(command.ToArray()));

            return command;

        }
        public static SellResponse BuildSellResponse(byte[] source)
        {
            var sourceString = Encoding.Default.GetString(source);

            var retVal = new SellResponse();
            retVal.HostCode = sourceString.Substring(5, 3);
            if(retVal.HostCode=="000")
            {
                retVal.ResponseCode = sourceString.Substring(10, 2);
                retVal.ResponseMessage= sourceString.Substring(12, 32);
                if(retVal.ResponseCode=="00")
                {
                    retVal.AuthCode = int.Parse(sourceString.Substring(44, 6));
                }
                
                retVal.VoucherCode= int.Parse(sourceString.Substring(50, 7));
                retVal.BatchNumber= int.Parse(sourceString.Substring(57, 3));
                retVal.CustomerName = sourceString.Substring(60, 26).Trim();
                retVal.CardLast4 = int.Parse(sourceString.Substring(86, 4));
                retVal.CardFirst6 = int.Parse(sourceString.Substring(90, 6));

                var dateString = sourceString.Substring(96, 10);
                var timeString = sourceString.Substring(106, 8);

                retVal.TransactionDate = new DateTime(
                    int.Parse(dateString.Substring(6, 4)),
                    int.Parse(dateString.Substring(3,2)),
                    int.Parse(dateString.Substring(0,2)),
                    int.Parse(timeString.Substring(0,2)),
                    int.Parse(timeString.Substring(3, 2)),
                    int.Parse(timeString.Substring(6, 2))
                    );
                retVal.TerminalId = int.Parse(sourceString.Substring(114, 8));
            }
            return retVal;          
        }
        #region "Aux string/hex routines"
        private static string StringBytesToAscii(string bytes)
        {
            var res = "";
            bytes = bytes.Replace(" ", "");
            for (int i = 0; i < bytes.Length; i += 2)
            {
                res += Convert.ToChar(Convert.ToInt32(bytes.Substring(i, 2), 16));
            }
            return res;
        }
        private static byte GetCrc(byte[] data)
        {
            byte crc = 0;

            foreach (byte item in data.Skip(1))
            {
                crc = (byte)(crc ^ item);
            }

            return crc;
        }
        private static string AsciiToHex(string ascii)
        {
            var sb = new StringBuilder();

            foreach (byte b in Encoding.UTF8.GetBytes(ascii))
            {
                sb.Append(string.Format("{0:x2}", b));
            }
            return sb.ToString();
        }
        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        #endregion
    }
}
