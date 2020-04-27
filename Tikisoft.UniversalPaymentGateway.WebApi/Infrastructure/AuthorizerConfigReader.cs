using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.ServiceInterfaces;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;
using TikiSoft.UniversalPaymentGateway.Domain.CustomAttributes;
using TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces;

namespace TikiSoft.UniversalPaymentGateway.Infrastructure
{
    public class AuthorizerConfigReader<TAuthorizer> : IAuthorizerConfigReader<TAuthorizer>
        where TAuthorizer : IAuthorizer
    {
        ConfigDbContext _context;
        string _authorizerName;

        public AuthorizerConfigReader(ConfigDbContext context)
        {

            _context = context;
            _authorizerName = GetAuthorizerName();
            if (_authorizerName == "")
                throw new ArgumentException("Class " + typeof(TAuthorizer).Name + " does not have 'Authorizer' attribute defined");
        }

        private string GetAuthorizerName()
        {
            var attr = typeof(TAuthorizer).GetCustomAttributes(typeof(AuthorizerAttribute), true).FirstOrDefault() as AuthorizerAttribute;
            return attr is null ? "" : attr.Name;
        }

        public async Task<string> GetValueAsync(string key, string defval = "")
        {            
            var configItem = await _context.Config.FirstOrDefaultAsync(p=> p.Processor == _authorizerName & p.Key == key);
            return (configItem != null ? configItem.Value:defval);
            
        }

        public async Task<string> GetValueAsync(int merchantId, string key, string defval = "")
        {
            if(merchantId==0)
            {
                return await (GetValueAsync(key, defval));
            }
            else
            {
                var configItem = await _context.MerchantConfig.FirstOrDefaultAsync(p => p.MerchantId == merchantId & p.Processor == _authorizerName & p.Key == key);
                return (configItem != null ? configItem.Value : defval);
            }
            
        }        
    }
}