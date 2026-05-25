using App.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.core.Contracts
{
    public interface IPaymentService
    {
        List<Payment> GetAll();
        Payment GetById(string  id);
        Payment Add(Payment payment);
        bool Update(Payment payment);
        bool Delete(string id);
        List<Payment> Search(string text, string status);
        List<Payment> GetByAppointmentId(string appointmentId);

    }
}
