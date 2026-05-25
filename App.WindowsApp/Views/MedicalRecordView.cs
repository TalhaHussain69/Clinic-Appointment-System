using System;
using System.Drawing;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;

namespace App.WindowsApp
{
    public partial class MedicalRecordView : UserControl
    {
        private IMedicalRecordService _recordService;
        private IPatientService _patientService;
        private IDoctorService _doctorService;
        private IAppointmentService _appointmentService;

        public MedicalRecordView(IMedicalRecordService recordService, IPatientService patientService, IDoctorService doctorService, IAppointmentService appointmentService)
        {
            _recordService = recordService;
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
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
            dgv.Columns.Add("Diagnosis", "Diagnosis");
            dgv.Columns.Add("Prescription", "Prescription");
            dgv.Columns.Add("RecordDate", "Date");
        }

        private void LoadData()
        {
            dgv.Rows.Clear();
            var records = _recordService.GetAll();

            foreach (var r in records)
            {
                dgv.Rows.Add(
                    r.Id,
                    r.PatientName,
                    r.DoctorName,
                    r.Diagnosis,
                    r.Prescription ?? "-",
                    r.RecordDate.ToShortDateString()
                );
            }
        }

        private void SearchData()
        {
            dgv.Rows.Clear();
            var results = _recordService.Search(txtSearch.Text);

            foreach (var r in results)
            {
                dgv.Rows.Add(
                    r.Id,
                    r.PatientName,
                    r.DoctorName,
                    r.Diagnosis,
                    r.Prescription ?? "-",
                    r.RecordDate.ToShortDateString()
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new MedicalRecordForm(_recordService, _patientService, _doctorService, _appointmentService, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to edit.");
                return;
            }
            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var record = _recordService.GetById(id);
            var form = new MedicalRecordForm(_recordService, _patientService, _doctorService, _appointmentService, record);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var confirm = MessageBox.Show(
                "Are you sure you want to delete this record?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                _recordService.Delete(id);
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadData();
        private void txtSearch_TextChanged(object sender, EventArgs e) => SearchData();
    }
}