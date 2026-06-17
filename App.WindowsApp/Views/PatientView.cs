using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class PatientView : UserControl
    {
        private IPatientService _service;
        private BindingSource _bindingSource = new BindingSource();
        private List<Patient> _patients = new List<Patient>();

        public PatientView(IPatientService service)
        {
            _service = service;
            InitializeComponent();
            SetupGrid();
            LoadData();
        }

        private void SetupGrid()
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 40);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 252);
            dgv.AutoGenerateColumns = false;

            dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", HeaderText = "ID" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", DataPropertyName = "Name", HeaderText = "Name" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Age", DataPropertyName = "Age", HeaderText = "Age" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Gender", DataPropertyName = "Gender", HeaderText = "Gender" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Phone" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "BloodGroup", DataPropertyName = "BloodGroup", HeaderText = "Blood Group" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RegisteredOn", DataPropertyName = "RegisteredOn", HeaderText = "Registered On" });

            dgv.DataSource = _bindingSource;
        }

        private void LoadData()
        {
            _patients = _service.GetAll();
            _bindingSource.DataSource = null;
            _bindingSource.DataSource = _patients;
            dgv.Refresh();
        }

        private void SearchData()
        {
            Gender? gender = null;
            if (cmbGender.SelectedIndex == 1) gender = Gender.Male;
            if (cmbGender.SelectedIndex == 2) gender = Gender.Female;

            _patients = _service.Search(txtSearch.Text, gender);
            _bindingSource.DataSource = null;
            _bindingSource.DataSource = _patients;
            dgv.Refresh();
        }

        private Patient GetSelectedPatient()
        {
            if (dgv.SelectedRows.Count == 0) return null;
            return dgv.SelectedRows[0].DataBoundItem as Patient;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new PatientForm(_service, null, FormMode.Add);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var patient = GetSelectedPatient();
            if (patient == null)
            {
                MessageBox.Show("Please select a patient to edit.");
                return;
            }
            var form = new PatientForm(_service, patient, FormMode.Edit);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var patient = GetSelectedPatient();
            if (patient == null)
            {
                MessageBox.Show("Please select a patient to view.");
                return;
            }
            var form = new PatientForm(_service, patient, FormMode.View);
            form.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var patient = GetSelectedPatient();
            if (patient == null)
            {
                MessageBox.Show("Please select a patient to delete.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete {patient.Name}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                _service.Delete(patient.Id);
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadData();
        private void txtSearch_TextChanged(object sender, EventArgs e) => SearchData();
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) => SearchData();
    }
}