using System;
using System.Collections.Generic;
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
        private BindingSource _bindingSource = new BindingSource();
        private List<Appointment> _appointments = new List<Appointment>();

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
            dgv.AutoGenerateColumns = false;

            dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", HeaderText = "ID" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "PatientName", DataPropertyName = "PatientName", HeaderText = "Patient" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "DoctorName", DataPropertyName = "DoctorName", HeaderText = "Doctor" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "AppDate", DataPropertyName = "AppDate", HeaderText = "Date" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "AppTime", DataPropertyName = "AppTime", HeaderText = "Time" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", DataPropertyName = "Type", HeaderText = "Type" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Status" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fee", DataPropertyName = "Fee", HeaderText = "Fee (Rs.)" });

            dgv.DataSource = _bindingSource;
        }

        public void LoadData()
        {
            _appointments = _appointmentService.GetAll();
            _bindingSource.DataSource = null;
            _bindingSource.DataSource = _appointments;
            dgv.Refresh();

            var main = this.FindForm() as mainform;
            if (main != null) main.RefreshCharts();

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

            _appointments = _appointmentService.Search(txtSearch.Text, status, type);
            _bindingSource.DataSource = null;
            _bindingSource.DataSource = _appointments;
            dgv.Refresh();
        }

        private Appointment GetSelectedAppointment()
        {
            if (dgv.SelectedRows.Count == 0) return null;
            return dgv.SelectedRows[0].DataBoundItem as Appointment;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new AppointmentForm(_appointmentService, _patientService, _doctorService, null, FormMode.Add);
            if (form.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var appointment = GetSelectedAppointment();
            if (appointment == null) { MessageBox.Show("Please select an appointment to edit."); return; }
            var form = new AppointmentForm(_appointmentService, _patientService, _doctorService, appointment, FormMode.Edit);
            if (form.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var appointment = GetSelectedAppointment();
            if (appointment == null) { MessageBox.Show("Please select an appointment to view."); return; }
            new AppointmentForm(_appointmentService, _patientService, _doctorService, appointment, FormMode.View).ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var appointment = GetSelectedAppointment();
            if (appointment == null) { MessageBox.Show("Please select an appointment to delete."); return; }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete this appointment?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                _appointmentService.Delete(appointment.Id);
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadData();
        private void txtSearch_TextChanged(object sender, EventArgs e) => SearchData();
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e) => SearchData();
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e) => SearchData();
    }
}