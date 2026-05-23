namespace App.WindowsApp
{
    partial class mainform
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
            this.pnlNav = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnPatients = new System.Windows.Forms.Button();
            this.btnDoctors = new System.Windows.Forms.Button();
            this.btnAppointments = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlNav.SuspendLayout();
            this.SuspendLayout();

            // pnlNav
            this.pnlNav.BackColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.pnlNav.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNav.Name = "pnlNav";
            this.pnlNav.Width = 200;
            this.pnlNav.Controls.Add(this.lblAppName);
            this.pnlNav.Controls.Add(this.btnDashboard);
            this.pnlNav.Controls.Add(this.btnPatients);
            this.pnlNav.Controls.Add(this.btnDoctors);
            this.pnlNav.Controls.Add(this.btnAppointments);

            // lblAppName
            this.lblAppName.AutoSize = false;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.Location = new System.Drawing.Point(0, 0);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(200, 60);
            this.lblAppName.Text = "Clinic System";
            this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnDashboard
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDashboard.Location = new System.Drawing.Point(0, 70);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(200, 42);
            this.btnDashboard.Text = "  Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.BackColor = System.Drawing.Color.Transparent;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);

            // btnPatients
            this.btnPatients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPatients.FlatAppearance.BorderSize = 0;
            this.btnPatients.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.btnPatients.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPatients.Location = new System.Drawing.Point(0, 112);
            this.btnPatients.Name = "btnPatients";
            this.btnPatients.Size = new System.Drawing.Size(200, 42);
            this.btnPatients.Text = "  Patients";
            this.btnPatients.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPatients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPatients.BackColor = System.Drawing.Color.Transparent;
            this.btnPatients.Click += new System.EventHandler(this.btnPatients_Click);

            // btnDoctors
            this.btnDoctors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDoctors.FlatAppearance.BorderSize = 0;
            this.btnDoctors.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.btnDoctors.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDoctors.Location = new System.Drawing.Point(0, 154);
            this.btnDoctors.Name = "btnDoctors";
            this.btnDoctors.Size = new System.Drawing.Size(200, 42);
            this.btnDoctors.Text = "  Doctors";
            this.btnDoctors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDoctors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDoctors.BackColor = System.Drawing.Color.Transparent;
            this.btnDoctors.Click += new System.EventHandler(this.btnDoctors_Click);

            // btnAppointments
            this.btnAppointments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppointments.FlatAppearance.BorderSize = 0;
            this.btnAppointments.ForeColor = System.Drawing.Color.FromArgb(180, 180, 190);
            this.btnAppointments.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAppointments.Location = new System.Drawing.Point(0, 196);
            this.btnAppointments.Name = "btnAppointments";
            this.btnAppointments.Size = new System.Drawing.Size(200, 42);
            this.btnAppointments.Text = "  Appointments";
            this.btnAppointments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppointments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAppointments.BackColor = System.Drawing.Color.Transparent;
            this.btnAppointments.Click += new System.EventHandler(this.btnAppointments_Click);

            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Name = "pnlContent";

            // mainform
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 680);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clinic Appointment System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlNav);
            this.pnlNav.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlNav;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnPatients;
        private System.Windows.Forms.Button btnDoctors;
        private System.Windows.Forms.Button btnAppointments;
        private System.Windows.Forms.Panel pnlContent;
    }
}