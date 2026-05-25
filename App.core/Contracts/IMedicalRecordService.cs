using App.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.core.Contracts
{
    public interface IMedicalRecordService
    {
        List<MedicalRecord> GetAll();
        MedicalRecord GetById(string id);
        MedicalRecord Add(MedicalRecord record);
        bool Update(MedicalRecord record);
        bool Delete(string id);
        List<MedicalRecord> Search(string text);
        List<MedicalRecord> GetByPatientId(string patientId);
    }
}
