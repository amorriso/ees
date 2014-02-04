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
using System.Data.SQLite;
using MySql.Data.MySqlClient;


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
        private DataTable _future_table = new DataTable();
        private Dictionary<string, DataTable> _future_ids_2_optiondefs = new Dictionary<string, DataTable>();
        private Dictionary<string, bool> _cleaned_optiondefs = new Dictionary<string, bool>();
        private Dictionary<string, string> _database_events = new Dictionary<string, string>();
        private Dictionary<string, string> _database_add_event = new Dictionary<string, string>();
        private Dictionary<string, DataTable> _optiondef2optioncontracts = new Dictionary<string, DataTable>();
        private Dictionary<string, int> _mnemonic2optioncontractid = new Dictionary<string, int>();
        private bool _database_dict_locked = false;
        private DataTable _optiondef_table = new DataTable();

        //BackgroundWorker _worker = new BackgroundWorker();

        //private SQLiteConnection _db_cnn;
        private MySqlConnection _db_cnn;



        public Form1()
        {

            // attach to db
            System.IO.StreamReader file = new System.IO.StreamReader("Config/db.config");
            string dblocation = file.ReadLine().Replace(System.Environment.NewLine, "");
            file.Close();

            try
            {
                //_db_cnn = new SQLiteConnection(dblocation);
                _db_cnn = new MySqlConnection(dblocation);
                _db_cnn.Open();
            }
            catch
            {
                MessageBox.Show("Can't connect to database. Check config file path.");
                Environment.Exit(0);
            }

            InitializeComponent();
            this.Text = "EESDataFetcher";
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


            if (sItem.StructureItem.Symbol != null && sItem.StructureItem.IsTradeableEntity == true && sItem.StructureItem.StrategyCode == null)
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
                string id = Convert.ToString(future["id"]);
                string command = "UPDATE marketdata_future SET bid=" + Convert.ToString(Bid) + ", bid_volume=" + Convert.ToString(BidVolume) + ", ask=" + Convert.ToString(Ask) + ", ask_volume=" + Convert.ToString(AskVolume) + ", value=" + Convert.ToString((Bid + Ask)/2.0) + ", last_trade_value=" + Convert.ToString(LastTrade) + ", last_trade_volume=" + Convert.ToString(LastTradeVol) + ", last_updated='" + Convert.ToString(Now.ToString("yyyy-MM-dd HH:mm:ss")) + "' WHERE id=" + Convert.ToString(id) + "";
                update_db_events("marketdata_future_" + id, (string)command.Clone());
                updateOptionDb(id, (Bid + Ask)/2.0);
            }
        }

        private double return_ATM_strike(double value, double strike_interval)
        {
            //value = 131.1;
            //List<double> possible_atms = new List<double>();
            //List<double> distance = new List<double>();

            //double val = Convert.ToDouble(Convert.ToInt32(value)) - 2 * strike_interval;
            double val = (double)Convert.ToInt32((value - 2 * strike_interval) / strike_interval) * strike_interval;
            double old_dist = 1000000000.00;
            double new_dist = old_dist;

            while (true)
            {
                new_dist = Math.Pow(val - value, 2);
                if (new_dist >= old_dist)
                    return val;
                val += strike_interval;
                old_dist = new_dist;
                if (val >= value + 2*strike_interval)
                {
                    break;
                }
            }
            return val;
        }

        private void updateOptionDb(string sTe, double Bid, double BidVolume, double Ask, double AskVolume, double LastTrade, double LastTradeVol, DateTime Now)
        {

            if (_mnemonic2optioncontractid.ContainsKey(sTe))
            {
                int id = _mnemonic2optioncontractid[sTe];
                string command = "UPDATE marketdata_optioncontract SET bid=" + Convert.ToString(Bid) + ", bid_volume=" + Convert.ToString(BidVolume) + ", ask=" + Convert.ToString(Ask) + ", ask_volume=" + Convert.ToString(AskVolume) + ", value=" + Convert.ToString((Bid + Ask) / 2.0) + ", last_trade_value=" + Convert.ToString(LastTrade) + ", last_trade_volume=" + Convert.ToString(LastTradeVol) + ", last_updated='" + Convert.ToString(Now.ToString("yyyy-MM-dd HH:mm:ss")) + "' WHERE id=" + Convert.ToString(id);
                update_db_events("marketdata_optioncontract_" + Convert.ToString(id), (string)command.Clone());
            }
            
        }


        private void updateOptionDb(string future_id, double value)
        {

            // initialise some stuff
            double atm_strike;
            HashSet<string> ees_mnemonics = new HashSet<string>();
            HashSet<string> registered_ees_mnemonics = new HashSet<string>();
            Dictionary<string, string> ees_mnemonics2strike = new Dictionary<string, string>();
            Dictionary<string, string> ees_mnemonics2bloomberg = new Dictionary<string, string>();
            int num_otm_options;
            double val;
            string mnemonic = null;
            string bloomberg_postfix = null;
            double strike_interval;
            //SQLiteCommand command = new SQLiteCommand(_db_cnn);
            MySqlCommand command = new MySqlCommand();

            // get our option defintions (there could be up to 3 of these 
            // per future id. these define collections of options (with
            // different strikes)
            DataTable optiondefs = _future_ids_2_optiondefs[future_id];

            // for each option defintion
            foreach (DataRow row in optiondefs.Rows)
            {
                ees_mnemonics.Clear();
                strike_interval = Convert.ToDouble(row["strike_interval"]);
                atm_strike = return_ATM_strike(value, strike_interval);
                num_otm_options = Convert.ToInt32(row["number_of_OTM_options"]) / 2;
                val = atm_strike - (num_otm_options * strike_interval);// +strike_interval * 2;

                int counter = 0;

                // create the easy screen mnemonics
                while (true)
                {
                    // I used to believe the price_movement was required to create the EES mnemonic. It isn't. To create the menmonic
                    // you simply convert multiply the value by 100 (or equally as is done below divide by 0.01).
                    //bloomberg_postfix = (Convert.ToInt32(val / Convert.ToDouble(row["price_movement"]))).ToString();
                    bloomberg_postfix = (Convert.ToInt32(val / 0.01)).ToString(); 
                    if (val < atm_strike)
                    {
                        bloomberg_postfix += "p";
                    }
                    else
                    {
                        bloomberg_postfix += "c";
                    }
                    bloomberg_postfix += " 0";
                    mnemonic = row["easyscreen_prefix"] + " " + bloomberg_postfix;
                    ees_mnemonics.Add(mnemonic);
                    ees_mnemonics2strike.Add(mnemonic, Convert.ToString(val));
                    ees_mnemonics2bloomberg.Add(mnemonic, row["bloomberg_prefix"] + " " + bloomberg_postfix);
                    val += strike_interval;
                    if (counter >= num_otm_options*2 - 1)// 
                    {
                        break;
                    }
                    counter += 1;
                }

                DataTable optioncontract_table = _optiondef2optioncontracts[Convert.ToString(row["id"])];

                // remove all options that we don't need to fetch anymore
                foreach (DataRow option in optioncontract_table.Rows)
                {
                    if (!ees_mnemonics.Contains(option["easy_screen_mnemonic"].ToString()) && !_cleaned_optiondefs[Convert.ToString(row["id"])])
                    {
                        command.Connection = _db_cnn;
                        command.Parameters.Clear();
                        command.CommandText = "DELETE FROM marketdata_optioncontract WHERE id=@id";
                        command.Parameters.AddWithValue("@id", Convert.ToString(option["id"]));
                        command.ExecuteNonQuery();

                        // we now require that options be successfully deleted from database, only once a day should this happen
                        CStructureDataStore StructureDS = _clientAPIManagement.GetStructureDataStore();
                        CStructureItem pItem1 = StructureDS.FindOrCreateItem(option["easy_screen_mnemonic"].ToString());
                        pItem1.UnregisterInterest();
                    }
                    else
                    {
                        registered_ees_mnemonics.Add(option["easy_screen_mnemonic"].ToString());
                    }
                }

                _cleaned_optiondefs[Convert.ToString(row["id"])] = true;

                // add options that we now need to fetch that we weren't already fetching
                string add_event = null;
                string expiry_date = null;
                foreach (string option_mnemonic in ees_mnemonics)
                {
                    if (!registered_ees_mnemonics.Contains(option_mnemonic))
                    {
                        expiry_date = Convert.ToDateTime(row["expiry_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                        add_event = "INSERT INTO marketdata_optioncontract (optiondefinition_id, easy_screen_mnemonic, bloomberg_name, strike, bid, bid_volume, ask, ask_volume, value, last_trade_value, last_trade_volume, vol, delta, expiry_date, time_to_expiry, last_updated) VALUES (" + Convert.ToString(Convert.ToString(row["id"])) + ", '" + Convert.ToString(option_mnemonic) + "', '" + Convert.ToString(ees_mnemonics2bloomberg[option_mnemonic]) + "', " + Convert.ToString(Convert.ToDouble(ees_mnemonics2strike[option_mnemonic])) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", " + Convert.ToString(-99.0) + ", '" + expiry_date + "', " + Convert.ToString(-99.0) + ", '1900-01-01 00:00:00')";

                        if (!_database_add_event.ContainsKey(option_mnemonic))
                        {
                            _database_add_event.Add(option_mnemonic, (string)add_event.Clone());
                        }
                        //command.ExecuteNonQuery();

                        //CStructureDataStore StructureDS = _clientAPIManagement.GetStructureDataStore();
                        //CStructureItem pItem1 = StructureDS.FindOrCreateItem(option_mnemonic);
                        //pItem1.RegisterInterest();
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

                //BackgroundWorker worker = new BackgroundWorker();

                if (sTe.StartsWith("EBF"))
                {
                    //worker.DoWork += (obj, e) => updateFutureDb(sTe, Bid, BidVolume, Ask, AskVolume, LastTrade, LastTradeVol, Now);
                    //worker.RunWorkerAsync();
                    updateFutureDb(sTe, Bid, BidVolume, Ask, AskVolume, LastTrade, LastTradeVol, Now);
                }
                else if (sTe.StartsWith("EBO"))
                {
                    //worker.DoWork += (obj, e) => updateOptionDb(sTe, Bid, BidVolume, Ask, AskVolume, LastTrade, LastTradeVol, Now);
                    //worker.RunWorkerAsync();
                    updateOptionDb(sTe, Bid, BidVolume, Ask, AskVolume, LastTrade, LastTradeVol, Now);
                }

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
            // load the futures
            //SQLiteCommand command = new SQLiteCommand(_db_cnn);
            MySqlCommand command = new MySqlCommand();
            command.Connection = _db_cnn;
            command.CommandText = "SELECT * FROM marketdata_future";
            //SQLiteDataReader reader = command.ExecuteReader();
            MySqlDataReader reader = command.ExecuteReader();
            _future_table.Load(reader);
            reader.Close();
            DateTime Now = DateTime.Now;

            foreach (DataRow r in _future_table.Rows)
            {
                int diff = DateTime.Compare(Now, Convert.ToDateTime(r["expiry_date"]));
                if (diff <= 0)
                {
                    // subscribe future
                    Subscribe(r["easyscreen_id"].ToString());

                    // now find all the option defs based on subscribed future
                    command.CommandText = "SELECT * FROM marketdata_optiondefinition WHERE future_id=@future_id";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@future_id", r["id"].ToString());
                    reader = command.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);
                    reader.Close();
                    _future_ids_2_optiondefs.Add(r["id"].ToString(), table);
                }
            }
            // load the options
            //command = new SQLiteCommand(_db_cnn);
            command.CommandText = "SELECT * FROM marketdata_optioncontract";
            command.Parameters.Clear();
            reader = command.ExecuteReader();
            DataTable optioncontract_table = new DataTable();
            optioncontract_table.Load(reader);
            foreach (DataRow r in optioncontract_table.Rows)
            {
                int diff = DateTime.Compare(Now, Convert.ToDateTime(r["expiry_date"]));
                if (diff <= 0)
                {
                    if (!_mnemonic2optioncontractid.ContainsKey(Convert.ToString(r["easy_screen_mnemonic"])))
                    {
                        _mnemonic2optioncontractid.Add(Convert.ToString(r["easy_screen_mnemonic"]), Convert.ToInt32(r["id"]));
                    }
                    else
                    {
                        _mnemonic2optioncontractid[Convert.ToString(r["easy_screen_mnemonic"])] = Convert.ToInt32(r["id"]);
                    }
                    Subscribe(r["easy_screen_mnemonic"].ToString());
                }
            }

            // get our option definitions we need to store all these so that we know 
            // to clean the option contract strikes at least once per day
            command.CommandText = "SELECT * FROM marketdata_optiondefinition";
            command.Parameters.Clear();
            reader = command.ExecuteReader();
            DataTable optiondefinition_table = new DataTable();
            optiondefinition_table.Load(reader);
            foreach (DataRow r in optiondefinition_table.Rows)
            {
                int diff = DateTime.Compare(Now, Convert.ToDateTime(r["expiry_date"]));
                if (diff <= 0)
                {
                    if (!_cleaned_optiondefs.ContainsKey(Convert.ToString(r["id"])))
                    {
                        _cleaned_optiondefs.Add(Convert.ToString(r["id"]), false);
                        
                        command.CommandText = "SELECT * FROM marketdata_optioncontract WHERE optiondefinition_id=@optiondefinition_id";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@optiondefinition_id", Convert.ToString(r["id"]));
                        DataTable refined_optioncontract_table = new DataTable();
                        reader = command.ExecuteReader();
                        refined_optioncontract_table.Load(reader);
                        _optiondef2optioncontracts.Add(Convert.ToString(r["id"]), refined_optioncontract_table);
                    }
                }
            }

            reader.Close();
            
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (obj, e) => update_db();
            worker.RunWorkerAsync();
            

        }


        private void update_db()
        {

            MySqlCommand command = new MySqlCommand();
            command.Connection = _db_cnn;
            // add stuff to db
            while (true)
            {
                System.Threading.Thread.Sleep(3000);
                 if (!_database_dict_locked)
                {
                    _database_dict_locked = true;

                    if (_database_add_event.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> entry in _database_add_event)
                        {
                            command.CommandText = entry.Value;
                            int rows_effected = command.ExecuteNonQuery();
                        }
            
                        // update the _mnemonic2optioncontractid map
                        command.CommandText = "SELECT * FROM marketdata_optioncontract";
                        command.Parameters.Clear();
                        MySqlDataReader reader = command.ExecuteReader();
                        //reader = command.ExecuteReader();
                        DataTable optioncontract_table = new DataTable();
                        optioncontract_table.Load(reader);
            
                        foreach (DataRow r in optioncontract_table.Rows)
                        {
                            if (!_mnemonic2optioncontractid.ContainsKey(Convert.ToString(r["easy_screen_mnemonic"])))
                            {
                                _mnemonic2optioncontractid.Add(Convert.ToString(r["easy_screen_mnemonic"]), Convert.ToInt32(r["id"]));
                            }
                            else
                            {
                                _mnemonic2optioncontractid[Convert.ToString(r["easy_screen_mnemonic"])] = Convert.ToInt32(r["id"]);
                            }
                        }
    
                        foreach (KeyValuePair<string, DataTable> entry in _future_ids_2_optiondefs)
                        {
                            DataTable optiondefs = entry.Value;
                            foreach (DataRow row in optiondefs.Rows)
                            {
                                command.Parameters.Clear();
                                command.CommandText = "SELECT * FROM marketdata_optioncontract WHERE optiondefinition_id=@optiondefinition_id";
                                command.Parameters.AddWithValue("@optiondefinition_id", Convert.ToString(row["id"]));
                                DataTable refined_optioncontract_table = new DataTable();
                                reader = command.ExecuteReader();
                                refined_optioncontract_table.Load(reader);
                                
                                // now we need to check the refined_optioncontract_table for double ups (EES mnemonics/strikes), 
                                // if there are double ups we need to remove them from the refined_optioncontract_table and from the 
                                // database itself.
                                HashSet<string> mnemonic_set = new HashSet<string>();
                                List<int> delete_positions = new List<int>();
                                List<int> delete_table_ids = new List<int>();
                                
                                int pos = 0;
                                foreach (DataRow check_row in refined_optioncontract_table.Rows)
                                {
                                    if (mnemonic_set.Contains(Convert.ToString(check_row["easy_screen_mnemonic"])))
                                    {
                                        delete_positions.Add(pos);
                                        delete_table_ids.Add(Convert.ToInt32(check_row["id"]));
                                    }
                                    else
                                    {
                                        mnemonic_set.Add(Convert.ToString(check_row["easy_screen_mnemonic"]));
                                    }
                                    pos += 1;
                                }
                                if (delete_positions.Count > 0)
                                {
                                    delete_positions.Reverse();
                                    delete_table_ids.Reverse();
                                    for (int i = 0; i < delete_positions.Count; i++)
                                    {
                                        refined_optioncontract_table.Rows[delete_positions[i]].Delete();
                                        command.Parameters.Clear();
                                        command.CommandText = "DELETE FROM marketdata_optioncontract WHERE id=@id";
                                        command.Parameters.AddWithValue("@id", Convert.ToString(delete_table_ids[i]));
                                        command.ExecuteNonQuery();
                                    }
                                }

                                if (!_optiondef2optioncontracts.ContainsKey(Convert.ToString(row["id"])))
                                {
                                    _optiondef2optioncontracts.Add(Convert.ToString(row["id"]), refined_optioncontract_table);
                                }
                                else
                                {
                                    _optiondef2optioncontracts[Convert.ToString(row["id"])] = refined_optioncontract_table;
                                }
                            }
                        }
                        
            
                        // now subscribe, only subscribe after the dbs been modified and the _mnemonic2optioncontractid has been updated. I was getting 
                        // a key error before
                        foreach (KeyValuePair<string, string> entry in _database_add_event)
                        {
                            Subscribe(entry.Key);
                        }
    
                        _database_add_event.Clear();
                    }

                    foreach (KeyValuePair<string, string> entry in _database_events)
                    {
                        command.CommandText = entry.Value;
                        command.Connection = _db_cnn;
                        command.ExecuteNonQuery();
                    }

                    _database_events.Clear();
                    _database_dict_locked = false;
                }

            }

            
        }


        private void update_db_events(string id, string command)
        {
            if (!_database_dict_locked)
            {
                _database_dict_locked = true;
                if (!_database_events.ContainsKey(id))
                {
                    _database_events.Add(id, command);
                }
                else
                {
                    _database_events[id] = command; 
                }
                _database_dict_locked = false;
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
            if (_subscribed == false)
            {
                _AutoSubscribe();
            }
            _subscribed = true;
        }

    }
}
