namespace Tikisoft.UniversalPaymentGateway.TestPos
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TransactionList = new System.Windows.Forms.ListView();
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Price = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransTotalLabel = new System.Windows.Forms.Label();
            this.CatalogKeypad = new Tikisoft.UniversalPaymentGateway.TestPos.Keypads.CatalogKeypad();
            this.ProcessPaymentButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TransactionList
            // 
            this.TransactionList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.TransactionList.AutoArrange = false;
            this.TransactionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Description,
            this.Price});
            this.TransactionList.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TransactionList.GridLines = true;
            this.TransactionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.TransactionList.HideSelection = false;
            this.TransactionList.Location = new System.Drawing.Point(308, 0);
            this.TransactionList.Name = "TransactionList";
            this.TransactionList.Scrollable = false;
            this.TransactionList.Size = new System.Drawing.Size(300, 198);
            this.TransactionList.TabIndex = 1;
            this.TransactionList.UseCompatibleStateImageBehavior = false;
            this.TransactionList.View = System.Windows.Forms.View.Details;
            // 
            // Description
            // 
            this.Description.Width = 84;
            // 
            // Price
            // 
            this.Price.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Price.Width = 212;
            // 
            // TransTotalLabel
            // 
            this.TransTotalLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TransTotalLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TransTotalLabel.Location = new System.Drawing.Point(308, 201);
            this.TransTotalLabel.Name = "TransTotalLabel";
            this.TransTotalLabel.Size = new System.Drawing.Size(300, 24);
            this.TransTotalLabel.TabIndex = 2;
            this.TransTotalLabel.Text = "TOTAL: $0";
            this.TransTotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CatalogKeypad
            // 
            this.CatalogKeypad.Location = new System.Drawing.Point(0, 0);
            this.CatalogKeypad.Name = "CatalogKeypad";
            this.CatalogKeypad.Size = new System.Drawing.Size(302, 198);
            this.CatalogKeypad.TabIndex = 0;
            this.CatalogKeypad.Visible = false;            
            // 
            // ProcessPaymentButton
            // 
            this.ProcessPaymentButton.Location = new System.Drawing.Point(646, 50);
            this.ProcessPaymentButton.Name = "ProcessPaymentButton";
            this.ProcessPaymentButton.Size = new System.Drawing.Size(118, 95);
            this.ProcessPaymentButton.TabIndex = 3;
            this.ProcessPaymentButton.Text = "Pagar QR";
            this.ProcessPaymentButton.UseVisualStyleBackColor = true;
            this.ProcessPaymentButton.Click += new System.EventHandler(this.ProcessPaymentButton_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 244);
            this.Controls.Add(this.ProcessPaymentButton);
            this.Controls.Add(this.TransTotalLabel);
            this.Controls.Add(this.TransactionList);
            this.Controls.Add(this.CatalogKeypad);
            this.Name = "MainView";
            this.Text = "Universal Payment Gayeway Demo";
            this.Load += new System.EventHandler(this.MainView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Keypads.CatalogKeypad CatalogKeypad;
        private System.Windows.Forms.ListView TransactionList;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ColumnHeader Price;
        private System.Windows.Forms.Label TransTotalLabel;
        private System.Windows.Forms.Button ProcessPaymentButton;
    }
}

