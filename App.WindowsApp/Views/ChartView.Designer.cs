namespace App.WindowsApp
{
    partial class ChartView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblChart1 = new System.Windows.Forms.Label();
            this.lblChart2 = new System.Windows.Forms.Label();
            this.pnlChart1 = new System.Windows.Forms.Panel();
            this.pnlChart2 = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Charts & Analytics";

            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.Location = new System.Drawing.Point(20, 60);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 32);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.lblChart1.AutoSize = true;
            this.lblChart1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblChart1.ForeColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.lblChart1.Location = new System.Drawing.Point(20, 108);
            this.lblChart1.Name = "lblChart1";
            this.lblChart1.Text = "Appointments by Status";

            this.pnlChart1.BackColor = System.Drawing.Color.White;
            this.pnlChart1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChart1.Location = new System.Drawing.Point(20, 132);
            this.pnlChart1.Name = "pnlChart1";
            this.pnlChart1.Size = new System.Drawing.Size(460, 340);

            this.lblChart2.AutoSize = true;
            this.lblChart2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblChart2.ForeColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.lblChart2.Location = new System.Drawing.Point(510, 108);
            this.lblChart2.Name = "lblChart2";
            this.lblChart2.Text = "Patients by Gender";

            this.pnlChart2.BackColor = System.Drawing.Color.White;
            this.pnlChart2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChart2.Location = new System.Drawing.Point(510, 132);
            this.pnlChart2.Name = "pnlChart2";
            this.pnlChart2.Size = new System.Drawing.Size(460, 340);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.Name = "ChartView";
            this.Size = new System.Drawing.Size(1000, 510);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblChart1);
            this.Controls.Add(this.pnlChart1);
            this.Controls.Add(this.lblChart2);
            this.Controls.Add(this.pnlChart2);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblChart1;
        private System.Windows.Forms.Label lblChart2;
        private System.Windows.Forms.Panel pnlChart1;
        private System.Windows.Forms.Panel pnlChart2;
    }
}