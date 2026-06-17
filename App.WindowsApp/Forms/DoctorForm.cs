using System;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class DoctorForm : Form
    {
        private IDoctorService _service;
        private Doctor _doctor;
        private FormMode _mode;

        public DoctorForm(IDoctorService service, Doctor doctor, FormMode mode)
        {
            _service = service;
            _doctor = doctor;
            _mode = mode;
            InitializeComponent();

            switch (_mode)
            {
                case FormMode.Add:
                    lblTitle.Text = "Add Doctor";
                    btnSave.Text = "Save";
                    break;
                case FormMode.Edit:
                    lblTitle.Text = "Edit Doctor";
                    btnSave.Text = "Update";
                    FillForm();
                    break;
                case FormMode.View:
                    lblTitle.Text = "View Doctor";
                    btnSave.Visible = false;
                    txtName.ReadOnly = true;
                    txtSpec.ReadOnly = true;
                    txtPhone.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                    txtFee.ReadOnly = true;
                    cmbStatus.Enabled = false;
                    FillForm();
                    break;
            }
        }

        private void FillForm()
        {
            txtName.Text = _doctor.Name;
            txtSpec.Text = _doctor.Specialization;
            txtPhone.Text = _doctor.Phone;
            txtEmail.Text = _doctor.Email ?? "";
            txtFee.Text = _doctor.Fee.ToString();
            cmbStatus.SelectedItem = _doctor.Status.ToString();
        }


        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Full Name is required!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSpec.Text))
            {
                MessageBox.Show("Specialization is required!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSpec.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone is required!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (!decimal.TryParse(txtFee.Text, out decimal fee))
            {
                MessageBox.Show("Please enter a valid Fee!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFee.Focus();
                return;
            }

            DoctorStatus status = (DoctorStatus)System.Enum.Parse(
                typeof(DoctorStatus), cmbStatus.SelectedItem.ToString());

            if (_mode == FormMode.Edit)
            {
                _doctor.Name = txtName.Text.Trim();
                _doctor.Specialization = txtSpec.Text.Trim();
                _doctor.Phone = txtPhone.Text.Trim();
                _doctor.Email = txtEmail.Text.Trim();
                _doctor.Fee = fee;
                _doctor.Status = status;
                _service.Update(_doctor);
            }
            else
            {
                Doctor d = new Doctor();
                d.Name = txtName.Text.Trim();
                d.Specialization = txtSpec.Text.Trim();
                d.Phone = txtPhone.Text.Trim();
                d.Email = txtEmail.Text.Trim();
                d.Fee = fee;
                d.Status = status;
                _service.Add(d);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}