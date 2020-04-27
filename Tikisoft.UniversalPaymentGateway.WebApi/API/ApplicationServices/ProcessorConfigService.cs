using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.API.Dto;
using TikiSoft.UniversalPaymentGateway.Domain.Model;
using TikiSoft.UniversalPaymentGateway.Domain.Model.Merchants;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;

namespace TikiSoft.UniversalPaymentGateway.API.ApplicationServices
{
    public interface IProcessorConfigService
    {        
        Task<IList<ConfigItem>> GetConfig(string processorName, string terminalId="default");             
        Task ConfigureProcessor(string processorName, IList<ConfigItemDto> config, string terminalId="default");
    }
    public class ProcessorConfigService : IProcessorConfigService
    {        
        ConfigDbContext _context;

        public ProcessorConfigService(ConfigDbContext context)
        {
            _context = context;            
        }

        public async Task<IList<ConfigItem>> GetConfig(string processorName, string terminalId = "")
        {
            terminalId = (terminalId == "") ? ConfigDbContext.DefaultTerminalId : terminalId;

            return await _context.Config.Where(p => p.Processor == processorName & p.TerminalId == terminalId).ToListAsync();
        }

        public async Task ConfigureProcessor(string processorName, IList<ConfigItemDto> config, string terminalId = "")
        {
            terminalId = (terminalId == "") ? ConfigDbContext.DefaultTerminalId : terminalId;

            var itemsToDelete = await GetConfig(processorName, terminalId);

            foreach (var item in itemsToDelete)
            {
                _context.Remove(item);
            }
            await _context.SaveChangesAsync();

            foreach (var item in config)
            {
                _context.Add
                    (new ConfigItem
                    {
                        Processor = processorName,
                        Key = item.Key,
                        Value = item.Value,
                        TerminalId = terminalId
                    });
            }

            await _context.SaveChangesAsync();
        }
    }
}
