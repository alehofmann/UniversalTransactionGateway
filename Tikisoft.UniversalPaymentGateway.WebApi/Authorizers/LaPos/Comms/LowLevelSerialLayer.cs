using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text;
using System.Threading;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms
{
    public class LowLevelSerialLayer : IDisposable
    {
        private SerialPort _port;
        private AutoResetEvent _waiter = new AutoResetEvent(false);
        StringBuilder _buffer = new StringBuilder();
        private const byte Etx = 3;
        private const byte Ack = 6;

        public LowLevelSerialLayer(string comport)
        {            
            try
            {
                _port = new SerialPort(comport, 19200, Parity.None, 8, StopBits.One);
                _port.Open();
            }
            catch (System.IO.IOException ex)
            {
                throw (new ApplicationException("El puerto " + comport + " no pudo ser encontrado o abierto", ex));
            }

            _port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            
        }

        

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _buffer.Append(_port.ReadExisting());
            if (_buffer.ToString().Contains(Convert.ToChar(Etx)) |
                _buffer.ToString().Contains(Convert.ToChar(Ack))
                )
            {
                _waiter.Set();
                //buffer.Clear();
            }
        }

        public async Task<(bool success,byte[] responseBytes)> WaitForResponse(TimeSpan timeout)
        {
            await Task.Delay(300);
            if (!_waiter.WaitOne((int)timeout.TotalMilliseconds))
            {                
                return (false, null);
            }
            
            return (true, ASCIIEncoding.ASCII.GetBytes(_buffer.ToString()));
        }
        public async Task<string> SendCommandAsync(byte[] data, int responseTimeoutSeconds=0)
        {            

            if (data.Length == 0)
            {
                throw new ArgumentNullException("data", "data is empty, cannot send");
            }

         
                _buffer.Clear();                
                _port.Write(data, 0, data.Length);

                if (responseTimeoutSeconds!=0)
                {                                   
                    var waitResult = await WaitForResponse(TimeSpan.FromSeconds(responseTimeoutSeconds));

                    if (!waitResult.success)
                        { throw new TimeoutException("Timeout esperando respuesta del Pinpad"); }
                    return Encoding.Default.GetString(waitResult.responseBytes);
                }                    
                else { return null; }

          
        }

        public void Dispose()
        {
            if (!(_port is null) && _port.IsOpen)
            {
                _port.Close();
                _port.Dispose();
            }
        }
    }
}
