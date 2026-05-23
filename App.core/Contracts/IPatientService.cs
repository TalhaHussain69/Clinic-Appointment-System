using App.core.Models;
using App.core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.core.Contracts
{
    public interface IPatientService
    {
        List<Patient> GetAll();
        Patient GetById(string id);
        Patient Add(Patient patient);
        bool Update(Patient patient);
        bool Delete(string id);
        List<Patient> Search(string text, Gender? gender);
    }
}
