﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientAPI;
using System.Xml;
using System.Data.SQLite;


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
        //jhlogs log = null;
        Dictionary<string,ulong> _orderList = null;

        private bool _subscribed = false;
        private SQLiteConnection _db_cnn;
        private DataTable _future_table = new DataTable();
        private Dictionary<string, DataTable> _future_ids_2_optiondefs = new Dictionary<string, DataTable>();
        private DataTable _optiondef_table = new DataTable();
        private DataTable _optioncontract_table = new DataTable();



        public Form1()
        {

            // attach to db
            System.IO.StreamReader file = new System.IO.StreamReader("Config/db.config");
            string dblocation = file.ReadLine().Replace(System.Environment.NewLine, "");
            file.Close();

            try
            {
                _db_cnn = new SQLiteConnection("Data Source=" + dblocation);
                _db_cnn.Open();
            }
            catch
            {
                MessageBox.Show("Can't connect to database. Check config file path.");
                Environment.Exit(0);
            }

            InitializeComponent();
            dicDepth = new Dictionary<string, DepthScreen>();

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
            _orderList = new Dictionary<string,ulong>();
            
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
                //Request risk
                CRiskDataStore RiskDS = _clientAPIManagement.GetRiskDataStore();
                RiskDS.RegisterRiskInterest(pItem.AccountItem.ID);
            }
            
            
            //CMarketDataStore MarketDS = _clientAPIManagement.GetMarketDataStore();
            //MarketDS.OnMarketDataItemBestUpdated += OnMarketDataItem;
        }

        //create a dictionary
        Dictionary<string, int> dicOrders = new Dictionary<string, int>();
        int iIndexOrder = 0;

        //dictionary for ordertimings
        Dictionary<string, int> dicOrderTimings = new Dictionary<string, int>();
        
        private void OnStructureItemUpdated(CEventWrapperStructureItem sItem)
        {


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
                double dLastTradeVol = pvdItemLastTrade.Volume;
                DateTime now = DateTime.Now;
                updatePriceGrid(sCode, dBestBidVolume, dBestBid, dBestAsk, dBestAskVolume, dLastTrade, dLastTradeVol);

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

        private void updateFutureDb(string sTe, double Bid, double BidVolume, double Ask, double AskVolume, double LastTrade, double LastTradeVol, DateTime Now)
        {
            DataRow[] foundFutures = _future_table.Select("easyscreen_id = '" + sTe + "'");
            if (foundFutures.Length == 1)
            {
                DataRow future = foundFutures[0];
                var id = future["id"];
                //List<string> column_headings = (from DataColumn x in _future_table.Columns select x.ColumnName).ToList<string>();
                //int id_index = (int)column_headings.IndexOf("id");
                //var id = future.ItemArray[id_index];
                // do we need to remove and add options? If so, that happens here
                SQLiteCommand command = new SQLiteCommand(_db_cnn);
                command.CommandText = "UPDATE marketdata_future SET bid=:bid, bid_volume=:bid_volume, ask=:ask, ask_volume=:ask_volume, value=:value, last_trade_value=:last_trade_value, last_trade_volume=:last_trade_volume, last_updated=:last_updated WHERE id=:id";
                command.Parameters.AddWithValue("bid", Bid);
                command.Parameters.AddWithValue("bid_volume", BidVolume);
                command.Parameters.AddWithValue("ask", Ask);
                command.Parameters.AddWithValue("ask_volume", AskVolume);
                command.Parameters.AddWithValue("value", (Bid + Ask)/2.0);
                command.Parameters.AddWithValue("last_trade_value", LastTrade);
                command.Parameters.AddWithValue("last_trade_volume", LastTradeVol);
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("last_updated", Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
                updateOptionDb(Convert.ToString(id), (Bid + Ask)/2.0);
            }
        }

        private double return_ATM_strike(double value, double strike_interval)
        {
            //value = 131.1;
            List<double> possible_atms = new List<double>();
            List<double> distance = new List<double>();

            double val = Convert.ToDouble(Convert.ToInt32(value)) - 2 * strike_interval;
            while (true)
            {
                possible_atms.Add(val);
                distance.Add(Math.Pow(val,2));
                val += strike_interval;
                if (val >= value + 2*strike_interval)
                {
                    break;
                }
            }
            return possible_atms[distance.IndexOf(distance.Min())];
        }

        private void updateOptionDb(string future_id, double value)
        {

            double atm_strike;
            List<double> strikes = new List<double>();
            HashSet<string> ees_mnemonics = new HashSet<string>();
            int num_otm_options;
            double val;
            string mnemonic = null;
            double strike_interval;

            DataTable optiondefs = _future_ids_2_optiondefs[future_id];
            foreach (DataRow row in optiondefs.Rows)
            {
                strikes.Clear();
                ees_mnemonics.Clear();
                strike_interval = Convert.ToDouble(row["strike_interval"]);
                atm_strike = return_ATM_strike(value, strike_interval);
                num_otm_options = Convert.ToInt32(row["number_of_OTM_options"]) / 2;
                val = atm_strike - num_otm_options * strike_interval;

                int counter = 0;
                while (true)
                {
                    strikes.Add(val);
                    mnemonic = row["easyscreen_prefix"] + " " + 
                                        (Convert.ToInt32(val/Convert.ToDouble(row["price_movement"]))).ToString();
                    if (val < atm_strike)
                    {
                        mnemonic += "p";
                    }
                    else
                    {
                        mnemonic += "c";
                    }
                    mnemonic += " 0";
                    ees_mnemonics.Add(mnemonic);
                    val += strike_interval;
                    counter += 1;
                    if (counter >= num_otm_options*2 + 1)
                    {
                        break;
                    }
                }

                SQLiteCommand command = new SQLiteCommand(_db_cnn);
                command.CommandText = "SELECT * FROM marketdata_optioncontracts";
                SQLiteDataReader reader = command.ExecuteReader();
                _optioncontract_table.Load(reader);
                reader.Close();

                foreach (DataRow option in _optioncontract_table.Rows)
                {
                    if (!ees_mnemonics.Contains(option["easy_screen_mnemonic"].ToString()))
                    {
                        command.CommandText = "DELETE * FROM marketdata_optioncontracts WHERE id=:id";
                        command.Parameters.AddWithValue("id", option["easy_screen_mnemonic"].ToString());
                        command.ExecuteNonQuery();
                    }

                }


            }



        }

        private void updatePriceGrid(string sTe, double BidVolume, double Bid, double Ask, double AskVolume, double LastTrade, double LastTradeVol)
        {
            if (dgvPrices.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { updatePriceGrid(sTe,BidVolume,Bid,Ask,AskVolume,LastTrade,LastTradeVol); }));
            }
            else
            {
                index = dicInstruments[sTe];
                dgvPrices["BID", index].Value = Bid;
                dgvPrices["BidVolume", index].Value = BidVolume;
                dgvPrices["Ask", index].Value = Ask;
                dgvPrices["AskVolume", index].Value = AskVolume;
                dgvPrices["LastTrade", index].Value = LastTrade;
                DateTime Now = DateTime.Now;

                updateFutureDb(sTe, Bid, BidVolume, Ask, AskVolume, LastTrade, LastTradeVol, Now);

            }
            
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            try
            {
                _clientAPIManagement.Logoff(_clientAPIWorkingUser.Username.ToUpper());
                _clientAPIWorkingUser = null;
                // detach from db
                _db_cnn.Close();
                lblLoginStatus.Text = "Account logged out.";
                MessageBox.Show("Successfully detached & logged out.");
            }
            catch
            {
                //
            }
            this.Close();
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
            /*
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(EESTesterClientAPI.Properties.Settings.Default.XMLFile);
            XmlNode root = xDoc.DocumentElement;
            XmlNodeList name = xDoc.GetElementsByTagName("contract");
            */
            // load the futures
            SQLiteCommand command = new SQLiteCommand(_db_cnn);
            command.CommandText = "SELECT * FROM marketdata_future";
            SQLiteDataReader reader = command.ExecuteReader();
            _future_table.Load(reader);
            reader.Close();
            DateTime Now = DateTime.Now;
            foreach (DataRow r in _future_table.Rows)
            {
                int diff = DateTime.Compare(Now, Convert.ToDateTime(r["expiry_date"]));
                if (diff <= 0)
                {
                    // subscribe future
                    string delete = r["easyscreen_id"].ToString();
                    Subscribe(r["easyscreen_id"].ToString());

                    // now find all the option defs based on subscribed future
                    command.CommandText = "SELECT * FROM marketdata_optiondefinition WHERE future_id=:id";
                    command.Parameters.AddWithValue("id", r["id"].ToString());
                    reader = command.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);
                    reader.Close();
                    _future_ids_2_optiondefs.Add(r["id"].ToString(), table);
                }
            }
            // load the options
//            command = new SQLiteCommand(_db_cnn);
//            command.CommandText = "SELECT id, easyscreen_id, bid, bid_volume, ask, ask_volume, last_trade_value, last_trade_volume, last_updated, expiry_date FROM marketdata_future";
//            SQLiteDataReader reader = command.ExecuteReader();
//            _future_table.Load(reader);
//            reader.Close();
//            foreach (DataRow r in _future_table.Rows)
//            {
//                Subscribe(r["easyscreen_id"].ToString());
//            }
            /*

            foreach (XmlNode contract in name)
            {
                Subscribe(contract.InnerText);
            }
            */
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
            if (_subscribed == false)
            {
                _AutoSubscribe();
            }
            _subscribed = true;
        }

    }
}
