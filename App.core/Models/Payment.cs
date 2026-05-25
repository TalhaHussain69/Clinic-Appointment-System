using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.core.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public string AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }
}
