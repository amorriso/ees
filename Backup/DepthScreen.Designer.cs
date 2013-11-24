namespace EESTesterClientAPI
{
    partial class DepthScreen
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
            this.dgvDepth = new System.Windows.Forms.DataGridView();
            this.BID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRICE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SELL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDepth
            // 
            this.dgvDepth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDepth.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BID,
            this.PRICE,
            this.SELL});
            this.dgvDepth.Location = new System.Drawing.Point(12, 13);
            this.dgvDepth.Name = "dgvDepth";
            this.dgvDepth.Size = new System.Drawing.Size(363, 523);
            this.dgvDepth.TabIndex = 0;
            // 
            // BID
            // 
            this.BID.HeaderText = "BID";
            this.BID.Name = "BID";
            // 
            // PRICE
            // 
            this.PRICE.HeaderText = "Price";
            this.PRICE.Name = "PRICE";
            // 
            // SELL
            // 
            this.SELL.HeaderText = "SELL";
            this.SELL.Name = "SELL";
            // 
            // DepthScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 548);
            this.Controls.Add(this.dgvDepth);
            this.Name = "DepthScreen";
            this.Text = "Depth";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDepth;
        private System.Windows.Forms.DataGridViewTextBoxColumn BID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRICE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SELL;
    }
}