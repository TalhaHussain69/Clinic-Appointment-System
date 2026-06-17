# Clinic Appointment System

> Semester Project — Advanced Programming (COSC-5136) | Spring 2026
> Department of Computer Science

---

## Group Members

| Name | Roll Number |
|---|---|
| Muhammad Talha Hussain | F23BDOCS1E02140 |
| Ahmad Aftab | F23BDOCS1E02151 |
| Aamna Aftab | F23BDOCS1E2100 |

---

## About the Project

Clinic Appointment System is a desktop-based clinic management application built using C# and WinForms on .NET Framework 4.8. It was developed as our semester project for the Advanced Programming course and covers the complete workflow of a clinic — from registering patients and managing doctors to booking appointments, tracking payments, and maintaining medical records.

The project follows the 3-layer architecture taught in class, with a strict separation between the business logic layer (`App.core`) and the user interface layer (`App.WindowsApp`). All data access is done through raw ADO.NET with no ORM, and the project implements both synchronous and asynchronous database operations.

---

## Features

### Patient Management
- Add, edit, view, and delete patient records
- Tracks full name, age, gender, phone, CNIC, blood group, and address
- Search by name or phone number
- Filter by gender
- Confirm-before-delete dialog on all records

### Doctor Management
- Add, edit, view, and delete doctor records
- Tracks name, specialization, phone, email, consultation fee, and availability status
- Search by name
- Filter by status (Active, On Leave, Inactive)

### Appointment Management
- Book new appointments between any patient and any doctor
- Tracks appointment date, time slot, type, status, fee, and notes
- Status options: Scheduled, Confirmed, Pending, Cancelled
- Search by patient name or doctor name
- Filter by status and appointment type

### Payment Management
- Record and track payments linked to appointments
- Tracks amount, payment method (Cash, Card, Bank Transfer, Online), status, and date
- Search by patient name
- Filter by payment status (Paid, Pending, Cancelled, Refunded)

### Medical Records
- Maintain diagnosis and prescription records per patient
- Links records to a specific patient, doctor, and appointment
- Search by patient name or diagnosis keyword

### Dashboard
- Live stat cards showing total patients, total doctors, total appointments, and pending count
- Recent appointments table with full details

### Charts & Analytics
- Pie chart — appointments broken down by status (Scheduled, Confirmed, Pending, Cancelled)
- Bar chart — patients broken down by gender (Male, Female)
- Charts pull from real database data and update on refresh

### General
- Mode-driven forms — one form per entity handles Add, Edit, and View modes via a `FormMode` enum
- FlowLayoutPanel for dynamic button layout in View mode
- Confirm-before-delete dialog on all records
- Field validation with MessageBox feedback on required fields
- BindingSource used for all DataGridView bindings
- Search and filter on all entity views
- Async data loading with Task.Delay demonstration

---

## Tech Stack

| Layer | Technology |
|---|---|
| Language | C# / .NET Framework 4.8 |
| UI Framework | Windows Forms (WinForms) |
| Business Logic | Class Library — `App.core` |
| Data Access | ADO.NET — `System.Data.SqlClient` |
| Charts | LiveCharts2 — `LiveChartsCore.SkiaSharpView.WinForms` |
| Database | SQL Server LocalDB `(localdb)\MSSQLLocalDB` |
| Configuration | `App.config` + `ConfigurationManager` |

---

## Project Structure

```
Clinic-Appointment-System/
│
├── App.core/                          # Business logic — no UI dependency
│   ├── Models/
│   │   ├── Patient.cs                 # Patient entity
│   │   ├── Doctor.cs                  # Doctor entity
│   │   ├── Appointment.cs             # Appointment entity
│   │   ├── Payment.cs                 # Payment entity
│   │   └── MedicalRecord.cs           # Medical record entity
│   │
│   ├── Contracts/
│   │   ├── IPatientService.cs         # Contract for patient operations
│   │   ├── IDoctorService.cs          # Contract for doctor operations
│   │   ├── IAppointmentService.cs     # Contract for appointment operations
│   │   ├── IPaymentService.cs         # Contract for payment operations
│   │   └── IMedicalRecordService.cs   # Contract for medical record operations
│   │
│   ├── Services/
│   │   ├── DbPatientService.cs        # ADO.NET implementation
│   │   ├── DbDoctorService.cs         # ADO.NET implementation
│   │   ├── DbAppointmentService.cs    # ADO.NET implementation
│   │   ├── DbPaymentService.cs        # ADO.NET implementation
│   │   └── DbMedicalRecordService.cs  # ADO.NET implementation
│   │
│   └── Utilities/
│       ├── Gender.cs                  # Gender enum
│       ├── DoctorStatus.cs            # DoctorStatus enum
│       ├── AppointmentType.cs         # AppointmentType enum
│       ├── AppointmentStatus.cs       # AppointmentStatus enum
│       ├── PaymentMethod.cs           # PaymentMethod enum
│       ├── PaymentStatus.cs           # PaymentStatus enum
│       └── FormMode.cs                # Add / Edit / View enum
│
└── App.WindowsApp/                    # WinForms UI
    ├── Views/
    │   ├── PatientView.cs             # Patient grid + search + filter + toolbar
    │   ├── DoctorView.cs              # Doctor grid + search + filter + toolbar
    │   ├── AppointmentView.cs         # Appointment grid + search + filter + toolbar
    │   ├── PaymentView.cs             # Payment grid + search + filter + toolbar
    │   ├── MedicalRecordView.cs       # Medical records grid + search + toolbar
    │   └── ChartView.cs              # Pie chart + Bar chart from live data
    │
    ├── Forms/
    │   ├── PatientForm.cs             # Add / Edit / View patient
    │   ├── DoctorForm.cs              # Add / Edit / View doctor
    │   ├── AppointmentForm.cs         # Add / Edit / View appointment
    │   ├── PaymentForm.cs             # Add / Edit / View payment
    │   └── MedicalRecordForm.cs       # Add / Edit / View medical record
    │
    ├── mainform.cs                    # Shell — sidebar navigation + dashboard
    └── App.config                     # Connection string
```

---

## Architecture

The project strictly follows the **3-Layer Architecture** pattern:

```
┌──────────────────────────────────────┐
│         App.WindowsApp (UI)          │  WinForms views and forms
│   PatientView, DoctorView,           │  Only talks to interfaces
│   AppointmentView, ChartView etc.    │
└──────────────────┬───────────────────┘
                   │ depends on
┌──────────────────▼───────────────────┐
│      App.core (Contracts)            │  Interfaces — IPatientService etc.
│      No knowledge of UI or DB        │  Pure abstraction layer
└──────────────────┬───────────────────┘
                   │ implemented by
┌──────────────────▼───────────────────┐
│      App.core (Services)             │  Raw ADO.NET — SqlConnection,
│      DbPatientService                │  SqlCommand, SqlDataReader
│      DbDoctorService                 │  CRUD operations
│      DbAppointmentService            │
│      DbPaymentService                │
│      DbMedicalRecordService          │
└──────────────────┬───────────────────┘
                   │
┌──────────────────▼───────────────────┐
│      SQL Server LocalDB              │  5 tables: Patient, Doctor,
│      Database: ClinicDB              │  Appointment, Payment, MedicalRecord
└──────────────────────────────────────┘
```

**Key design decisions:**
- `App.core` has zero knowledge of the UI — it can be reused with any frontend
- The UI depends on interfaces, not concrete services — follows Dependency Inversion
- VARCHAR primary keys (GUID-derived) — no auto-increment INT IDs
- No FK constraints at database level per project requirements
- `using` blocks on every `SqlConnection` — no resource leaks
- `SqlParameter` used for all queries — no string concatenation (SQL injection prevention)
- `BindingSource` connects lists to DataGridView — `_bindingSource.Current` for row selection
- `FormMode` enum drives Add / Edit / View behaviour in all forms
- `FlowLayoutPanel` handles button layout — Cancel auto-shifts when Save is hidden in View mode

---

## Database

**Server:** `(localdb)\MSSQLLocalDB`
**Database name:** `ClinicDB`

| Table | Primary Key | Key Columns |
|---|---|---|
| Patient | Id (VARCHAR) | Name, Age, Gender, Phone, CNIC, BloodGroup, Address, RegisteredOn |
| Doctor | Id (VARCHAR) | Name, Specialization, Phone, Email, Fee, Status |
| Appointment | Id (VARCHAR) | PatientId, DoctorId, AppDate, AppTime, Type, Status, Fee, Notes |
| Payment | Id (VARCHAR) | AppointmentId, Amount, PaymentDate, PaymentMethod, Status, Notes |
| MedicalRecord | Id (VARCHAR) | PatientId, DoctorId, AppointmentId, Diagnosis, Prescription, Notes, RecordDate |

---

## Getting Started

**Prerequisites**
- Visual Studio 2022 or later
- .NET Framework 4.8
- SQL Server LocalDB (comes with Visual Studio by default)

**Setup steps**

1. Clone the repository
   ```bash
   git clone https://github.com/TalhaHussain69/Clinic-Appointment-System.git
   ```

2. Open `Clinic-Appointment-System.sln` in Visual Studio

3. Set up the database
   - Go to **View → SQL Server Object Explorer**
   - Expand **SQL Server → (localdb)\MSSQLLocalDB**
   - Right-click **Databases → New Database** → name it `ClinicDB`
   - Right-click `ClinicDB` → **New Query**
   - Run the following SQL to create all tables:

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
       Notes     VARCHAR(500)  NULL
   );

   CREATE TABLE Payment (
       Id            VARCHAR(20)   PRIMARY KEY,
       AppointmentId VARCHAR(20)   NOT NULL,
       Amount        DECIMAL(10,2) NOT NULL,
       PaymentDate   DATETIME      DEFAULT GETDATE(),
       PaymentMethod VARCHAR(20)   NOT NULL,
       Status        VARCHAR(20)   NOT NULL,
       Notes         VARCHAR(500)  NULL
   );

   CREATE TABLE MedicalRecord (
       Id            VARCHAR(20)   PRIMARY KEY,
       PatientId     VARCHAR(20)   NOT NULL,
       DoctorId      VARCHAR(20)   NOT NULL,
       AppointmentId VARCHAR(20)   NULL,
       Diagnosis     VARCHAR(500)  NOT NULL,
       Prescription  VARCHAR(500)  NULL,
       Notes         VARCHAR(500)  NULL,
       RecordDate    DATETIME      DEFAULT GETDATE()
   );
   ```

4. Update connection string in `App.WindowsApp/App.config` if needed:
   ```xml
   <connectionStrings>
     <add name="ClinicDB"
          connectionString="Server=(localdb)\MSSQLLocalDB;Database=ClinicDB;Integrated Security=True;"
          providerName="System.Data.SqlClient"/>
   </connectionStrings>
   ```

5. Set the startup project
   - Right-click **App.WindowsApp** → **Set as Startup Project**

6. Build and run
   - Press **Ctrl+Shift+B** to build
   - Press **Ctrl+F5** to run

---

## Course Information

- **Course:** Advanced Programming (COSC-5136)
- **Semester:** Spring 2026
- **Domain:** Clinic Appointment Management
