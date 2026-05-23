using System;
using System.Drawing;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class DoctorView : UserControl
    {
        private IDoctorService _service;

        public DoctorView(IDoctorService service)
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
            dgv.Columns.Add("Specialization", "Specialization");
            dgv.Columns.Add("Phone", "Phone");
            dgv.Columns.Add("Email", "Email");
            dgv.Columns.Add("Fee", "Fee (Rs.)");
            dgv.Columns.Add("Status", "Status");
        }

        private void LoadData()
        {
            dgv.Rows.Clear();
            var doctors = _service.GetAll();

            foreach (var d in doctors)
            {
                dgv.Rows.Add(
                    d.Id,
                    d.Name,
                    d.Specialization,
                    d.Phone,
                    d.Email ?? "-",
                    d.Fee.ToString("N0"),
                    d.Status.ToString()
                );
            }
        }

        private void SearchData()
        {
            DoctorStatus? status = null;
            if (cmbStatus.SelectedIndex == 1) status = DoctorStatus.Active;
            if (cmbStatus.SelectedIndex == 2) status = DoctorStatus.OnLeave;
            if (cmbStatus.SelectedIndex == 3) status = DoctorStatus.Inactive;

            dgv.Rows.Clear();
            var results = _service.Search(txtSearch.Text, status);

            foreach (var d in results)
            {
                dgv.Rows.Add(
                    d.Id,
                    d.Name,
                    d.Specialization,
                    d.Phone,
                    d.Email ?? "-",
                    d.Fee.ToString("N0"),
                    d.Status.ToString()
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new DoctorForm(_service, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a doctor to edit.");
                return;
            }
            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var doctor = _service.GetById(id);
            var form = new DoctorForm(_service, doctor);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a doctor to delete.");
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

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchData();
        }
    }
}