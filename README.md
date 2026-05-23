# Clinic Appointment System

A desktop-based Clinic Appointment Management System built with C# Windows Forms and SQL Server. This application helps clinic staff manage patients, doctors, and appointments efficiently.

---

## Features

- **Patient Management** — Add, edit, delete, and search patients with full details including blood group, CNIC, and contact info
- **Doctor Management** — Manage doctors with specialization, fee, and availability status
- **Appointment Management** — Schedule, confirm, and track appointments with date, time, and type
- **Dashboard** — Real-time stats showing total patients, doctors, appointments, and pending count
- **Search & Filter** — Filter data by name, gender, status, and appointment type

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Language | C# (.NET Framework 4.8) |
| UI Framework | Windows Forms (WinForms) |
| Database | SQL Server (LocalDB) |
| Architecture | Service Layer Pattern |
| IDE | Visual Studio 2022 |

---

## Project Structure

Clinic-Appointment-System/
├── App.core/
│   ├── Contracts/        # Interfaces (IPatientService, IDoctorService, IAppointmentService)
│   ├── Models/           # Data models (Patient, Doctor, Appointment)
│   ├── Services/         # Database services (DbPatientService, DbDoctorService, DbAppointmentService)
│   └── Utilities/        # Enums (AppointmentStatus, AppointmentType, DoctorStatus, Gender)
└── App.WindowsApp/
├── mainform.cs       # Main window with navigation
├── PatientView.cs    # Patient list screen
├── PatientForm.cs    # Add/Edit patient form
├── DoctorView.cs     # Doctor list screen
├── DoctorForm.cs     # Add/Edit doctor form
├── AppointmentView.cs  # Appointment list screen
└── AppointmentForm.cs  # Add/Edit appointment form

---

## Database Setup

Run this SQL script in SQL Server to create the required tables:

```sql
CREATE TABLE Doctor (
    Id             VARCHAR(20)   PRIMARY KEY,
    Name           VARCHAR(100)  NOT NULL,
    Specialization VARCHAR(50)   NOT NULL,
    Phone          VARCHAR(20)   NOT NULL,
    Email          VARCHAR(100)  NULL,
    Fee            DECIMAL(10,2) NOT NULL,
    Status         VARCHAR(20)   NOT NULL
);

CREATE TABLE Patient (
    Id           VARCHAR(20)   PRIMARY KEY,
    Name         VARCHAR(100)  NOT NULL,
    Age          INT           NOT NULL,
    Gender       VARCHAR(10)   NOT NULL,
    Phone        VARCHAR(20)   NOT NULL,
    CNIC         VARCHAR(20)   NULL,
    BloodGroup   VARCHAR(5)    NULL,
    Address      VARCHAR(200)  NULL,
    RegisteredOn DATETIME      DEFAULT GETDATE()
);

CREATE TABLE Appointment (
    Id        VARCHAR(20)   PRIMARY KEY,
    PatientId VARCHAR(20)   NOT NULL,
    DoctorId  VARCHAR(20)   NOT NULL,
    AppDate   DATE          NOT NULL,
    AppTime   VARCHAR(10)   NOT NULL,
    Type      VARCHAR(30)   NOT NULL,
    Status    VARCHAR(20)   NOT NULL,
    Fee       DECIMAL(10,2) NULL,
    Notes     VARCHAR(500)  NULL,
    FOREIGN KEY (PatientId) REFERENCES Patient(Id),
    FOREIGN KEY (DoctorId)  REFERENCES Doctor(Id)
);
```

---

## How to Run

1. Clone the repository
 https://github.com/TalhaHussain69/Clinic-Appointment-System.git
2. Open `Clinic-Appointment-System.sln` in Visual Studio

3. Update connection string in `App.WindowsApp/App.config`:
```xml
<connectionStrings>
  <add name="ClinicDB"
       connectionString="Server=(localdb)\MSSQLLocalDB;Database=ClinicDB;Integrated Security=True;"
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```

4. Run the SQL script above to create database tables

5. Press `Ctrl+F5` to run the application

---

## Developer

**Muhammad Talha Hussain**
- GitHub: [@Talha Hussain](https://github.com/TalhaHussain69)

---

## License

This project is for educational purposes.
