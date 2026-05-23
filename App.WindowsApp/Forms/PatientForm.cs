using System;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class PatientForm : Form
    {
        private IPatientService _service;
        private Patient _patient;
        private bool _isEdit;

        public PatientForm(IPatientService service, Patient patient)
        {
            _service = service;
            _patient = patient;
            _isEdit = patient != null;
            InitializeComponent();

            if (_isEdit)
            {
                lblTitle.Text = "Edit Patient";
                btnSave.Text = "Update";
                FillForm();
            }
        }

        private void FillForm()
        {
            txtName.Text = _patient.Name;
            txtAge.Text = _patient.Age.ToString();
            cmbGender.SelectedItem = _patient.Gender.ToString();
            txtPhone.Text = _patient.Phone;
            txtCnic.Text = _patient.CNIC ?? "";
            txtAddress.Text = _patient.Address ?? "";
            cmbBloodGroup.SelectedItem = _patient.BloodGroup ?? "-";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Full Name is required!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAge.Text) || !int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Please enter a valid Age!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone is required!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            Gender gender = cmbGender.SelectedItem.ToString() == "Male" ? Gender.Male : Gender.Female;
            string blood = cmbBloodGroup.SelectedItem.ToString() == "-" ? null : cmbBloodGroup.SelectedItem.ToString();

            if (_isEdit)
            {
                _patient.Name = txtName.Text.Trim();
                _patient.Age = age;
                _patient.Gender = gender;
                _patient.Phone = txtPhone.Text.Trim();
                _patient.CNIC = txtCnic.Text.Trim();
                _patient.BloodGroup = blood;
                _patient.Address = txtAddress.Text.Trim();
                _service.Update(_patient);
            }
            else
            {
                Patient p = new Patient();
                p.Name = txtName.Text.Trim();
                p.Age = age;
                p.Gender = gender;
                p.Phone = txtPhone.Text.Trim();
                p.CNIC = txtCnic.Text.Trim();
                p.BloodGroup = blood;
                p.Address = txtAddress.Text.Trim();
                p.RegisteredOn = DateTime.Now;
                _service.Add(p);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}