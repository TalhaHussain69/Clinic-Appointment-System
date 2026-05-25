using App.core.Contracts;
using App.core.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace App.core.Services
{
    public class DbMedicalRecordService : IMedicalRecordService
    {
        private readonly string _connectionString;

        public DbMedicalRecordService(string connectionString)
        {
            _connectionString = connectionString;
        }

        MedicalRecord IMedicalRecordService.Add(MedicalRecord record)
        {
            record.Id = "MR-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO MedicalRecord(Id, PatientId, DoctorId, AppointmentId, Diagnosis, Prescription, Notes, RecordDate) " +
                             "VALUES(@Id, @PatientId, @DoctorId, @AppointmentId, @Diagnosis, @Prescription, @Notes, @RecordDate)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", record.Id);
                    cmd.Parameters.AddWithValue("@PatientId", record.PatientId);
                    cmd.Parameters.AddWithValue("@DoctorId", record.DoctorId);
                    cmd.Parameters.AddWithValue("@AppointmentId", record.AppointmentId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Diagnosis", record.Diagnosis);
                    cmd.Parameters.AddWithValue("@Prescription", record.Prescription ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", record.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RecordDate", record.RecordDate);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? record : null;
                }
            }
        }

        bool IMedicalRecordService.Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM MedicalRecord WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        List<MedicalRecord> IMedicalRecordService.GetAll()
        {
            List<MedicalRecord> records = new List<MedicalRecord>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT mr.*, 
                                      p.Name AS PatientName,
                                      d.Name AS DoctorName
                               FROM MedicalRecord mr
                               INNER JOIN Patient p ON mr.PatientId = p.Id
                               INNER JOIN Doctor  d ON mr.DoctorId  = d.Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        records.Add(ReadRecord(reader));
                }
            }
            return records;
        }

        MedicalRecord IMedicalRecordService.GetById(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT mr.*, 
                                      p.Name AS PatientName,
                                      d.Name AS DoctorName
                               FROM MedicalRecord mr
                               INNER JOIN Patient p ON mr.PatientId = p.Id
                               INNER JOIN Doctor  d ON mr.DoctorId  = d.Id
                               WHERE mr.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return ReadRecord(reader);
                    }
                }
            }
            return null;
        }

        List<MedicalRecord> IMedicalRecordService.GetByPatientId(string patientId)
        {
            List<MedicalRecord> records = new List<MedicalRecord>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT mr.*, 
                                      p.Name AS PatientName,
                                      d.Name AS DoctorName
                               FROM MedicalRecord mr
                               INNER JOIN Patient p ON mr.PatientId = p.Id
                               INNER JOIN Doctor  d ON mr.DoctorId  = d.Id
                               WHERE mr.PatientId = @PatientId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            records.Add(ReadRecord(reader));
                    }
                }
            }
            return records;
        }

        List<MedicalRecord> IMedicalRecordService.Search(string text)
        {
            List<MedicalRecord> records = new List<MedicalRecord>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT mr.*, 
                                      p.Name AS PatientName,
                                      d.Name AS DoctorName
                               FROM MedicalRecord mr
                               INNER JOIN Patient p ON mr.PatientId = p.Id
                               INNER JOIN Doctor  d ON mr.DoctorId  = d.Id
                               WHERE 1=1";

                if (!string.IsNullOrEmpty(text))
                    sql += " AND (p.Name LIKE @Text OR mr.Diagnosis LIKE @Text)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(text))
                        cmd.Parameters.AddWithValue("@Text", "%" + text.Trim() + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            records.Add(ReadRecord(reader));
                    }
                }
            }
            return records;
        }

        bool IMedicalRecordService.Update(MedicalRecord record)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE MedicalRecord SET " +
                             "Diagnosis=@Diagnosis, Prescription=@Prescription, Notes=@Notes " +
                             "WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Diagnosis", record.Diagnosis);
                    cmd.Parameters.AddWithValue("@Prescription", record.Prescription ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", record.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", record.Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private MedicalRecord ReadRecord(SqlDataReader reader)
        {
            MedicalRecord r = new MedicalRecord();
            r.Id = reader["Id"].ToString();
            r.PatientId = reader["PatientId"].ToString();
            r.DoctorId = reader["DoctorId"].ToString();
            r.AppointmentId = reader["AppointmentId"] == DBNull.Value ? null : reader["AppointmentId"].ToString();
            r.Diagnosis = reader["Diagnosis"].ToString();
            r.Prescription = reader["Prescription"] == DBNull.Value ? null : reader["Prescription"].ToString();
            r.Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();
            r.RecordDate = Convert.ToDateTime(reader["RecordDate"]);
            r.PatientName = reader["PatientName"].ToString();
            r.DoctorName = reader["DoctorName"].ToString();
            return r;
        }
    }
}
