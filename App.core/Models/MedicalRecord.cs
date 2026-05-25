using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.core.Models
{
    public class MedicalRecord
    {
        public string  Id { get; set; }
        public string PatientId { get; set; }
        public string doctorId { get; set; }
        public string AppointmentId { get; set; }
        public string Diagnosis {  get; set; }
        public string Prescription {  get; set; }
        public string Notes { get; set; }
        public DateTime RecordDate { get; set; }


        public string PatientName { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
    }
}
