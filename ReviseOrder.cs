using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EESTesterClientAPI
{
    public partial class ReviseOrder : Form
    {

        #region global variables
        string Price = null;
        string Qty = null;
        string Temnemonic = null;
        string Account = null;
        string orderid = null;
        Form1 jhform = null;
        #endregion

        public ReviseOrder(string strPrice,string strQty,string strTemnemonic,string strAccount,string sorderID,Form1 form)
        {
            InitializeComponent();
            ValueFromParent = strPrice;
            ValueFromParent2 = strQty;
            Temnemonic = strTemnemonic;
            Account = strAccount;
            orderid = sorderID;

            jhform = form;
        }
        public string ValueFromParent
        {
            set
            {
                this.txtPrice.Text = value;

            }
        }
        public string ValueFromParent2
        {
            set
            {
                this.txtQty.Text = value;

            }
        }

        private void btnRevise_Click(object sender, EventArgs e)
        {
            double dprice = Convert.ToDouble(txtPrice.Text); 
            int iqty = Convert.ToInt32(txtQty.Text);
            
            this.Close();
        }
    }
}
