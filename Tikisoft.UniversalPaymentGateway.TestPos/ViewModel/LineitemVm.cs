using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tikisoft.UniversalPaymentGateway.TestPos.ViewModel
{
    class LineitemVm
    {
        public int Quantity { get; set; }
        public long ProductId { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; set; }
        public LineitemVm(long productId,string description,int quantity,decimal price)
        {
            Quantity = quantity;
            ProductId = productId;
            Description = description;
            Price = price;
        }
    }
}
