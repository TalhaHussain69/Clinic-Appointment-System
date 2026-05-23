using App.core.Contracts;
using App.core.Models;
using App.core.Utilities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace App.core.Services
{
    public class DbPatientService : IPatientService
    {
        private readonly string _connectionString;

        public DbPatientService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ============================================================
        // ADD — Naya patient database mein daalo
        // ============================================================
        Patient IPatientService.Add(Patient patient)
        {
            patient.Id = "PT-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO Patient(Id, Name, Age, Gender, Phone, CNIC, BloodGroup, Address, RegisteredOn) " +
                             "VALUES(@Id, @Name, @Age, @Gender, @Phone, @CNIC, @BloodGroup, @Address, @RegisteredOn)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", patient.Id);
                    cmd.Parameters.AddWithValue("@Name", patient.Name);
                    cmd.Parameters.AddWithValue("@Age", patient.Age);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender.ToString());
                    cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                    cmd.Parameters.AddWithValue("@CNIC", patient.CNIC ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", patient.Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RegisteredOn", patient.RegisteredOn);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? patient : null;
                }
            }
        }

        // ============================================================
        // DELETE — Patient ko ID se hatao
        // ============================================================
        bool IPatientService.Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Patient WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // ============================================================
        // GET ALL — Saare patients lao
        // ============================================================
        List<Patient> IPatientService.GetAll()
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Patient", conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patient p = new Patient();
                        p.Id = reader["Id"].ToString();
                        p.Name = reader["Name"].ToString();
                        p.Age = Convert.ToInt32(reader["Age"]);
                        p.Gender = Enum.TryParse<Gender>(reader["Gender"].ToString(), out var gender)
                                        ? gender : Gender.Male;
                        p.Phone = reader["Phone"].ToString();
                        p.CNIC = reader["CNIC"] == DBNull.Value ? null : reader["CNIC"].ToString();
                        p.BloodGroup = reader["BloodGroup"] == DBNull.Value ? null : reader["BloodGroup"].ToString();
                        p.Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString();
                        p.RegisteredOn = Convert.ToDateTime(reader["RegisteredOn"]);

                        patients.Add(p);
                    }
                }
            }

            return patients;
        }

        // ============================================================
        // GET BY ID — Ek patient ID se dhundo
        // ============================================================
        Patient IPatientService.GetById(string id)
        {
            Patient patient = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Patient WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            patient = new Patient();
                            patient.Id = reader["Id"].ToString();
                            patient.Name = reader["Name"].ToString();
                            patient.Age = Convert.ToInt32(reader["Age"]);
                            patient.Gender = Enum.TryParse<Gender>(reader["Gender"].ToString(), out var gender)
                                                  ? gender : Gender.Male;
                            patient.Phone = reader["Phone"].ToString();
                            patient.CNIC = reader["CNIC"] == DBNull.Value ? null : reader["CNIC"].ToString();
                            patient.BloodGroup = reader["BloodGroup"] == DBNull.Value ? null : reader["BloodGroup"].ToString();
                            patient.Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString();
                            patient.RegisteredOn = Convert.ToDateTime(reader["RegisteredOn"]);
                        }
                    }
                }
            }

            return patient;
        }

        // ============================================================
        // SEARCH — Name, Phone ya Gender se dhundo
        // ============================================================
        List<Patient> IPatientService.Search(string text, Gender? gender)
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Patient WHERE 1=1";

                if (!string.IsNullOrEmpty(text))
                    query += " AND (Name LIKE @Text OR Phone LIKE @Text)";

                if (gender != null)
                    query += " AND Gender = @Gender";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(text))
                        cmd.Parameters.AddWithValue("@Text", "%" + text.Trim() + "%");

                    if (gender != null)
                        cmd.Parameters.AddWithValue("@Gender", gender.ToString());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Patient p = new Patient();
                            p.Id = reader["Id"].ToString();
                            p.Name = reader["Name"].ToString();
                            p.Age = Convert.ToInt32(reader["Age"]);
                            p.Gender = Enum.TryParse<Gender>(reader["Gender"].ToString(), out var genderEnum)
                                            ? genderEnum : Gender.Male;
                            p.Phone = reader["Phone"].ToString();
                            p.CNIC = reader["CNIC"] == DBNull.Value ? null : reader["CNIC"].ToString();
                            p.BloodGroup = reader["BloodGroup"] == DBNull.Value ? null : reader["BloodGroup"].ToString();
                            p.Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString();
                            p.RegisteredOn = Convert.ToDateTime(reader["RegisteredOn"]);

                            patients.Add(p);
                        }
                    }
                }
            }

            return patients;
        }

        // ============================================================
        // UPDATE — Existing patient ki values badlo
        // ============================================================
        bool IPatientService.Update(Patient patient)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Patient SET " +
                             "Name=@Name, Age=@Age, Gender=@Gender, Phone=@Phone, " +
                             "CNIC=@CNIC, BloodGroup=@BloodGroup, Address=@Address " +
                             "WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", patient.Name);
                    cmd.Parameters.AddWithValue("@Age", patient.Age);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender.ToString());
                    cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                    cmd.Parameters.AddWithValue("@CNIC", patient.CNIC ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", patient.Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", patient.Id);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }
}