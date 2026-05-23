using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Services;

namespace App.WindowsApp
{
    public partial class mainform : Form
    {
        private string _connectionString;
        private IDoctorService _doctorService;
        private IPatientService _patientService;
        private IAppointmentService _appointmentService;
        private Button _activeBtn;

        public mainform()
        {
            InitializeComponent();

            _connectionString = ConfigurationManager
                .ConnectionStrings["ClinicDB"].ConnectionString;

            _doctorService = new DbDoctorService(_connectionString);
            _patientService = new DbPatientService(_connectionString);
            _appointmentService = new DbAppointmentService(_connectionString);

            SetActiveButton(btnDashboard);
            ShowDashboard();
        }

        private void ShowView(UserControl view)
        {
            pnlContent.Controls.Clear();
            view.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(view);
        }

        private void SetActiveButton(Button btn)
        {
            if (_activeBtn != null)
            {
                _activeBtn.BackColor = Color.Transparent;
                _activeBtn.ForeColor = Color.FromArgb(180, 180, 190);
            }
            btn.BackColor = Color.FromArgb(50, 50, 65);
            btn.ForeColor = Color.White;
            _activeBtn = btn;
        }

        private void ShowDashboard()
        {
            pnlContent.Controls.Clear();

            // Stats counts
            int totalPatients = 0;
            int totalDoctors = 0;
            int totalAppointments = 0;
            int pendingCount = 0;

            try
            {
                var patients = _patientService.GetAll();
                var doctors = _doctorService.GetAll();
                var appointments = _appointmentService.GetAll();

                totalPatients = patients.Count;
                totalDoctors = doctors.Count;
                totalAppointments = appointments.Count;

                foreach (var a in appointments)
                    if (a.Status == App.core.Utilities.AppointmentStatus.Pending)
                        pendingCount++;
            }
            catch { }

            // Title
            Label lblTitle = new Label
            {
                Text = "Dashboard - Clinic Appointment System",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Stats Panel
            Panel pnlStats = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(900, 100),
                BackColor = Color.Transparent
            };

            pnlStats.Controls.Add(MakeStatCard("Total Patients", totalPatients.ToString(), 0));
            pnlStats.Controls.Add(MakeStatCard("Total Appointments", totalAppointments.ToString(), 230));
            pnlStats.Controls.Add(MakeStatCard("Doctors Available", totalDoctors.ToString(), 460));
            pnlStats.Controls.Add(MakeStatCard("Pending", pendingCount.ToString(), 690));

            // Recent Appointments Label
            Label lblRecent = new Label
            {
                Text = "Recent Appointments",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                AutoSize = true,
                Location = new Point(20, 175)
            };

            // DataGridView
            DataGridView dgv = new DataGridView
            {
                Location = new Point(20, 205),
                Size = new Size(950, 380),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(229, 229, 229)
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 40);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 252);

            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("PatientName", "Patient");
            dgv.Columns.Add("DoctorName", "Doctor");
            dgv.Columns.Add("AppDate", "Date");
            dgv.Columns.Add("AppTime", "Time");
            dgv.Columns.Add("Type", "Type");
            dgv.Columns.Add("Status", "Status");
            dgv.Columns.Add("Fee", "Fee (Rs.)");

            try
            {
                var appointments = _appointmentService.GetAll();
                foreach (var a in appointments)
                {
                    dgv.Rows.Add(
                        a.Id,
                        a.PatientName,
                        a.DoctorName,
                        a.AppDate.ToShortDateString(),
                        a.AppTime,
                        a.Type.ToString(),
                        a.Status.ToString(),
                        a.Fee.ToString("N0")
                    );
                }
            }
            catch { }

            pnlContent.Controls.Add(lblTitle);
            pnlContent.Controls.Add(pnlStats);
            pnlContent.Controls.Add(lblRecent);
            pnlContent.Controls.Add(dgv);
        }

        private Panel MakeStatCard(string title, string value, int x)
        {
            Panel card = new Panel
            {
                Location = new Point(x, 0),
                Size = new Size(210, 90),
                BackColor = Color.White
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                AutoSize = false,
                Size = new Size(210, 45),
                Location = new Point(15, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(120, 120, 130),
                AutoSize = false,
                Size = new Size(210, 25),
                Location = new Point(15, 55),
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            return card;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            ShowDashboard();
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnPatients);
            ShowView(new PatientView(_patientService));
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDoctors);
            ShowView(new DoctorView(_doctorService));
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAppointments);
            ShowView(new AppointmentView(_appointmentService, _patientService, _doctorService));
        }
    }
}