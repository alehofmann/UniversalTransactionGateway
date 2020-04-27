using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tikisoft.UniversalPaymentGateway.TestPos.Keypads
{
    public class ProductSelectedEventArgs
    {
        public ProductSelectedEventArgs(Product selectedProduct) { SelectedProduct = selectedProduct; }
        public Product SelectedProduct { get; }
    }

    public partial class CatalogKeypad : GenericKeypad
    {
        private Product[] _products;
        public delegate void ProductSelectedEventHandler(object sender, ProductSelectedEventArgs e);
        public event ProductSelectedEventHandler ProductSelected;

        public CatalogKeypad()
        {
            InitializeComponent();
            this.Visible = false;
        }

        public void Display(Product[] products)
        {
            _products = products;
            for(var index=1;(index <= products.Count()) & index<=6 ; index++)
            {                
                ((Button)this.Controls["btn" + index]).BackgroundImage = products[index - 1].Pic;
                ((Button)this.Controls["btn" + index]).BackgroundImageLayout = ImageLayout.Stretch;
                ((Button)this.Controls["btn" + index]).Click += new EventHandler(ButtonClickHandler);

            }
            this.Visible = true;
        }

        private void ButtonClickHandler(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var productIndex = int.Parse(button.Name.Substring(3))-1;

            ProductSelected?.Invoke(this, new ProductSelectedEventArgs(_products[productIndex]));
        }
    }
}
