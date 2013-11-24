namespace EESTesterClientAPI
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblLoginStatus = new System.Windows.Forms.Label();
            this.btnLogoff = new System.Windows.Forms.Button();
            this.btnLogon = new System.Windows.Forms.Button();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.OrderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mnemonic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.Account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrossLiquidity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RiskPermissioningLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountCurrency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Commission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnAutoSubscribe = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLots = new System.Windows.Forms.TextBox();
            this.lblTimeType = new System.Windows.Forms.Label();
            this.lblOrderType = new System.Windows.Forms.Label();
            this.lblAccount = new System.Windows.Forms.Label();
            this.cmbTimeType = new System.Windows.Forms.ComboBox();
            this.cmbOrderType = new System.Windows.Forms.ComboBox();
            this.cmbAccount = new System.Windows.Forms.ComboBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblMnemonic = new System.Windows.Forms.Label();
            this.cmbMnemonic = new System.Windows.Forms.ComboBox();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnBuy = new System.Windows.Forms.Button();
            this.dgvPrices = new System.Windows.Forms.DataGridView();
            this.Contract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BidVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastTrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AskVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depth = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrices)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblLoginStatus);
            this.groupBox1.Controls.Add(this.btnLogoff);
            this.groupBox1.Controls.Add(this.btnLogon);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.lblUsername);
            this.groupBox1.Controls.Add(this.cmbServer);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Location = new System.Drawing.Point(152, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logon Information";
            // 
            // lblLoginStatus
            // 
            this.lblLoginStatus.AutoSize = true;
            this.lblLoginStatus.Location = new System.Drawing.Point(22, 117);
            this.lblLoginStatus.Name = "lblLoginStatus";
            this.lblLoginStatus.Size = new System.Drawing.Size(0, 13);
            this.lblLoginStatus.TabIndex = 8;
            // 
            // btnLogoff
            // 
            this.btnLogoff.Location = new System.Drawing.Point(308, 64);
            this.btnLogoff.Name = "btnLogoff";
            this.btnLogoff.Size = new System.Drawing.Size(75, 23);
            this.btnLogoff.TabIndex = 7;
            this.btnLogoff.Text = "Logoff";
            this.btnLogoff.UseVisualStyleBackColor = true;
            this.btnLogoff.Click += new System.EventHandler(this.btnLogoff_Click);
            // 
            // btnLogon
            // 
            this.btnLogon.Location = new System.Drawing.Point(308, 30);
            this.btnLogon.Name = "btnLogon";
            this.btnLogon.Size = new System.Drawing.Size(75, 23);
            this.btnLogon.TabIndex = 6;
            this.btnLogon.Text = "Logon";
            this.btnLogon.UseVisualStyleBackColor = true;
            this.btnLogon.Click += new System.EventHandler(this.btnLogon_Click);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(56, 92);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 5;
            this.lblServer.Text = "Server";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(56, 64);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(56, 36);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username";
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Items.AddRange(new object[] {
            "easy2test.net"});
            this.cmbServer.Location = new System.Drawing.Point(142, 84);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(121, 21);
            this.cmbServer.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(142, 57);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(142, 30);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(36, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(746, 542);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(738, 516);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Logon";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvOrders);
            this.tabPage2.Controls.Add(this.dgvAccounts);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(738, 516);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account Details";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToDeleteRows = false;
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderID,
            this.Mnemonic,
            this.Status,
            this.Qty,
            this.AccountName,
            this.Price});
            this.dgvOrders.Location = new System.Drawing.Point(33, 203);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.Size = new System.Drawing.Size(674, 209);
            this.dgvOrders.TabIndex = 1;
            this.dgvOrders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellContentClick);
            // 
            // OrderID
            // 
            this.OrderID.HeaderText = "OrderID";
            this.OrderID.Name = "OrderID";
            this.OrderID.ReadOnly = true;
            // 
            // Mnemonic
            // 
            this.Mnemonic.HeaderText = "Mnemonic";
            this.Mnemonic.Name = "Mnemonic";
            this.Mnemonic.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            // 
            // AccountName
            // 
            this.AccountName.HeaderText = "AccountName";
            this.AccountName.Name = "AccountName";
            this.AccountName.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // dgvAccounts
            // 
            this.dgvAccounts.AllowUserToDeleteRows = false;
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Account,
            this.GrossLiquidity,
            this.AccountStatus,
            this.RiskPermissioningLevel,
            this.AccountCurrency,
            this.Commission});
            this.dgvAccounts.Location = new System.Drawing.Point(33, 46);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.ReadOnly = true;
            this.dgvAccounts.Size = new System.Drawing.Size(674, 150);
            this.dgvAccounts.TabIndex = 0;
            // 
            // Account
            // 
            this.Account.HeaderText = "Account";
            this.Account.Name = "Account";
            this.Account.ReadOnly = true;
            // 
            // GrossLiquidity
            // 
            this.GrossLiquidity.HeaderText = "Gross Liquidity";
            this.GrossLiquidity.Name = "GrossLiquidity";
            this.GrossLiquidity.ReadOnly = true;
            // 
            // AccountStatus
            // 
            this.AccountStatus.HeaderText = "AccountStatus";
            this.AccountStatus.Name = "AccountStatus";
            this.AccountStatus.ReadOnly = true;
            // 
            // RiskPermissioningLevel
            // 
            this.RiskPermissioningLevel.HeaderText = "RiskLevel";
            this.RiskPermissioningLevel.Name = "RiskPermissioningLevel";
            this.RiskPermissioningLevel.ReadOnly = true;
            // 
            // AccountCurrency
            // 
            this.AccountCurrency.HeaderText = "Currency";
            this.AccountCurrency.Name = "AccountCurrency";
            this.AccountCurrency.ReadOnly = true;
            // 
            // Commission
            // 
            this.Commission.HeaderText = "Commission";
            this.Commission.Name = "Commission";
            this.Commission.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnAutoSubscribe);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.dgvPrices);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(738, 516);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Prices";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnAutoSubscribe
            // 
            this.btnAutoSubscribe.Location = new System.Drawing.Point(48, 430);
            this.btnAutoSubscribe.Name = "btnAutoSubscribe";
            this.btnAutoSubscribe.Size = new System.Drawing.Size(164, 23);
            this.btnAutoSubscribe.TabIndex = 2;
            this.btnAutoSubscribe.Text = "Auto Subscribe";
            this.btnAutoSubscribe.UseVisualStyleBackColor = true;
            this.btnAutoSubscribe.Click += new System.EventHandler(this.btnAutoSubscribe_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtLots);
            this.groupBox2.Controls.Add(this.lblTimeType);
            this.groupBox2.Controls.Add(this.lblOrderType);
            this.groupBox2.Controls.Add(this.lblAccount);
            this.groupBox2.Controls.Add(this.cmbTimeType);
            this.groupBox2.Controls.Add(this.cmbOrderType);
            this.groupBox2.Controls.Add(this.cmbAccount);
            this.groupBox2.Controls.Add(this.lblPrice);
            this.groupBox2.Controls.Add(this.txtPrice);
            this.groupBox2.Controls.Add(this.lblMnemonic);
            this.groupBox2.Controls.Add(this.cmbMnemonic);
            this.groupBox2.Controls.Add(this.btnSell);
            this.groupBox2.Controls.Add(this.btnBuy);
            this.groupBox2.Location = new System.Drawing.Point(22, 255);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(689, 149);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Orders";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Lots";
            // 
            // txtLots
            // 
            this.txtLots.Location = new System.Drawing.Point(231, 109);
            this.txtLots.Name = "txtLots";
            this.txtLots.Size = new System.Drawing.Size(100, 20);
            this.txtLots.TabIndex = 12;
            // 
            // lblTimeType
            // 
            this.lblTimeType.AutoSize = true;
            this.lblTimeType.Location = new System.Drawing.Point(395, 111);
            this.lblTimeType.Name = "lblTimeType";
            this.lblTimeType.Size = new System.Drawing.Size(57, 13);
            this.lblTimeType.TabIndex = 11;
            this.lblTimeType.Text = "Time Type";
            // 
            // lblOrderType
            // 
            this.lblOrderType.AutoSize = true;
            this.lblOrderType.Location = new System.Drawing.Point(392, 71);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(60, 13);
            this.lblOrderType.TabIndex = 10;
            this.lblOrderType.Text = "Order Type";
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(405, 38);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(47, 13);
            this.lblAccount.TabIndex = 9;
            this.lblAccount.Text = "Account";
            // 
            // cmbTimeType
            // 
            this.cmbTimeType.FormattingEnabled = true;
            this.cmbTimeType.Items.AddRange(new object[] {
            "DayOrder",
            "GTD",
            "GTC"});
            this.cmbTimeType.Location = new System.Drawing.Point(458, 108);
            this.cmbTimeType.Name = "cmbTimeType";
            this.cmbTimeType.Size = new System.Drawing.Size(121, 21);
            this.cmbTimeType.TabIndex = 8;
            this.cmbTimeType.Text = "DayOrder";
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.FormattingEnabled = true;
            this.cmbOrderType.Items.AddRange(new object[] {
            "Limit",
            "Market",
            "EasyStop",
            "EasyStopLimit",
            "IOC",
            "FOK"});
            this.cmbOrderType.Location = new System.Drawing.Point(458, 71);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(121, 21);
            this.cmbOrderType.TabIndex = 7;
            this.cmbOrderType.Text = "Limit";
            // 
            // cmbAccount
            // 
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new System.Drawing.Point(458, 36);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new System.Drawing.Size(121, 21);
            this.cmbAccount.TabIndex = 6;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(160, 41);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(31, 13);
            this.lblPrice.TabIndex = 5;
            this.lblPrice.Text = "Price";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(231, 38);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(121, 20);
            this.txtPrice.TabIndex = 4;
            // 
            // lblMnemonic
            // 
            this.lblMnemonic.AutoSize = true;
            this.lblMnemonic.Location = new System.Drawing.Point(160, 80);
            this.lblMnemonic.Name = "lblMnemonic";
            this.lblMnemonic.Size = new System.Drawing.Size(56, 13);
            this.lblMnemonic.TabIndex = 3;
            this.lblMnemonic.Text = "Mnemonic";
            // 
            // cmbMnemonic
            // 
            this.cmbMnemonic.FormattingEnabled = true;
            this.cmbMnemonic.Location = new System.Drawing.Point(231, 77);
            this.cmbMnemonic.Name = "cmbMnemonic";
            this.cmbMnemonic.Size = new System.Drawing.Size(121, 21);
            this.cmbMnemonic.TabIndex = 2;
            // 
            // btnSell
            // 
            this.btnSell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnSell.Location = new System.Drawing.Point(26, 77);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(88, 38);
            this.btnSell.TabIndex = 1;
            this.btnSell.Text = "Sell";
            this.btnSell.UseVisualStyleBackColor = false;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnBuy
            // 
            this.btnBuy.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnBuy.Location = new System.Drawing.Point(26, 38);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(88, 33);
            this.btnBuy.TabIndex = 0;
            this.btnBuy.Text = "Buy";
            this.btnBuy.UseVisualStyleBackColor = false;
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // dgvPrices
            // 
            this.dgvPrices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Contract,
            this.BidVolume,
            this.Bid,
            this.LastTrade,
            this.Ask,
            this.AskVolume,
            this.Depth});
            this.dgvPrices.Location = new System.Drawing.Point(22, 23);
            this.dgvPrices.Name = "dgvPrices";
            this.dgvPrices.Size = new System.Drawing.Size(689, 216);
            this.dgvPrices.TabIndex = 0;
            this.dgvPrices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrices_CellContentClick);
            // 
            // Contract
            // 
            this.Contract.HeaderText = "Contract";
            this.Contract.Name = "Contract";
            // 
            // BidVolume
            // 
            this.BidVolume.HeaderText = "BidVolume";
            this.BidVolume.Name = "BidVolume";
            // 
            // Bid
            // 
            this.Bid.HeaderText = "Bid";
            this.Bid.Name = "Bid";
            // 
            // LastTrade
            // 
            this.LastTrade.HeaderText = "LastTrade";
            this.LastTrade.Name = "LastTrade";
            // 
            // Ask
            // 
            this.Ask.HeaderText = "Ask";
            this.Ask.Name = "Ask";
            // 
            // AskVolume
            // 
            this.AskVolume.HeaderText = "AskVolume";
            this.AskVolume.Name = "AskVolume";
            // 
            // Depth
            // 
            this.Depth.HeaderText = "Depth";
            this.Depth.Name = "Depth";
            this.Depth.Text = "Depth";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 579);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrices)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnLogoff;
        private System.Windows.Forms.Button btnLogon;
        private System.Windows.Forms.Label lblLoginStatus;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Account;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrossLiquidity;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn RiskPermissioningLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountCurrency;
        private System.Windows.Forms.DataGridViewTextBoxColumn Commission;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvPrices;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblMnemonic;
        private System.Windows.Forms.ComboBox cmbMnemonic;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.Label lblTimeType;
        private System.Windows.Forms.Label lblOrderType;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.ComboBox cmbTimeType;
        private System.Windows.Forms.ComboBox cmbOrderType;
        private System.Windows.Forms.ComboBox cmbAccount;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLots;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mnemonic;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contract;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bid;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastTrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ask;
        private System.Windows.Forms.DataGridViewTextBoxColumn AskVolume;
        private System.Windows.Forms.DataGridViewButtonColumn Depth;
        private System.Windows.Forms.Button btnAutoSubscribe;
    }
}

