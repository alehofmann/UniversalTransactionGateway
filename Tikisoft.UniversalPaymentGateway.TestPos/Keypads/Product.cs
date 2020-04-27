using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tikisoft.UniversalPaymentGateway.TestPos.Keypads
{
    public class Product
    {
        private long _id;
        private string _desc;
        private Image _pic;
        private decimal _price;

        public decimal Price => _price;
        public long Id => _id;
        public string Desc => _desc;
        public Image Pic => _pic;

        public Product(long id, string desc, decimal price,Image pic)
        {
            _id = id;
            _desc = desc;
            _pic = pic;
            _price = price;
        }
    }
}
