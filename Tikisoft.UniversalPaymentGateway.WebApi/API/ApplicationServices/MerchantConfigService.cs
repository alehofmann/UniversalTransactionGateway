using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.API.Dto;
using TikiSoft.UniversalPaymentGateway.Domain.Model.Merchants;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;

namespace TikiSoft.UniversalPaymentGateway.API.ApplicationServices
{
    public interface IMerchantConfigService
    {
        Task<IList<MerchantConfigItem>> GetConfig(string processorName, int merchantId, string terminalId = "default");        
        Task ConfigureProcessor(string processorName, int merchantId, IList<ConfigItemDto> config, string terminalId = "");        
    }

    public class MerchantConfigService : IMerchantConfigService
    {
        ConfigDbContext _context;        

        public MerchantConfigService(ConfigDbContext context)
        {
            _context = context;            
        }

        public async Task ConfigureProcessor(string processorName, int merchantId, IList<ConfigItemDto> config, string terminalId = "")
        {
            var merchant = 
                (await _context.Merchants.Include(p => p.Config).FirstOrDefaultAsync(c => c.Id == merchantId)) 
                ?? throw(new InvalidOperationException(merchantId + " is not a valid merchantId in database"));

            
            terminalId = (terminalId == "" | terminalId is null) ? ConfigDbContext.DefaultTerminalId : terminalId;

            merchant.RemoveConfig(processorName, terminalId);
            await _context.SaveChangesAsync();

            foreach (var configItem in config)
            {
                merchant.AddConfig(processorName, terminalId, configItem.Key, configItem.Value);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IList<MerchantConfigItem>> GetConfig(string processorName, int merchantId, string terminalId = "default")
        {
            terminalId = (terminalId == "" | terminalId is null) ? ConfigDbContext.DefaultTerminalId : terminalId;

            return await _context.MerchantConfig.Where(p => p.Processor == processorName & p.MerchantId == merchantId & p.TerminalId == terminalId).ToListAsync();
        }
    }
}
