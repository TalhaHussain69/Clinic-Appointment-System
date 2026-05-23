using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace App.core.Services
{
    public class DbDoctorService : IDoctorService
    {
        private readonly string _connectionString;

        public DbDoctorService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ============================================================
        // ADD — Naya doctor database mein daalo
        // ============================================================
        Doctor IDoctorService.Add(Doctor doctor)
        {
            doctor.Id = "DR-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO Doctor(Id, Name, Specialization, Phone, Email, Fee, Status) " +
                             "VALUES(@Id, @Name, @Specialization, @Phone, @Email, @Fee, @Status)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", doctor.Id);
                    cmd.Parameters.AddWithValue("@Name", doctor.Name);
                    cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                    cmd.Parameters.AddWithValue("@Phone", doctor.Phone);
                    cmd.Parameters.AddWithValue("@Email", doctor.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Fee", doctor.Fee);
                    cmd.Parameters.AddWithValue("@Status", doctor.Status.ToString());

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? doctor : null;
                }
            }
        }

        // ============================================================
        // DELETE — Doctor ko ID se hatao
        // ============================================================
        bool IDoctorService.Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Doctor WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // ============================================================
        // GET ALL — Saare doctors lao
        // ============================================================
        List<Doctor> IDoctorService.GetAll()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Doctor", conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Doctor d = new Doctor();
                        d.Id = reader["Id"].ToString();
                        d.Name = reader["Name"].ToString();
                        d.Specialization = reader["Specialization"].ToString();
                        d.Phone = reader["Phone"].ToString();
                        d.Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString();
                        d.Fee = Convert.ToDecimal(reader["Fee"]);
                        d.Status = Enum.TryParse<DoctorStatus>(reader["Status"].ToString(), out var status)
                                           ? status : DoctorStatus.Active;

                        doctors.Add(d);
                    }
                }
            }

            return doctors;
        }

        // ============================================================
        // GET BY ID — Ek doctor ID se dhundo
        // ============================================================
        Doctor IDoctorService.GetById(string id)
        {
            Doctor doctor = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Doctor WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            doctor = new Doctor();
                            doctor.Id = reader["Id"].ToString();
                            doctor.Name = reader["Name"].ToString();
                            doctor.Specialization = reader["Specialization"].ToString();
                            doctor.Phone = reader["Phone"].ToString();
                            doctor.Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString();
                            doctor.Fee = Convert.ToDecimal(reader["Fee"]);
                            doctor.Status = Enum.TryParse<DoctorStatus>(reader["Status"].ToString(), out var status)
                                                    ? status : DoctorStatus.Active;
                        }
                    }
                }
            }

            return doctor;
        }

        // ============================================================
        // SEARCH — Name ya Status se dhundo
        // ============================================================
        List<Doctor> IDoctorService.Search(string text, DoctorStatus? status)
        {
            List<Doctor> doctors = new List<Doctor>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Doctor WHERE 1=1";

                if (!string.IsNullOrEmpty(text))
                    query += " AND Name LIKE @Text";

                if (status != null)
                    query += " AND Status = @Status";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(text))
                        cmd.Parameters.AddWithValue("@Text", "%" + text.Trim() + "%");

                    if (status != null)
                        cmd.Parameters.AddWithValue("@Status", status.ToString());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Doctor d = new Doctor();
                            d.Id = reader["Id"].ToString();
                            d.Name = reader["Name"].ToString();
                            d.Specialization = reader["Specialization"].ToString();
                            d.Phone = reader["Phone"].ToString();
                            d.Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString();
                            d.Fee = Convert.ToDecimal(reader["Fee"]);
                            d.Status = Enum.TryParse<DoctorStatus>(reader["Status"].ToString(), out var statusEnum)
                                               ? statusEnum : DoctorStatus.Active;

                            doctors.Add(d);
                        }
                    }
                }
            }

            return doctors;
        }

        // ============================================================
        // UPDATE — Existing doctor ki values badlo
        // ============================================================
        bool IDoctorService.Update(Doctor doctor)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Doctor SET " +
                             "Name=@Name, Specialization=@Specialization, " +
                             "Phone=@Phone, Email=@Email, Fee=@Fee, Status=@Status " +
                             "WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", doctor.Name);
                    cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                    cmd.Parameters.AddWithValue("@Phone", doctor.Phone);
                    cmd.Parameters.AddWithValue("@Email", doctor.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Fee", doctor.Fee);
                    cmd.Parameters.AddWithValue("@Status", doctor.Status.ToString());
                    cmd.Parameters.AddWithValue("@Id", doctor.Id);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }
}