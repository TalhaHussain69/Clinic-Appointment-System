using System;
using System.Collections.Generic;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class PaymentForm : Form
    {
        private IPaymentService _paymentService;
        private IAppointmentService _appointmentService;
        private Payment _payment;
        private FormMode _mode;
        private List<Appointment> _appointments;

        public PaymentForm(IPaymentService paymentService, IAppointmentService appointmentService, Payment payment, FormMode mode)
        {
            _paymentService = paymentService;
            _appointmentService = appointmentService;
            _payment = payment;
            _mode = mode;
            InitializeComponent();
            LoadAppointments();

            switch (_mode)
            {
                case FormMode.Add:
                    lblTitle.Text = "Add Payment";
                    btnSave.Text = "Save";
                    break;
                case FormMode.Edit:
                    lblTitle.Text = "Edit Payment";
                    btnSave.Text = "Update";
                    FillForm();
                    break;
                case FormMode.View:
                    lblTitle.Text = "View Payment";
                    btnSave.Visible = false;
                    cmbAppointment.Enabled = false;
                    txtAmount.ReadOnly = true;
                    cmbMethod.Enabled = false;
                    cmbStatus.Enabled = false;
                    dtpDate.Enabled = false;
                    txtNotes.ReadOnly = true;
                    FillForm();
                    break;
            }
        }

        private void LoadAppointments()
        {
            _appointments = _appointmentService.GetAll();
            cmbAppointment.Items.Clear();
            foreach (var a in _appointments)
                cmbAppointment.Items.Add(a.PatientName + " — " + a.DoctorName + " (" + a.AppDate.ToShortDateString() + ")");
            if (cmbAppointment.Items.Count > 0)
                cmbAppointment.SelectedIndex = 0;
        }

        private void FillForm()
        {
            for (int i = 0; i < _appointments.Count; i++)
                if (_appointments[i].Id == _payment.AppointmentId) { cmbAppointment.SelectedIndex = i; break; }

            txtAmount.Text = _payment.Amount.ToString();
            cmbMethod.SelectedItem = _payment.PaymentMethod;
            cmbStatus.SelectedItem = _payment.Status;
            dtpDate.Value = _payment.PaymentDate;
            txtNotes.Text = _payment.Notes ?? "";
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (cmbAppointment.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an Appointment!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Please enter a valid Amount!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_mode == FormMode.Edit)
            {
                _payment.AppointmentId = _appointments[cmbAppointment.SelectedIndex].Id;
                _payment.Amount = amount;
                _payment.PaymentMethod = cmbMethod.SelectedItem.ToString();
                _payment.Status = cmbStatus.SelectedItem.ToString();
                _payment.PaymentDate = dtpDate.Value;
                _payment.Notes = txtNotes.Text.Trim();
                _paymentService.Update(_payment);
            }
            else
            {
                Payment p = new Payment();
                p.AppointmentId = _appointments[cmbAppointment.SelectedIndex].Id;
                p.Amount = amount;
                p.PaymentMethod = cmbMethod.SelectedItem.ToString();
                p.Status = cmbStatus.SelectedItem.ToString();
                p.PaymentDate = dtpDate.Value;
                p.Notes = txtNotes.Text.Trim();
                _paymentService.Add(p);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}