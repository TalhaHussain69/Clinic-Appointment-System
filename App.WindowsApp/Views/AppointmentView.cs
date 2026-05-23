using System;
using System.Drawing;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class AppointmentView : UserControl
    {
        private IAppointmentService _appointmentService;
        private IPatientService _patientService;
        private IDoctorService _doctorService;

        public AppointmentView(IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
            InitializeComponent();
            SetupGrid();
            LoadData();
        }

        private void SetupGrid()
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 30, 40);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 248, 252);

            dgv.Columns.Clear();
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("PatientName", "Patient");
            dgv.Columns.Add("DoctorName", "Doctor");
            dgv.Columns.Add("AppDate", "Date");
            dgv.Columns.Add("AppTime", "Time");
            dgv.Columns.Add("Type", "Type");
            dgv.Columns.Add("Status", "Status");
            dgv.Columns.Add("Fee", "Fee (Rs.)");
        }

        private void LoadData()
        {
            dgv.Rows.Clear();
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

        private void SearchData()
        {
            AppointmentStatus? status = null;
            AppointmentType? type = null;

            if (cmbStatus.SelectedIndex == 1) status = AppointmentStatus.Scheduled;
            if (cmbStatus.SelectedIndex == 2) status = AppointmentStatus.Confirmed;
            if (cmbStatus.SelectedIndex == 3) status = AppointmentStatus.Pending;
            if (cmbStatus.SelectedIndex == 4) status = AppointmentStatus.Cancelled;

            if (cmbType.SelectedIndex == 1) type = AppointmentType.General;
            if (cmbType.SelectedIndex == 2) type = AppointmentType.Dental;
            if (cmbType.SelectedIndex == 3) type = AppointmentType.FollowUp;
            if (cmbType.SelectedIndex == 4) type = AppointmentType.Emergency;

            dgv.Rows.Clear();
            var results = _appointmentService.Search(txtSearch.Text, status, type);

            foreach (var a in results)
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new AppointmentForm(_appointmentService, _patientService, _doctorService, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to edit.");
                return;
            }
            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var appointment = _appointmentService.GetById(id);
            var form = new AppointmentForm(_appointmentService, _patientService, _doctorService, appointment);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            string patient = dgv.SelectedRows[0].Cells["PatientName"].Value.ToString();

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete appointment for {patient}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                _appointmentService.Delete(id);
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchData();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchData();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchData();
        }
    }
}