using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.core.Utilities;
using System.Threading.Tasks;

namespace App.core.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string CNIC { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredOn { get; set; }
    }
}
