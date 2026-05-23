using App.core.Models;
using App.core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.core.Contracts
{
    public  interface IAppointmentService
    {
        List<Appointment> GetAll();
        Appointment GetById(string id);
        Appointment Add(Appointment appointment);
        bool Update(Appointment appointment);
        bool Delete(string id);
        List<Appointment> Search(string text, AppointmentStatus? status, AppointmentType? type);
    }
}
