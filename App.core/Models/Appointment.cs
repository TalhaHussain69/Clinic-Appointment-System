using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.core.Utilities;

namespace App.core.Models
{
    public class Appointment
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppDate { get; set; }
        public string AppTime { get; set; }
        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal Fee { get; set; }
        public string Notes { get; set; }

        // Extra — UI mein naam dikhane ke liye
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }
}
