using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace App.core.Services
{
    public class DbAppointmentService : IAppointmentService
    {
        private readonly string _connectionString;

        public DbAppointmentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ============================================================
        // ADD — Naya appointment database mein daalo
        // ============================================================
        Appointment IAppointmentService.Add(Appointment appointment)
        {
            appointment.Id = "APT-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO Appointment(Id, PatientId, DoctorId, AppDate, AppTime, Type, Status, Fee, Notes) " +
                             "VALUES(@Id, @PatientId, @DoctorId, @AppDate, @AppTime, @Type, @Status, @Fee, @Notes)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", appointment.Id);
                    cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                    cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                    cmd.Parameters.AddWithValue("@AppDate", appointment.AppDate);
                    cmd.Parameters.AddWithValue("@AppTime", appointment.AppTime);
                    cmd.Parameters.AddWithValue("@Type", appointment.Type.ToString());
                    cmd.Parameters.AddWithValue("@Status", appointment.Status.ToString());
                    cmd.Parameters.AddWithValue("@Fee", appointment.Fee);
                    cmd.Parameters.AddWithValue("@Notes", appointment.Notes ?? (object)DBNull.Value);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? appointment : null;
                }
            }
        }

        // ============================================================
        // DELETE
        // ============================================================
        bool IAppointmentService.Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Appointment WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // ============================================================
        // GET ALL — Patient aur Doctor ka naam bhi lao (JOIN)
        // ============================================================
        List<Appointment> IAppointmentService.GetAll()
        {
            List<Appointment> appointments = new List<Appointment>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // JOIN lagaya — Patient aur Doctor ka naam bhi aayega
                string sql = @"SELECT a.*, 
                                      p.Name AS PatientName, 
                                      d.Name AS DoctorName
                               FROM Appointment a
                               INNER JOIN Patient p ON a.PatientId = p.Id
                               INNER JOIN Doctor  d ON a.DoctorId  = d.Id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(ReadAppointment(reader));
                    }
                }
            }

            return appointments;
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        Appointment IAppointmentService.GetById(string id)
        {
            Appointment appointment = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"SELECT a.*, 
                                      p.Name AS PatientName, 
                                      d.Name AS DoctorName
                               FROM Appointment a
                               INNER JOIN Patient p ON a.PatientId = p.Id
                               INNER JOIN Doctor  d ON a.DoctorId  = d.Id
                               WHERE a.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            appointment = ReadAppointment(reader);
                    }
                }
            }

            return appointment;
        }

        // ============================================================
        // SEARCH — Text, Status, Type se filter karo
        // ============================================================
        List<Appointment> IAppointmentService.Search(string text, AppointmentStatus? status, AppointmentType? type)
        {
            List<Appointment> appointments = new List<Appointment>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT a.*, 
                                        p.Name AS PatientName, 
                                        d.Name AS DoctorName
                                 FROM Appointment a
                                 INNER JOIN Patient p ON a.PatientId = p.Id
                                 INNER JOIN Doctor  d ON a.DoctorId  = d.Id
                                 WHERE 1=1";

                if (!string.IsNullOrEmpty(text))
                    query += " AND (p.Name LIKE @Text OR d.Name LIKE @Text)";

                if (status != null)
                    query += " AND a.Status = @Status";

                if (type != null)
                    query += " AND a.Type = @Type";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(text))
                        cmd.Parameters.AddWithValue("@Text", "%" + text.Trim() + "%");

                    if (status != null)
                        cmd.Parameters.AddWithValue("@Status", status.ToString());

                    if (type != null)
                        cmd.Parameters.AddWithValue("@Type", type.ToString());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            appointments.Add(ReadAppointment(reader));
                    }
                }
            }

            return appointments;
        }

        // ============================================================
        // UPDATE
        // ============================================================
        bool IAppointmentService.Update(Appointment appointment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Appointment SET " +
                             "PatientId=@PatientId, DoctorId=@DoctorId, " +
                             "AppDate=@AppDate, AppTime=@AppTime, " +
                             "Type=@Type, Status=@Status, Fee=@Fee, Notes=@Notes " +
                             "WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                    cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                    cmd.Parameters.AddWithValue("@AppDate", appointment.AppDate);
                    cmd.Parameters.AddWithValue("@AppTime", appointment.AppTime);
                    cmd.Parameters.AddWithValue("@Type", appointment.Type.ToString());
                    cmd.Parameters.AddWithValue("@Status", appointment.Status.ToString());
                    cmd.Parameters.AddWithValue("@Fee", appointment.Fee);
                    cmd.Parameters.AddWithValue("@Notes", appointment.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", appointment.Id);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // ============================================================
        // HELPER — Reader se Appointment object banana (DRY principle)
        // ============================================================
        private Appointment ReadAppointment(SqlDataReader reader)
        {
            Appointment a = new Appointment();
            a.Id = reader["Id"].ToString();
            a.PatientId = reader["PatientId"].ToString();
            a.DoctorId = reader["DoctorId"].ToString();
            a.AppDate = Convert.ToDateTime(reader["AppDate"]);
            a.AppTime = reader["AppTime"].ToString();
            a.Type = Enum.TryParse<AppointmentType>(reader["Type"].ToString(), out var type)
                            ? type : AppointmentType.General;
            a.Status = Enum.TryParse<AppointmentStatus>(reader["Status"].ToString(), out var status)
                            ? status : AppointmentStatus.Scheduled;
            a.Fee = Convert.ToDecimal(reader["Fee"]);
            a.Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();

            // JOIN se aaye naam
            a.PatientName = reader["PatientName"].ToString();
            a.DoctorName = reader["DoctorName"].ToString();

            return a;
        }
    }
}