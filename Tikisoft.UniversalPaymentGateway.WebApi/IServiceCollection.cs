using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway
{
    public interface IServiceCollection : IList<ServiceDescriptor>, ICollection<ServiceDescriptor>,
        IEnumerable<ServiceDescriptor>, IEnumerable
    {
    }
}
