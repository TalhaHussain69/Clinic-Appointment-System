USE ClinicDB;

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