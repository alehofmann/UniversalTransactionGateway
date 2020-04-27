using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizerAttribute : Attribute
    {
        public string Name { get; }
        public AuthorizerAttribute(string name)
        {
            Name = name;
        }
    }
}
