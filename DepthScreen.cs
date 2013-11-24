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
    public partial class DepthScreen : Form
    {
        Form1 myform = null;
        string _te = null;

        public DepthScreen(string te)
        {
            InitializeComponent();
            dgvDepth.Rows.Add(21);
            _te = te;
        }

        public void update(double price, double volume, int level, string sTe)
        {
            if (sTe == _te)
            {
                if (level < 10)
                {
                    level = level + 9;
                    dgvDepth.Rows[level].Cells["PRICE"].Value = price;
                    dgvDepth.Rows[level].Cells["BID"].Value = volume;
                }
                else { }
            }
        }
        //OVERLOAD FOR SELLS
        public void update(double price, double volume, int level, string sell, string sTe)
        {
            if (sTe == _te)
            {
                if (level < 10)
                {
                    level = 10 - level;
                    dgvDepth.Rows[level].Cells["PRICE"].Value = price;
                    dgvDepth.Rows[level].Cells["SELL"].Value = volume;
                }
                else { }
            }
        }

    }
}

