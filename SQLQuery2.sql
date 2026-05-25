USE ClinicDB;

CREATE TABLE Payment (
    Id              VARCHAR(20)   PRIMARY KEY,
    AppointmentId   VARCHAR(20)   NOT NULL,
    Amount          DECIMAL(10,2) NOT NULL,
    PaymentDate     DATETIME      DEFAULT GETDATE(),
    PaymentMethod   VARCHAR(20)   NOT NULL,
    Status          VARCHAR(20)   NOT NULL,
    Notes           VARCHAR(500)  NULL,
    FOREIGN KEY (AppointmentId) REFERENCES Appointment(Id)
);

CREATE TABLE MedicalRecord (
    Id            VARCHAR(20)   PRIMARY KEY,
    PatientId     VARCHAR(20)   NOT NULL,
    DoctorId      VARCHAR(20)   NOT NULL,
    AppointmentId VARCHAR(20)   NULL,
    Diagnosis     VARCHAR(500)  NOT NULL,
    Prescription  VARCHAR(500)  NULL,
    Notes         VARCHAR(500)  NULL,
    RecordDate    DATETIME      DEFAULT GETDATE(),
    FOREIGN KEY (PatientId)     REFERENCES Patient(Id),
    FOREIGN KEY (DoctorId)      REFERENCES Doctor(Id),
    FOREIGN KEY (AppointmentId) REFERENCES Appointment(Id)
);