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
    public partial class GenericKeypad : UserControl
    {
        public GenericKeypad()
        {
            InitializeComponent();            
        }
        #region Events
        public event KeyPressEventHandler ButtonPressed;
        public event EventHandler ButtonClick;

        #endregion Events
        private void ButtonClickHandler(object sender, EventArgs e)
        {

        }

        #region Methods
        public void RaiseButtonPressed(char WhatToSend)
        {
            ButtonPressed?.Invoke(this, new KeyPressEventArgs(WhatToSend));
        }
        public void RaiseButtonClick()
        {

        }
        #endregion Methods       
    }
}
