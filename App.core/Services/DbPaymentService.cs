using App.core.Contracts;
using App.core.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace App.core.Services
{
    public class DbPaymentService : IPaymentService
    {
        private readonly string _connectionString;

        public DbPaymentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        Payment IPaymentService.Add(Payment payment)
        {
            payment.Id = "PAY-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Payment(Id, AppointmentId, Amount, PaymentDate, PaymentMethod, Status, Notes) " +
                             "VALUES(@Id, @AppointmentId, @Amount, @PaymentDate, @PaymentMethod, @Status, @Notes)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", payment.Id);
                    cmd.Parameters.AddWithValue("@AppointmentId", payment.AppointmentId);
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                    cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
                    cmd.Parameters.AddWithValue("@Status", payment.Status);
                    cmd.Parameters.AddWithValue("@Notes", payment.Notes ?? (object)DBNull.Value);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? payment : null;
                }
            }
        }

        bool IPaymentService.Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Payment WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        List<Payment> IPaymentService.GetAll()
        {
            List<Payment> payments = new List<Payment>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT p.*, 
                                      pt.Name AS PatientName,
                                      d.Name  AS DoctorName
                               FROM Payment p
                               INNER JOIN Appointment a  ON p.AppointmentId = a.Id
                               INNER JOIN Patient     pt ON a.PatientId      = pt.Id
                               INNER JOIN Doctor      d  ON a.DoctorId       = d.Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        payments.Add(ReadPayment(reader));
                }
            }
            return payments;
        }

        Payment IPaymentService.GetById(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT p.*, 
                                      pt.Name AS PatientName,
                                      d.Name  AS DoctorName
                               FROM Payment p
                               INNER JOIN Appointment a  ON p.AppointmentId = a.Id
                               INNER JOIN Patient     pt ON a.PatientId      = pt.Id
                               INNER JOIN Doctor      d  ON a.DoctorId       = d.Id
                               WHERE p.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return ReadPayment(reader);
                    }
                }
            }
            return null;
        }

        List<Payment> IPaymentService.GetByAppointmentId(string appointmentId)
        {
            List<Payment> payments = new List<Payment>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Payment WHERE AppointmentId = @AppointmentId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            payments.Add(ReadPayment(reader));
                    }
                }
            }
            return payments;
        }

        List<Payment> IPaymentService.Search(string text, string status)
        {
            List<Payment> payments = new List<Payment>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT p.*, 
                                      pt.Name AS PatientName,
                                      d.Name  AS DoctorName
                               FROM Payment p
                               INNER JOIN Appointment a  ON p.AppointmentId = a.Id
                               INNER JOIN Patient     pt ON a.PatientId      = pt.Id
                               INNER JOIN Doctor      d  ON a.DoctorId       = d.Id
                               WHERE 1=1";

                if (!string.IsNullOrEmpty(text))
                    sql += " AND pt.Name LIKE @Text";

                if (!string.IsNullOrEmpty(status) && status != "-- All --")
                    sql += " AND p.Status = @Status";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(text))
                        cmd.Parameters.AddWithValue("@Text", "%" + text.Trim() + "%");

                    if (!string.IsNullOrEmpty(status) && status != "-- All --")
                        cmd.Parameters.AddWithValue("@Status", status);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            payments.Add(ReadPayment(reader));
                    }
                }
            }
            return payments;
        }

        bool IPaymentService.Update(Payment payment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE Payment SET Amount=@Amount, PaymentDate=@PaymentDate, " +
                             "PaymentMethod=@PaymentMethod, Status=@Status, Notes=@Notes " +
                             "WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                    cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
                    cmd.Parameters.AddWithValue("@Status", payment.Status);
                    cmd.Parameters.AddWithValue("@Notes", payment.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", payment.Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private Payment ReadPayment(SqlDataReader reader)
        {
            Payment p = new Payment();
            p.Id = reader["Id"].ToString();
            p.AppointmentId = reader["AppointmentId"].ToString();
            p.Amount = Convert.ToDecimal(reader["Amount"]);
            p.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
            p.PaymentMethod = reader["PaymentMethod"].ToString();
            p.Status = reader["Status"].ToString();
            p.Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();

            if (HasColumn(reader, "PatientName"))
                p.PatientName = reader["PatientName"].ToString();
            if (HasColumn(reader, "DoctorName"))
                p.DoctorName = reader["DoctorName"].ToString();

            return p;
        }

        private bool HasColumn(SqlDataReader reader, string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i) == name) return true;
            return false;
        }
    }
}