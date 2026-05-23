using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.core.Utilities;
using System.Threading.Tasks;

namespace App.core.Models
{
    public class Doctor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public decimal Fee { get; set; }
        public DoctorStatus Status { get; set; }
    }
}

    