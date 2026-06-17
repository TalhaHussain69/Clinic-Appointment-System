using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.WindowsApp
{
    public partial class PaymentView : UserControl
    {
        private IPaymentService _paymentService;
        private IAppointmentService _appointmentService;

        public PaymentView(IPaymentService paymentService, IAppointmentService appointmentService)
        {
            _paymentService = paymentService;
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
            dgv.Columns.Add("Amount", "Amount (Rs.)");
            dgv.Columns.Add("PaymentMethod", "Method");
            dgv.Columns.Add("PaymentDate", "Date");
            dgv.Columns.Add("Status", "Status");
        }

        private void LoadData()
        {
            dgv.Rows.Clear();
            var payments = _paymentService.GetAll();

            foreach (var p in payments)
            {
                dgv.Rows.Add(
                    p.Id,
                    p.PatientName,
                    p.DoctorName,
                    p.Amount.ToString("N0"),
                    p.PaymentMethod,
                    p.PaymentDate.ToShortDateString(),
                    p.Status
                );
            }
        }

        private void SearchData()
        {
            string status = cmbStatus.SelectedIndex == 0 ? "" : cmbStatus.SelectedItem.ToString();
            dgv.Rows.Clear();
            var results = _paymentService.Search(txtSearch.Text, status);

            foreach (var p in results)
            {
                dgv.Rows.Add(
                    p.Id,
                    p.PatientName,
                    p.DoctorName,
                    p.Amount.ToString("N0"),
                    p.PaymentMethod,
                    p.PaymentDate.ToShortDateString(),
                    p.Status
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new PaymentForm(_paymentService, _appointmentService, null, FormMode.Add);
            if (form.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Please select a payment to edit."); return; }
            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var payment = _paymentService.GetById(id);
            var form = new PaymentForm(_paymentService, _appointmentService, payment, FormMode.Edit);
            if (form.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Please select a payment to view."); return; }
            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var payment = _paymentService.GetById(id);
            new PaymentForm(_paymentService, _appointmentService, payment, FormMode.View).ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Please select a payment to delete."); return; }
            string id = dgv.SelectedRows[0].Cells["Id"].Value.ToString();
            var confirm = MessageBox.Show("Are you sure you want to delete this payment?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes) { _paymentService.Delete(id); LoadData(); }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadData();
        private void txtSearch_TextChanged(object sender, EventArgs e) => SearchData();
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e) => SearchData();
    }
}