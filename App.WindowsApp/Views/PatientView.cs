using System;
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

        public PatientView(IPatientService service)
        {
            _service = service;
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
            dgv.Columns.Add("Name", "Name");
            dgv.Columns.Add("Age", "Age");
            dgv.Columns.Add("Gender", "Gender");
            dgv.Columns.Add("Phone", "Phone");
            dgv.Columns.Add("BloodGroup", "Blood Group");
            dgv.Columns.Add("RegisteredOn", "Registered On");
        }

        private void LoadData()
        {
            dgv.Rows.Clear();
            var patients = _service.GetAll();

            foreach (var p in patients)
            {
                dgv.Rows.Add(
                    p.Id,
                    p.Name,
                    p.Age,
                    p.Gender.ToString(),
                    p.Phone,
                    p.BloodGroup ?? "-",
                    p.RegisteredOn.ToShortDateString()
                );
            }
        }

        private void SearchData()
        {
            Gender? gender = null;
            if (cmbGender.SelectedIndex == 1) gender = Gender.Male;
            if (cmbGender.SelectedIndex == 2) gender = Gender.Female;

            dgv.Rows.Clear();
            var results = _service.Search(txtSearch.Text, gender);

            foreach (var p in results)
            {
                dgv.Rows.Add(
                    p.Id,
                    p.Name,
                    p.Age,
                    p.Gender.ToString(),
                    p.Phone,
                    p.BloodGroup ?? "-",
                    p.RegisteredOn.ToShortDateString()
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new PatientForm(_service, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a patient to edit.");
                return;
            }
            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var patient = _service.GetById(id);
            var form = new PatientForm(_service, patient);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a patient to delete.");
                return;
            }

            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            string name = dgv.SelectedRows[0].Cells["Name"].Value.ToString();

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete {name}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                _service.Delete(id);
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

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchData();
        }
    }
}