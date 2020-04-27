using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
