using System;
using System.Collections.Generic;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;

namespace App.WindowsApp
{
    public partial class MedicalRecordForm : Form
    {
        private IMedicalRecordService _recordService;
        private IPatientService _patientService;
        private IDoctorService _doctorService;
        private IAppointmentService _appointmentService;
        private MedicalRecord _record;
        private FormMode _mode;
        private List<Patient> _patients;
        private List<Doctor> _doctors;

        public MedicalRecordForm(IMedicalRecordService recordService, IPatientService patientService, IDoctorService doctorService, IAppointmentService appointmentService, MedicalRecord record, FormMode mode)
        {
            _recordService = recordService;
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _record = record;
            _mode = mode;
            InitializeComponent();
            LoadPatients();
            LoadDoctors();

            switch (_mode)
            {
                case FormMode.Add:
                    lblTitle.Text = "Add Medical Record";
                    btnSave.Text = "Save";
                    break;
                case FormMode.Edit:
                    lblTitle.Text = "Edit Medical Record";
                    btnSave.Text = "Update";
                    FillForm();
                    break;
                case FormMode.View:
                    lblTitle.Text = "View Medical Record";
                    btnSave.Visible = false;
                    cmbPatient.Enabled = false;
                    cmbDoctor.Enabled = false;
                    txtDiagnosis.ReadOnly = true;
                    txtPrescription.ReadOnly = true;
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
                if (_patients[i].Id == _record.PatientId) { cmbPatient.SelectedIndex = i; break; }

            for (int i = 0; i < _doctors.Count; i++)
                if (_doctors[i].Id == _record.DoctorId) { cmbDoctor.SelectedIndex = i; break; }

            txtDiagnosis.Text = _record.Diagnosis;
            txtPrescription.Text = _record.Prescription ?? "";
            txtNotes.Text = _record.Notes ?? "";
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDiagnosis.Text))
            {
                MessageBox.Show("Diagnosis is required!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiagnosis.Focus();
                return;
            }

            if (_mode == FormMode.Edit)
            {
                _record.PatientId = _patients[cmbPatient.SelectedIndex].Id;
                _record.DoctorId = _doctors[cmbDoctor.SelectedIndex].Id;
                _record.Diagnosis = txtDiagnosis.Text.Trim();
                _record.Prescription = txtPrescription.Text.Trim();
                _record.Notes = txtNotes.Text.Trim();
                _recordService.Update(_record);
            }
            else
            {
                MedicalRecord r = new MedicalRecord();
                r.PatientId = _patients[cmbPatient.SelectedIndex].Id;
                r.DoctorId = _doctors[cmbDoctor.SelectedIndex].Id;
                r.Diagnosis = txtDiagnosis.Text.Trim();
                r.Prescription = txtPrescription.Text.Trim();
                r.Notes = txtNotes.Text.Trim();
                r.RecordDate = DateTime.Now;
                _recordService.Add(r);
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