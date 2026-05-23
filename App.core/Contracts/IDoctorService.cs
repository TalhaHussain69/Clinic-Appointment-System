using App.core.Models;
using App.core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App.core.Contracts
{
    public interface IDoctorService
    {
        List<Doctor> GetAll();
        Doctor GetById(string id);
        Doctor Add(Doctor doctor);
        bool Update(Doctor doctor);
        bool Delete(string id);
        List<Doctor> Search(string text, DoctorStatus? status);
    }
}
