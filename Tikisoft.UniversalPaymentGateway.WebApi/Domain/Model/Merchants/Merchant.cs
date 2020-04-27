using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model.Merchants
{    
    public class Merchant
    {                
        public int Id { get; set; }
          
        public string Name { get; set; }

        public List<MerchantConfigItem> Config { get; set; } = new List<MerchantConfigItem>();

        public void RemoveConfig(string processor)
        {
            Config.RemoveAll(item => item.Processor == processor);
        }
        public void RemoveConfig(string processor, string terminalId)
        {            
            Config.RemoveAll(item => item.Processor == processor & item.TerminalId == terminalId);            
            
        }
        public void AddConfig(string processor, string terminalId,string key, string value)
        {
            if(Config.FirstOrDefault(p=>p.Processor==processor & p.Key==key & p.TerminalId==terminalId ) is null)
            {
                Config.Add(
                    new MerchantConfigItem
                    {
                        Processor = processor,
                        Key = key,
                        Value = value,
                        TerminalId = terminalId,
                        Merchant = this
                    }
                    );
            }
        }
    }
}
