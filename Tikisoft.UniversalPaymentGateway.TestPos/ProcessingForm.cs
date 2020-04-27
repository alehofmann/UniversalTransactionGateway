using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tikisoft.UniversalPaymentGateway.TestPos
{
    public partial class ProcessingForm : Form
    {
        public ProcessingForm()
        {
            InitializeComponent();
            OkButton.Click += new EventHandler(OkButtonClick);
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void WaitingPayment(string transactionReference)
        {
            ResetForm();
            this.Text = "Processing Transaction " + transactionReference;
            this.StatusLabel.Text = "Esperando el pago...";
            OkButton.Enabled = false;
            this.Show();
        }
        public void PaymentFailed(string errorCode, string errorDetail)
        {
            this.StatusLabel.Text = "Error en pago: " + errorCode;
            MessageBox.Show(errorDetail, errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            OkButton.Enabled = true;
        }

        public void PaymentSuccess(string resultJson)
        {
            JObject o = JObject.Parse(resultJson);

            var resultCodeString = (string)o.SelectToken("result_code");
            var resultDescription = (string)o.SelectToken("result_description");
            var totalPaid = (decimal)o.SelectToken("total_paid");
            var netReceived = (decimal)o.SelectToken("net_received");
            var totalFees = (decimal)o.SelectToken("total_fees");
            var cardType = (string)o.SelectToken("card_info").SelectToken("card_type");
            var firstSix= (string)o.SelectToken("card_info").SelectToken("first_six");
            var lastFour= (string)o.SelectToken("card_info").SelectToken("last_four");

            StatusLabel.Text= resultCodeString;
            if (!string.IsNullOrEmpty(resultDescription)) { StatusLabel.Text += " (" + resultDescription + ")"; }
            PaidLabel.Text = totalPaid.ToString();
            ReceivedLabel.Text = netReceived.ToString();
            if(!string.IsNullOrEmpty(cardType)) 
            {
                CardLabel.Text = cardType;
                if(!string.IsNullOrEmpty(firstSix)) { CardLabel.Text += " " + firstSix + "XXXXXX" + lastFour; }
            }
            JArray feeDetails = (JArray)o.SelectToken("fee_details");
            foreach(JToken feeItem in feeDetails)
            {
                if (FeesLabel.Text == "-")
                {
                    FeesLabel.Text = "";
                }
                else
                {
                    FeesLabel.Text += System.Environment.NewLine;
                }

                FeesLabel.Text += (string)feeItem.SelectToken("description") + ": $" + (string)feeItem.SelectToken("amount");
            }

            MoneyReleaseLabel.Text = ((DateTime)o.SelectToken("money_release_date")).ToString();
            AuthCodeLabel.Text = (string)o.SelectToken("auth_code").ToString();
            ExtRefLabel.Text = (string)o.SelectToken("external_reference").ToString();
            PaymentTypeLabel.Text= (string)o.SelectToken("payment_type").ToString();

            this.StatusLabel.Text = resultCodeString + " (" + resultDescription + ")";
            OkButton.Enabled = true;
        }

        private void ResetForm()
        {
            PaidLabel.Text = "-";
            ReceivedLabel.Text = "-";
            CardLabel.Text = "-";
            FeesLabel.Text = "-";
            MoneyReleaseLabel.Text = "-";
            AuthCodeLabel.Text = "-";
            ExtRefLabel.Text = "-";
            PaymentTypeLabel.Text = "-";

        }
        private void ProcessingForm_Load(object sender, EventArgs e)
        {

        }

        private void ReceivedLabel_Click(object sender, EventArgs e)
        {

        }

        private void PaidLabel_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void OkButton_Click(object sender, EventArgs e)
        {

        }
    }
}
