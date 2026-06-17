using System;
using System.Collections.Generic;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class AppointmentForm : Form
    {
        private IAppointmentService _appointmentService;
        private IPatientService _patientService;
        private IDoctorService _doctorService;
        private Appointment _appointment;
        private FormMode _mode;
        private List<Patient> _patients;
        private List<Doctor> _doctors;

        public AppointmentForm(IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService, Appointment appointment, FormMode mode)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
            _appointment = appointment;
            _mode = mode;
            InitializeComponent();
            LoadPatients();
            LoadDoctors();

            switch (_mode)
            {
                case FormMode.Add:
                    lblTitle.Text = "New Appointment";
                    btnSave.Text = "Save";
                    break;
                case FormMode.Edit:
                    lblTitle.Text = "Edit Appointment";
                    btnSave.Text = "Update";
                    FillForm();
                    break;
                case FormMode.View:
                    lblTitle.Text = "View Appointment";
                    btnSave.Visible = false;
                    cmbPatient.Enabled = false;
                    cmbDoctor.Enabled = false;
                    dtpDate.Enabled = false;
                    cmbTime.Enabled = false;
                    cmbType.Enabled = false;
                    cmbStatus.Enabled = false;
                    txtFee.ReadOnly = true;
                    txtNotes.ReadOnly = true;
                    FillForm();
                    break;
            }
        }

        private void LoadPatients()
        {
            _patients = _patientService.GetAll();
            cmbPatient.Items.Clear();
            foreach (var p in _patients)
                cmbPatient.Items.Add(p.Name);
            if (cmbPatient.Items.Count > 0)
                cmbPatient.SelectedIndex = 0;
        }

        private void LoadDoctors()
        {
            _doctors = _doctorService.GetAll();
            cmbDoctor.Items.Clear();
            foreach (var d in _doctors)
                cmbDoctor.Items.Add(d.Name + " (" + d.Specialization + ")");
            if (cmbDoctor.Items.Count > 0)
                cmbDoctor.SelectedIndex = 0;
        }

        private void FillForm()
        {
            for (int i = 0; i < _patients.Count; i++)
                if (_patients[i].Id == _appointment.PatientId) { cmbPatient.SelectedIndex = i; break; }

            for (int i = 0; i < _doctors.Count; i++)
                if (_doctors[i].Id == _appointment.DoctorId) { cmbDoctor.SelectedIndex = i; break; }

            dtpDate.Value = _appointment.AppDate;
            cmbTime.SelectedItem = _appointment.AppTime;
            cmbType.SelectedItem = _appointment.Type.ToString();
            cmbStatus.SelectedItem = _appointment.Status.ToString();
            txtFee.Text = _appointment.Fee.ToString();
            txtNotes.Text = _appointment.Notes ?? "";
        }

       

       

       
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select a Patient!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(cmbDoctor.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select a Doctor!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Decimal fee = 0;
            if(!string.IsNullOrWhiteSpace(txtFee.Text) && !decimal.TryParse(txtFee.Text, out fee))
            {
                MessageBox.Show("Please enter a valid Fee!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string patientId = _patients[cmbPatient.SelectedIndex].Id;
            string doctorId = _doctors[cmbDoctor.SelectedIndex].Id;

            AppointmentType type = (AppointmentType)System.Enum.Parse(
                typeof(AppointmentType), cmbType.SelectedItem.ToString());
            AppointmentStatus status = (AppointmentStatus)System.Enum.Parse(
                typeof(AppointmentStatus), cmbStatus.SelectedItem.ToString());

            if (_mode == FormMode.Edit)
            {
                _appointment.PatientId = patientId;
                _appointment.DoctorId = doctorId;
                _appointment.AppDate = dtpDate.Value.Date;
                _appointment.AppTime = cmbTime.SelectedItem.ToString();
                _appointment.Type = type;
                _appointment.Status = status;
                _appointment.Fee = fee;
                _appointment.Notes = txtNotes.Text.Trim();
                _appointmentService.Update(_appointment);
            }
            else
            {
                Appointment a = new Appointment();
                a.PatientId = patientId;
                a.DoctorId = doctorId;
                a.AppDate = dtpDate.Value.Date;
                a.AppTime = cmbTime.SelectedItem.ToString();
                a.Type = type;
                a.Status = status;
                a.Fee = fee;
                a.Notes = txtNotes.Text.Trim();
                _appointmentService.Add(a);
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