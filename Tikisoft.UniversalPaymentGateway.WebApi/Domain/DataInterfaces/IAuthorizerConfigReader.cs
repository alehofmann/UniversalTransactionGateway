using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces
{
    public interface IAuthorizerConfigReader<TAuthorizer>
    {
        Task<string> GetValueAsync(string key, string defval = "");
        Task<string> GetValueAsync(int merchantId, string key, string defval = "");
    }
}
