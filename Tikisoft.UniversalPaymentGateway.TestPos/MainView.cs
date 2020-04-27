using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tikisoft.UniversalPaymentGateway.TestPos.Dto;
using Tikisoft.UniversalPaymentGateway.TestPos.Keypads;
using Tikisoft.UniversalPaymentGateway.TestPos.Properties;
using Tikisoft.UniversalPaymentGateway.TestPos.Services;
using Tikisoft.UniversalPaymentGateway.TestPos.ViewModel;

namespace Tikisoft.UniversalPaymentGateway.TestPos
{

    public partial class MainView : Form
    {
        private IList<LineitemVm> _transaction=new List<LineitemVm>();
        private ProcessingForm _processingView = new ProcessingForm();
        public MainView()
        {
            InitializeComponent();
        }

      
        private void MainView_Load(object sender, EventArgs e)
        {
            CatalogKeypad.Display(new Keypads.Product[]
            {
                new Keypads.Product(1,"Coca",1,Resources.Coke),
                new Keypads.Product(2,"Papas",(decimal)1.6,Resources.Papas),
                new Keypads.Product(3,"Hamburguesa y Coca",(decimal)3.5,Resources.HambCoke),
                new Keypads.Product(4,"Combo Familiar",(decimal)3.9,Resources.FamilyCombo),
                new Keypads.Product(5,"Sundae",(decimal)1.1,Resources.Sundae),
                new Keypads.Product(6,"Papas c/Cheddar",(decimal)2.3,Resources.PapasCheddar)
            });

            CatalogKeypad.ProductSelected += new Keypads.CatalogKeypad.ProductSelectedEventHandler(ProductSelectedHandler);

            
         
        }

        private void RefreshView()
        {
            TransactionList.Items.Clear();
            TransTotalLabel.Text = "TOTAL: $0";
            foreach(var item in _transaction)
            {
                var lvi = new ListViewItem(item.Quantity + "x " + item.Description);
                lvi.SubItems.Add("$" + item.Price * item.Quantity);
                TransactionList.Items.Add(lvi);
                
                TransactionList.Columns[0].Width = -2;
                TransactionList.Columns[1].Width = -2;

                var transTotal = _transaction.Sum(i =>i.Price * i.Quantity);
                TransTotalLabel.Text = "TOTAL: $" + transTotal;

            }
        }
        private void ProductSelectedHandler(object sender, ProductSelectedEventArgs e)
        {
            var item = _transaction.Where(p => p.ProductId == e.SelectedProduct.Id).SingleOrDefault();
            if (item!=null)
            {
                var newQuantity = item.Quantity + 1;
                var newItem = new LineitemVm(item.ProductId,item.Description,newQuantity,e.SelectedProduct.Price * newQuantity);
   
                _transaction  = _transaction.Select(c => 
                {
                    if (c.ProductId == item.ProductId)
                    {
                        c.Quantity = newQuantity;
                        c.Price = e.SelectedProduct.Price;
                    }
                    return c; 
                }
                ).ToList();
                
            }
            else
            {
                _transaction.Add(new LineitemVm(e.SelectedProduct.Id, e.SelectedProduct.Desc, 1, e.SelectedProduct.Price));
            }

            
            RefreshView();

            
        }
      
        private string GetNextInvoice()
        {
            string newInvoice;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            
            
            var invoiceSetting = config.AppSettings.Settings["CurrentInvoiceNumber"];
            if(invoiceSetting==null)
            {
                newInvoice = "FAC-0001";
                config.AppSettings.Settings.Add("CurrentInvoiceNumber", "FAC-0001");
            }
            else
            {
                var invoice = invoiceSetting.Value;
                newInvoice = invoice.Substring(0, 4) + (int.Parse(invoice.Substring(4, 4)) + 1).ToString("0000");
                config.AppSettings.Settings["CurrentInvoiceNumber"].Value = newInvoice;
            }
           
            config.Save(ConfigurationSaveMode.Modified);

            return newInvoice;
        }
        private async Task ProcessPayment()
        {
            var baseUrl = ConfigurationManager.AppSettings.Get("UtgUrl");
            var processorName= ConfigurationManager.AppSettings.Get("ProcessorName");

            var url = baseUrl + "/transaction?processor=" + processorName ;

            using (var client = new HttpClient())
            {
                var transDef = new TransDefDto
                {
                    TransType = 1,
                    Amount = _transaction.Sum(i => i.Price),
                    TransactionReference = GetNextInvoice(),
                    PosId = "CAJA0001",
                    Items=_transaction.Select(p => new TransItemDto()
                        {
                            Description = p.Description,
                            Quantity = p.Quantity,
                            UnitPrice = p.Price
                        }).ToList()
                };

                var json = JsonConvert.SerializeObject(transDef);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                ////////////////GET TOKEN/////////////////

                var tokenService = new Auth0TokenService();
                var token = await tokenService.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //////////////////////////////////////////
                _processingView.WaitingPayment(transDef.TransactionReference);
                ChangeEnabled(false);
                var response = await client.PostAsync(url, data);
                var content = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {                    
                    _processingView.PaymentSuccess(content);
                    _transaction.Clear();
                    RefreshView();
                }
                else
                {
                    _processingView.PaymentFailed(response.ReasonPhrase, JToken.Parse(content).ToString(Formatting.Indented));
                }
                ChangeEnabled(true);
            }
        }

        void ChangeEnabled(bool enabled)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = enabled;
            }
        }

        private async void ProcessPaymentButton_Click(object sender, EventArgs e)
        {
            await ProcessPayment();            
        }

    }
}
