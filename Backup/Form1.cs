using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientAPI;
using System.Xml;


namespace EESTesterClientAPI
{
    public partial class Form1 : Form
    {
        int index = 0;
        //delegates
        private delegate void ERAccountItemDelegate(ClientAPI.CEventWrapperAccountItem pAccountItem);
        private delegate void OrderITemDelegate(ClientAPI.CEventWrapperOrderItem pItem);
        //Member stuff
        private ClientAPI.CManagement _clientAPIManagement = ClientAPI.CManagement.GetInstance();
        private ClientAPI.CUserInfo _clientAPIWorkingUser = null;
        private delegate void OnProgressDelegate(ClientAPI.CProgress pProgressData);
        Dictionary<string, DepthScreen> dicDepth = null;
        jhlogs log = null;
        Dictionary<string,ulong> orderList = null;
        
        public Form1()
        {
            InitializeComponent();
            dicDepth = new Dictionary<string, DepthScreen>();
            log = new jhlogs("c:\\logs\\clientapiordertimes.log");
        }

        void _clientAPIManagement_OnProgressUpdate(ClientAPI.CProgress pProgressIndicator)
        {
            if (pProgressIndicator.GetProgressType() == ClientAPI.EProgressType.eProgressTypeLogon)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new OnProgressDelegate(_clientAPIManagement_OnProgressUpdate), new object[] { pProgressIndicator });
                    return;
                }
                lblLoginStatus.Text = pProgressIndicator.GetProgressStatement();
            }
        }
       
        private void frmMain_Load(object sender, System.EventArgs e)
        {

            // PAW: Initialize the client api and the GUI fields that interact with it.
         
            _clientAPIManagement.Initialise();
            _clientAPIManagement.OnUserRequestUpdate += new ClientAPI.CManagement.UserRequestUpdate(_clientAPIManagement_OnUserRequestUpdate);
            _clientAPIManagement.OnProgressUpdate += new ClientAPI.CBusinessObjectBase.ProgressUpdate(_clientAPIManagement_OnProgressUpdate);
            orderList = new Dictionary<string,ulong>();
            
        }

        private delegate void OnUserRequestUpdateDelegate(ClientAPI.CUserInfo pUserInfo);

        void _clientAPIManagement_OnUserRequestUpdate(ClientAPI.CUserInfo pUserInfo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new OnUserRequestUpdateDelegate(_clientAPIManagement_OnUserRequestUpdate), new object[] { pUserInfo });
                return;
            }
            if (pUserInfo.UserCurrentState == EUserStatusValues.eUserStatusValueLoggedOn)
            {
                _clientAPIWorkingUser = pUserInfo;
            }

            //Request accounts
            ClientAPI.CAccountDataStore pAccounts = pUserInfo.Accounts;
            pUserInfo.Accounts.OnAccountItemUpdated += OnAccountItem;
            pAccounts.RequestInitialAccounts();

            //SetGuiControlByLogonStatus(pUserInfo.UserCurrentState);
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            
            if (_clientAPIWorkingUser == null)
            {
                if (!_clientAPIManagement.Logon(txtUsername.Text, txtPassword.Text, cmbServer.Text, true, "EasyScreen", "ClientAPIExampleApp"))
                    MessageBox.Show("Logon attempt failed synchronously.");
            }
            else
                _clientAPIManagement.Logoff(_clientAPIWorkingUser.Username.ToUpper());
        }

        private void OnAccountItem(ClientAPI.CEventWrapperAccountItem pItem)
        {
            // This shifts the callback onto the thread context of the main form
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ERAccountItemDelegate(OnAccountItem), new object[] { pItem });
                //return;
            }
            CAccountItem pAccitem = pItem.AccountItem.AccountDataStore.Accounts.First();
            dgvAccounts["Account", 0].Value = pAccitem.Name;
            
            if (!cmbAccount.Items.Contains(pAccitem.Name))
            {
                cmbAccount.Items.Add(pAccitem.Name);
                //regsiter for account orders   and risk updates         
                pAccitem.OnAccountOrderItemUpdated += OnOrderItemUpdated2;
                //Request risk
                CRiskDataStore RiskDS = _clientAPIManagement.GetRiskDataStore();
                RiskDS.RegisterRiskInterest(pItem.AccountItem.ID);
                RiskDS.OnRiskItemUpdated += OnRiskItem;
            }
            
            
            //Get some structure
            //CStructureDataStore StructureDS = _clientAPIManagement.GetStructureDataStore();
            //get allowed exchanges
            //StructureDS.PrepareSecurityExchanges();
            //StructureDS.OnStructureItemUpdated += OnStructureItemUpdated;
            //CStructureItem pItem6A = StructureDS.FindOrCreateItem("CGCF:6A Sep 11");
            //pItem6A.RegisterInterest();
            //CStructureItem pItem1 = StructureDS.FindOrCreateItem("CCLF:");
            //pItem1.RegisterInterest();
            /*//register an interest in CME currencies
            CStructureItem pItem6A = StructureDS.FindOrCreateItem("CGCF:6A Sep 11");
            pItem6A.RegisterInterest();
            CStructureItem pItem6B = StructureDS.FindOrCreateItem("CGCF:6B Sep 11");
            pItem6B.RegisterInterest();
            CStructureItem pItem6C = StructureDS.FindOrCreateItem("CGCF:6C Sep 11");
            pItem6C.RegisterInterest();
            CStructureItem pItem6E = StructureDS.FindOrCreateItem("CGCF:6E Sep 11");
            pItem6E.RegisterInterest();
            CStructureItem pItem6J = StructureDS.FindOrCreateItem("CGCF:6J Sep 11");
            pItem6J.RegisterInterest();
            CStructureItem pItem6S = StructureDS.FindOrCreateItem("CGCF:6S Sep 11");
            pItem6S.RegisterInterest();
            CStructureItem pItemFGBL = StructureDS.FindOrCreateItem("VVF:FGBL Sep 11");
            pItemFGBL.RegisterInterest();
             * */
            //start a marketdatastore
            //CMarketDataStore MarketDS = _clientAPIManagement.GetMarketDataStore();
            //MarketDS.OnMarketDataItemBestUpdated += OnMarketDataItem;
        }

        //create a dictionary
        Dictionary<string, int> dicOrders = new Dictionary<string, int>();
        int iIndexOrder = 0;

        //dictionary for ordertimings
        Dictionary<string, int> dicOrderTimings = new Dictionary<string, int>();
        
        private void OnOrderItemUpdated(ClientAPI.CEventWrapperOrderItem pItem)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new OrderITemDelegate(OnOrderItemUpdated), new object[] { pItem });
                return;
            }
            if (!dicOrderTimings.ContainsKey(pItem.OrderItem.ESUniqueOrderID))
            {
                dicOrderTimings[pItem.OrderItem.ESUniqueOrderID] = pItem.OrderItem.OrdStatus;
            }
            //char c = (char)pItem.OrderEventItem.ExecType;
           // char d = Convert.ToChar(pItem.OrderEventItem.ExecType);
            log.logAdd(DateTime.Now.ToString("hhmmss:fff") + " order updated " + pItem.OrderItem.ESUniqueOrderID + " , " + pItem.OrderItem.OrdStatus + ", SecBOID " + pItem.OrderItem.SecondaryOrderID);
            ClientAPI.COrderDataStore cOrder = _clientAPIManagement.GetOrderDataStore();
            //pItem.
            cOrder.Orders.Count();
            string stat = pItem.OrderItem.OrdStatus.ToString();
            if (!dicOrders.ContainsKey(pItem.OrderItem.OrderID.ToString()) && pItem.OrderItem.OrderID.ToString() != "0")
            {
                iIndexOrder = dgvOrders.Rows.Add(1);
                dicOrders[pItem.OrderItem.OrderID.ToString()] = iIndexOrder;
                orderList[pItem.OrderItem.OrderID.ToString()] = pItem.OrderItem.OrderID;
                dgvOrders["OrderID", iIndexOrder].Value = pItem.OrderItem.OrderID;
                dgvOrders["Mnemonic", iIndexOrder].Value = pItem.OrderItem.Mnemonic;
                dgvOrders["Qty", iIndexOrder].Value = pItem.OrderItem.OrderQty;
                dgvOrders["AccountName", iIndexOrder].Value = pItem.OrderItem.AccountName;
                
                char val = Convert.ToChar(pItem.OrderItem.OrdStatus);
                int iorder = int.Parse(string.Format("{0}",Convert.ToChar(val)));
                dgvOrders["Price", iIndexOrder].Value = pItem.OrderItem.Price;
                dgvOrders["Status", iIndexOrder].Value = iorder;
                //log.logAdd(DateTime.Now.ToString("hhmmss:fff") + " order updated " + pItem.OrderItem.ESUniqueOrderID);
            }
           // int i = dgvOrders.Rows.Add(1);
            //dgvOrders["OrderID", i].Value = pItem.OrderItem.OrderID.ToString();
        }

        private void OnOrderItemUpdated2(ClientAPI.CAccountItem pAccItem, ClientAPI.CEventWrapperOrderItem pItem)
        {
            ClientAPI.COrderItem pRealItem = pItem.OrderItem;
           // char d = Convert.ToChar(pItem.OrderEventItem.ExecType);
           // char s = Convert.ToChar(pItem.OrderEventItem.OrdStatus);
        }
        Dictionary<EPositionTypes,CRiskPosition> dicPostion = new Dictionary<EPositionTypes,CRiskPosition>(); 
        private void OnRiskItem(CEventWrapperRiskItem rItem)
        {
            dgvAccounts["GrossLiquidity", 0].Value = rItem.RiskItem.GrossLiquidity;
            dgvAccounts["AccountStatus", 0].Value = rItem.RiskItem.AccountStatus;
            dgvAccounts["RiskPermissioningLevel", 0].Value = rItem.RiskItem.RiskPermissioningLevel;
            dgvAccounts["AccountCurrency", 0].Value = rItem.RiskItem.AccountCurrency;
            dgvAccounts["Commission", 0].Value = rItem.RiskItem.Commission;
            dgvAccounts["GrossLiquidity", 0].Value = rItem.RiskItem.GrossLiquidity;
          // dicPostion = rItem.RiskItem.Position;
            //rItem.RiskItem.
        }

        private void OnStructureItemUpdated(CEventWrapperStructureItem sItem)
        {

            
            //if (sItem.StructureItem.SecurityExchange == "C" && sItem.StructureItem.Exchange == null)
           // {
            //    sItem.StructureItem.RegisterInterest();
          //  }
            //REGISTER INTEREST FOR CME  
            /*
            if (sItem.StructureItem.SecurityExchange == "C" && sItem.StructureItem.StrategyCode == null && sItem.StructureItem.SecurityType != "OPT" && sItem.StructureItem.Exchange == "CL")
            {
                sItem.StructureItem.RegisterInterest();
                string s = sItem.StructureItem.Code.ToString();

                System.Diagnostics.Debug.WriteLine(s);
                
            }
          //REGISTER INTEREST FOR GC
            if (sItem.StructureItem.Exchange != null && sItem.StructureItem.Exchange == "CL" && sItem.StructureItem.SecurityType == null)
            {
                sItem.StructureItem.RegisterInterest();
                string s = sItem.StructureItem.Code.ToString();

                System.Diagnostics.Debug.WriteLine(s);
                
            }
            if (sItem.StructureItem.Exchange == "C" && sItem.StructureItem.Exchange == "CL" && sItem.StructureItem.SecurityType == "FUT")
            {
                sItem.StructureItem.RegisterInterest();
                string s = sItem.StructureItem.Code.ToString();
            }
            //REGISTER FOR Futures only
            if (sItem.StructureItem.Symbol == "6A" && sItem.StructureItem.SecurityType == "FUT" && sItem.StructureItem.StrategyCode == null)
            {
                sItem.StructureItem.RegisterInterest();
            }
 
            if (sItem.StructureItem.Symbol == "6A" && sItem.StructureItem.IsTradeableEntity == true )
            {
                if (!dicInstruments.ContainsKey(sItem.StructureItem.Code))
                {
                sItem.StructureItem.RegisterInterest();
                sItem.StructureItem.get_GetLeg(1);
                updatePriceGrid(sItem.StructureItem.Code);
                CMarketDataItem MDItem = _clientAPIManagement.GetMarketDataStore().FindOrCreateItem(sItem.StructureItem.Code);
                MDItem.OnMarketDataItemBestUpdated += OnMarketDataItem;
                }
            }
          
            string s = sItem.StructureItem.Code.ToString();
            
            System.Diagnostics.Debug.WriteLine(s);
            sItem.StructureItem.GetHierarchyKey(1);
            //register an interest for CME Currencies
            


            //registers an interest in everything
            //sItem.StructureItem.RegisterInterest();
         */
            if (sItem.StructureItem.Symbol != null && sItem.StructureItem.IsTradeableEntity == true && sItem.StructureItem.SecurityType == "FUT" && sItem.StructureItem.StrategyCode == null)
            {
                if (!dicInstruments.ContainsKey(sItem.StructureItem.Code))
                {
                    string s = sItem.StructureItem.Code.ToString();

                    System.Diagnostics.Debug.WriteLine(s);
                    //sItem.StructureItem.RegisterInterest();
                    sItem.StructureItem.get_GetLeg(1);
                    updatePriceGrid(sItem.StructureItem.Code);
                    CMarketDataItem MDItem = _clientAPIManagement.GetMarketDataStore().FindOrCreateItem(sItem.StructureItem.Code);
                    MDItem.OnMarketDataItemBestUpdated += OnMarketDataItem;
                    MDItem.OnMarketDataItemDepthUpdated += OnMarketDepthItem;
                    //if (!cmbMnemonic.Items.Contains(sItem.StructureItem.Code))
                    //{
                    //    cmbMnemonic.Items.Add(sItem.StructureItem.Code);
                    //}
                }
            }
  
        }

        private void OnMarketDataItem(CEventWrapperMarketDataItem mItem)
        {
            //Repeating group for last trades
            if(mItem.LastTradeHistory != null)
            {
            List<CMarketDataPVD> LIST = mItem.LastTradeHistory;
            }
            string sCode = mItem.MarketDataItem.Code;
            int i = 0;
             //= mItem.MarketDataItem.GetMarketDataEntry;
            //Just last update no repeating group
            CMarketDataPVD pvdItemBestAsk = mItem.MarketDataItem.GetMarketDataEntry(EMarketDataFields.eMarketDataFieldBestAsk);
            CMarketDataPVD pvdItemBestBid = mItem.MarketDataItem.GetMarketDataEntry(EMarketDataFields.eMarketDataFieldBestBid);
            CMarketDataPVD pvdItemLastTrade = mItem.MarketDataItem.GetMarketDataEntry(EMarketDataFields.eMarketDataFieldLastTrade);
            //CMarketDataPVD pvdItemVolume = mItem.MarketDataItem.GetMarketDataEntry(EMarketDataFields.eMarketDataFieldVolumeTraded);
            if (pvdItemBestBid != null && pvdItemBestAsk != null )
            {
                double dBestAsk = pvdItemBestAsk.Price;
                double dBestAskVolume = pvdItemBestAsk.Volume;
                double dBestBid = pvdItemBestBid.Price;
                double dBestBidVolume = pvdItemBestBid.Volume;
                double dLastTrade = pvdItemLastTrade.Price;
                updatePriceGrid(sCode, dBestBidVolume, dBestBid, dBestAsk, dBestAskVolume, dLastTrade);
                //System.Diagnostics.Debug.WriteLine(i + " " + sCode + " " + dBestBidVolume + " " + dBestBid + " , " + dBestAsk + " " + dBestAskVolume);
                i++;
             
            }
        }

        private void OnMarketDepthItem(CEventWrapperMarketDataItem pItem)
        {
            if (dicDepth.ContainsKey(pItem.MarketDataItem.Code))
            {
                if ((pItem.UpdatedFieldBitMask & (uint)ClientAPI.EMarketDataFields.eMarketDataFieldContainsDepth) != 0)
                {
                    foreach (KeyValuePair<string, DepthScreen> key in dicDepth)
                    {
                        ClientAPI.CMarketDataPVD[] pvd2 = pItem.MarketDataItem.GetMarketDataDepth(0xFFFFFFFF, true);
                        for (int i = 0; i < 4; i++)
                        {
                            if (pvd2[i] != null)
                            {
                                key.Value.update(pvd2[i].Price, pvd2[i].Volume, i + 5, pItem.MarketDataItem.Code);
                            }
                        }
                        //IF ITS A SELL
                        ClientAPI.CMarketDataPVD[] pvdSell = pItem.MarketDataItem.GetMarketDataDepth(0xFFFFFFFF, false);
                        for (int j = 3; j >= 0; j--)
                        {
                            if (pvdSell[j] != null)
                            {
                                key.Value.update(pvdSell[j].Price, pvdSell[j].Volume, 4 - j, "sell", pItem.MarketDataItem.Code);
                            }
                        }
                    }
                }

            }
        }

        //create a dictionary
        Dictionary<string, int> dicInstruments = new Dictionary<string, int>();

        private void updatePriceGrid(string sTe) 
        {
            
            if (dgvPrices.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { updatePriceGrid(sTe); }));
            }
            else
            {

                if (!dicInstruments.ContainsKey(sTe))
                {
                    index = dgvPrices.Rows.Add(1);
                    dicInstruments[sTe] = index;
                    dgvPrices["Contract", index].Value = sTe;

                }
            }

        }

        private void updatePriceGrid(string sTe, double BidVolume, double Bid, double Ask, double AskVolume, double LastTrade)
        {
            if (dgvPrices.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { updatePriceGrid(sTe,BidVolume,Bid,Ask,AskVolume,LastTrade); }));
            }
            else
            {
                index = dicInstruments[sTe];
                dgvPrices["BID", index].Value = Bid;
                dgvPrices["BidVolume", index].Value = BidVolume;
                dgvPrices["Ask", index].Value = Ask;
                dgvPrices["AskVolume", index].Value = AskVolume;
                dgvPrices["LastTrade", index].Value = Ask;
            }
            
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            _clientAPIManagement.Logoff(_clientAPIWorkingUser.Username.ToUpper());
            //ClientAPI.CManagement.ReleaseInstance();
            //_clientAPIManagement.Uninitialise();
            _clientAPIWorkingUser = null;
        }
         
        private void btnBuy_Click(object sender, EventArgs e)
        {
            //retrieve the structure from the datastore so we can enrich the order.
            ClientAPI.CStructureItem cStructure = _clientAPIManagement.GetStructureDataStore().FindOrCreateItem(cmbMnemonic.Text);

            //get account info
            ClientAPI.CAccountItem pAccItem = _clientAPIWorkingUser.Accounts.Accounts.First();

            COrderDataStore OrderDS = _clientAPIManagement.GetOrderDataStore();
            COrderEntryItem m_NewOrderTicket = new COrderEntryItem();
            
            switch (cmbOrderType.Text)
            { 
                case "Limit":
                    m_NewOrderTicket.OrdType = (sbyte)'2';
                    break;
                case "Market":
                    m_NewOrderTicket.OrdType = (sbyte)'1';
                    break;
            }

            switch (cmbTimeType.Text)
            { 
                case "Day":
                    m_NewOrderTicket.TimeInForce = (sbyte)'0';
                    break;
                case "GTC":
                    m_NewOrderTicket.TimeInForce = (sbyte)'1';
                    break;
                case "IOC":
                    m_NewOrderTicket.TimeInForce = (sbyte)'3';
                    break;
                case "FOK":
                    m_NewOrderTicket.TimeInForce = (sbyte)'4';
                    break;
                case "GTD":
                    m_NewOrderTicket.TimeInForce = (sbyte)'6';
                    break;
            }
           
            m_NewOrderTicket.Price = double.Parse(txtPrice.Text);
            m_NewOrderTicket.OrderQty = int.Parse(txtLots.Text);
            m_NewOrderTicket.Side = (sbyte)'1';
            //enrich with accout details
            m_NewOrderTicket.EnrichFromAccount(pAccItem);
            //enrich with structure info
            m_NewOrderTicket.EnrichFromStructure(cStructure);
            //add user details
            m_NewOrderTicket.UserDetails = _clientAPIWorkingUser;
            m_NewOrderTicket.AdditionalFields = pAccItem.get_OrderTicketDefaults(cStructure.SecurityExchange);
            m_NewOrderTicket.ESCustomerData = "jh test";
            //subscribe for updates
            m_NewOrderTicket.OnOrderItemUpdated += OnOrderItemUpdated;

            if (OrderDS.EnrichOrderItem(m_NewOrderTicket))
            {
                string strUniqueID = m_NewOrderTicket.ESUniqueOrderID;
                OrderDS.PlaceOrder(m_NewOrderTicket);
                //log.logAdd(DateTime.Now.ToString("hhmmss:fff") + " order placed " + strUniqueID);
            }

        }

        public bool m_PlaceOrder(string sTe, int iSide, int iLots, int iOrderType, double dPrice)
        {
            
            //retrieve the structure from the datastore so we can enrich the order.
            ClientAPI.CStructureItem cStructure = _clientAPIManagement.GetStructureDataStore().FindOrCreateItem(sTe);

            //get account info
            ClientAPI.CAccountItem pAccItem = _clientAPIWorkingUser.Accounts.Accounts.First();

            COrderDataStore OrderDS = _clientAPIManagement.GetOrderDataStore();
            COrderEntryItem m_NewOrderTicket = new COrderEntryItem();

            if (iOrderType == 1)
            {
                m_NewOrderTicket.OrdType = (sbyte)'1';
            }
            if (iOrderType == 2)
            {
                m_NewOrderTicket.OrdType = (sbyte)'2';
            }
            m_NewOrderTicket.TimeInForce = (sbyte)'0';
            m_NewOrderTicket.Price = dPrice;
            m_NewOrderTicket.OrderQty = iLots;
            if (iSide == 1)
            {
                m_NewOrderTicket.Side = (sbyte)'1';
            }
            if (iSide == 2)
            {
                m_NewOrderTicket.Side = (sbyte)'2';
            }
            //enrich with accout details
            m_NewOrderTicket.EnrichFromAccount(pAccItem);
            //enrich with structure info
            m_NewOrderTicket.EnrichFromStructure(cStructure);
            //add user details
            m_NewOrderTicket.UserDetails = _clientAPIWorkingUser;
            m_NewOrderTicket.AdditionalFields = pAccItem.get_OrderTicketDefaults(cStructure.SecurityExchange);

            //subscribe for updates
            m_NewOrderTicket.OnOrderItemUpdated += OnOrderItemUpdated;
            string strUniqueID = m_NewOrderTicket.ESUniqueOrderID;
            if (OrderDS.EnrichOrderItem(m_NewOrderTicket))
            {
                OrderDS.PlaceOrder(m_NewOrderTicket);
                log.logAdd(DateTime.Now.ToString("hhmmss:fff") + " order placed " + strUniqueID);
            }
            //return m_PlaceOrder(sTe,iSide,iLots);
            
            return true;
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            //retrieve the structure from the datastore so we can enrich the order.
            ClientAPI.CStructureItem cStructure = _clientAPIManagement.GetStructureDataStore().FindOrCreateItem(cmbMnemonic.Text);

            //get account info
            ClientAPI.CAccountItem pAccItem = _clientAPIWorkingUser.Accounts.Accounts.First();

            COrderDataStore OrderDS = _clientAPIManagement.GetOrderDataStore();
            COrderEntryItem m_NewOrderTicket = new COrderEntryItem();
            

            switch (cmbOrderType.Text)
            {
                case "Limit":
                    m_NewOrderTicket.OrdType = (sbyte)'2';
                    break;
                case "Market":
                    m_NewOrderTicket.OrdType = (sbyte)'1';
                    break;
            }

            switch (cmbTimeType.Text)
            {
                case "Day":
                    m_NewOrderTicket.TimeInForce = (sbyte)'0';
                    break;
                case "GTC":
                    m_NewOrderTicket.TimeInForce = (sbyte)'1';
                    break;
                case "IOC":
                    m_NewOrderTicket.TimeInForce = (sbyte)'3';
                    break;
                case "FOK":
                    m_NewOrderTicket.TimeInForce = (sbyte)'4';
                    break;
                case "GTD":
                    m_NewOrderTicket.TimeInForce = (sbyte)'6';
                    break;
            }

            m_NewOrderTicket.Price = double.Parse(txtPrice.Text);
            m_NewOrderTicket.OrderQty = int.Parse(txtLots.Text);
            m_NewOrderTicket.Side = (sbyte)'2';
            //enrich with accout details
            m_NewOrderTicket.EnrichFromAccount(pAccItem);
            //enrich with structure info
            m_NewOrderTicket.EnrichFromStructure(cStructure);
            //add user details
            m_NewOrderTicket.UserDetails = _clientAPIWorkingUser;
            m_NewOrderTicket.AdditionalFields = pAccItem.get_OrderTicketDefaults(cStructure.SecurityExchange);
            m_NewOrderTicket.ESCustomerData = "JH TEST";
            //subscribe for updates
            m_NewOrderTicket.OnOrderItemUpdated += OnOrderItemUpdated;

            if (OrderDS.EnrichOrderItem(m_NewOrderTicket))
            {
               bool worked = OrderDS.PlaceOrder(m_NewOrderTicket);
            }
        }

        private void dgvPrices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                string contract = dgvPrices.Rows[e.RowIndex].Cells["Contract"].Value.ToString();
                if (!dicDepth.ContainsKey(contract))
                {
                    DepthScreen dScren = new DepthScreen(contract);
                    dicDepth.Add(contract, dScren);
                    dScren.Show();
                }
            }
        }

        private void _AutoSubscribe()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(EESTesterClientAPI.Properties.Settings.Default.XMLFile);
            XmlNode root = xDoc.DocumentElement;
            XmlNodeList name = xDoc.GetElementsByTagName("contract");
            foreach (XmlNode contract in name)
            {
                Subscribe(contract.InnerText);
            }
        }

        private void Subscribe(string sCommodity)
        {
            //GET THE DATASTORE
            CStructureDataStore StructureDS = _clientAPIManagement.GetStructureDataStore();
            //WHAT TO DO ON STRUCTURE ITEM
            StructureDS.OnStructureItemUpdated += OnStructureItemUpdated;
            //REGISTER FOR COMMODITIES
            CStructureItem pItem1 = StructureDS.FindOrCreateItem(sCommodity);
            pItem1.RegisterInterest();
        }

        private void btnAutoSubscribe_Click(object sender, EventArgs e)
        {
            _AutoSubscribe();
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string strPrice = dgvOrders.Rows[e.RowIndex].Cells["price"].Value.ToString();
            string strQty = dgvOrders.Rows[e.RowIndex].Cells["qty"].Value.ToString();
            string strTemnemonic = dgvOrders.Rows[e.RowIndex].Cells["mnemonic"].Value.ToString();
            string strAccount = dgvOrders.Rows[e.RowIndex].Cells["AccountName"].Value.ToString();
            string orderID = dgvOrders.Rows[e.RowIndex].Cells["OrderID"].Value.ToString();
            ReviseOrder revise = new ReviseOrder(strPrice, strQty, strTemnemonic, strAccount, orderID,this);
            revise.Show();
        }

        public void editOrder(string boid,double price, int quantity)
        {
            //retrieve the structure from the datastore so we can enrich the order.
            //ClientAPI.CStructureItem cStructure = _clientAPIManagement.GetStructureDataStore().FindOrCreateItem(sTe);

            //get account info
            ClientAPI.CAccountItem pAccItem = _clientAPIWorkingUser.Accounts.Accounts.First();
            ulong uOrderID = 0;
            COrderDataStore OrderDS = _clientAPIManagement.GetOrderDataStore();
            COrderItem  ordertoEdit = OrderDS.FindItem(boid);
            
            foreach(KeyValuePair<string,ulong> value in orderList)
            {
                if (value.Key == boid)
                {
                    uOrderID = value.Value;
                }
            }
            
            //COrderEditItem EditOrder = new COrderEditItem(ordertoEdit);
            //EditOrder.Price = price;
            //EditOrder.OrderQty = quantity;
            //OrderDS.EditOrder(uOrderID, _clientAPIWorkingUser, 1, price, 0);
            
            COrderEditItem m_NewOrderTicket = new COrderEditItem(OrderDS.FindItem(uOrderID));
            m_NewOrderTicket.Price = price;
            m_NewOrderTicket.OrderQty = quantity;
            //m_NewOrderTicket.Side = (sbyte)'2';
            //enrich with accout details
            
            //enrich with structure info
            
            //add user details
            
            m_NewOrderTicket.ESCustomerData = "JH TEST";
            //subscribe for updates
            m_NewOrderTicket.OnOrderItemUpdated += OnOrderItemUpdated;

            
                OrderDS.EditOrder(m_NewOrderTicket,_clientAPIWorkingUser);
            
        }
       
    }
}
