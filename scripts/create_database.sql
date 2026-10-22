-- ============================================================================
-- LEVEL UP Studio Management System (Group 10) — Database Schema
-- ============================================================================
-- Generated from:
--   - docs/design/class-diagram.md  (entities, attributes, enumerations, relationships)
--   - CLAUDE.md                     (load order, Primary Key Strategy, Enumerations pattern)
--
-- Conventions applied throughout:
--   - One INT NOT NULL PRIMARY KEY per table, named <entity>_id. No IDENTITY —
--     primary keys are assigned in C# (see "Primary Key Strategy" in CLAUDE.md).
--   - NVARCHAR(50) for short strings; NVARCHAR(MAX) only for long free text.
--   - INT for counts, DECIMAL(10,2) for money, DATE for calendar dates,
--     TIME for clock times, DATETIME2 for full timestamps (matches the
--     Date / Time / DateTime typing used in the class diagram).
--   - All columns NOT NULL unless a one-line comment justifies nullability.
--   - Foreign keys: ON DELETE NO ACTION ON UPDATE NO ACTION.
--   - Enum-like attributes: NVARCHAR(20) + CHECK constraint, no lookup tables.
--   - UNIQUE only where the class diagram / requirements explicitly say so.
--   - Tables are ordered so every FK target exists before its referencing table,
--     per the load order in CLAUDE.md: base entities -> FK-referencing entities
--     -> association classes last.
--
-- Do not run this script by hand against the shared database — schema changes
-- are committed here and applied from source control (see "Git" in CLAUDE.md).
-- ============================================================================

-- ============================================================================
-- 1. Base entities (no FK to other custom entities)
-- ============================================================================

CREATE TABLE Trainees (
    trainee_id        INT NOT NULL PRIMARY KEY,
    fullName          NVARCHAR(50) NOT NULL,
    phone             NVARCHAR(50) NOT NULL,
    email             NVARCHAR(50) NOT NULL,
    dateOfBirth       DATE NOT NULL,
    experienceLevel   NVARCHAR(50) NOT NULL, -- TODO: diagram types this as String with no named enumeration; not converted to a CHECK constraint until allowed values are confirmed
    status            NVARCHAR(20) NOT NULL -- soft-delete flag (UC "Manage Trainees": inactive rather than deleted, to preserve financial history)
        CHECK (status IN (N'ACTIVE', N'INACTIVE', N'ON_LEAVE')),
    registrationDate  DATE NOT NULL,
    medicalLimitations NVARCHAR(MAX) NULL, -- nullable: not every trainee has recorded medical limitations
    CONSTRAINT UQ_Trainees_email UNIQUE (email) -- UC "Manage Trainees" rationale explicitly requires preventing duplicate emails
);

CREATE TABLE Trainers (
    trainer_id        INT NOT NULL PRIMARY KEY,
    fullName          NVARCHAR(50) NOT NULL,
    phone             NVARCHAR(50) NOT NULL,
    email             NVARCHAR(50) NOT NULL,
    employmentType    NVARCHAR(20) NOT NULL
        CHECK (employmentType IN (N'INTERNAL', N'RENTING')), -- TODO: diagram doesn't formally enumerate this attribute; CLAUDE.md confirms two categories (internal team trainer vs. external renting trainer) but not the exact literal values — confirm with the team
    hourlyRate        DECIMAL(10,2) NOT NULL,
    specialization    NVARCHAR(50) NULL, -- nullable: not every trainer has a recorded specialization
    status            NVARCHAR(20) NOT NULL
        CHECK (status IN (N'ACTIVE', N'INACTIVE', N'ON_LEAVE'))
);

CREATE TABLE WorkoutTypes (
    workout_type_id     INT NOT NULL PRIMARY KEY,
    name                NVARCHAR(50) NOT NULL,
    maxDurationMinutes  INT NOT NULL,
    maxCapacity         INT NOT NULL,
    difficultyLevel     NVARCHAR(50) NOT NULL, -- TODO: diagram types this as String with no named enumeration; not converted to a CHECK constraint until allowed values are confirmed
    status              NVARCHAR(20) NOT NULL
        CHECK (status IN (N'ACTIVE', N'UNDER_MAINTENANCE', N'INACTIVE'))
);

CREATE TABLE Rooms (
    room_id      INT NOT NULL PRIMARY KEY,
    roomName     NVARCHAR(50) NOT NULL,
    maxCapacity  INT NOT NULL,
    status       NVARCHAR(20) NOT NULL
        CHECK (status IN (N'ACTIVE', N'UNDER_MAINTENANCE', N'INACTIVE'))
);

CREATE TABLE StudioManagers (
    studio_manager_id  INT NOT NULL PRIMARY KEY,
    username           NVARCHAR(50) NOT NULL,
    password           NVARCHAR(50) NOT NULL,
    fullName           NVARCHAR(50) NOT NULL,
    email              NVARCHAR(50) NOT NULL,
    phone              NVARCHAR(50) NOT NULL,
    status             NVARCHAR(20) NOT NULL
        CHECK (status IN (N'ACTIVE', N'INACTIVE', N'ON_LEAVE'))
);

-- ============================================================================
-- 2. Entities with FK references to base entities
-- ============================================================================

CREATE TABLE Memberships (
    membership_id        INT NOT NULL PRIMARY KEY,
    trainee_id            INT NOT NULL,
    purchaseDate          DATE NOT NULL,
    planName              NVARCHAR(50) NOT NULL,
    planType              NVARCHAR(50) NOT NULL, -- TODO: diagram types this as String with no named enumeration; not converted to a CHECK constraint until allowed values are confirmed
    price                 DECIMAL(10,2) NOT NULL,
    allowedEntries        INT NOT NULL,
    validityDays          INT NOT NULL,
    startDate             DATE NOT NULL,
    endDate               DATE NOT NULL,
    remainingEntries      INT NOT NULL,
    status                NVARCHAR(20) NOT NULL
        CHECK (status IN (N'ACTIVE', N'FROZEN', N'EXPIRED', N'SUSPENDED')),
    CONSTRAINT FK_Memberships_Trainees
        FOREIGN KEY (trainee_id) REFERENCES Trainees(trainee_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE HealthDeclarations (
    health_declaration_id  INT NOT NULL PRIMARY KEY,
    trainee_id              INT NOT NULL,
    declarationDate         DATE NOT NULL,
    status                  NVARCHAR(20) NOT NULL
        CHECK (status IN (N'PENDING', N'APPROVED', N'REJECTED')),
    content                 NVARCHAR(MAX) NOT NULL,
    CONSTRAINT UQ_HealthDeclarations_trainee_id UNIQUE (trainee_id), -- class diagram multiplicity: Trainee 1 : HealthDeclaration 0..1 (at most one declaration per trainee)
    CONSTRAINT FK_HealthDeclarations_Trainees
        FOREIGN KEY (trainee_id) REFERENCES Trainees(trainee_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE Equipment (
    equipment_id       INT NOT NULL PRIMARY KEY,
    room_id             INT NOT NULL,
    barcode             NVARCHAR(50) NOT NULL,
    itemName            NVARCHAR(50) NOT NULL,
    purchaseDate        DATE NOT NULL,
    conditionStatus     NVARCHAR(20) NOT NULL
        CHECK (conditionStatus IN (N'FUNCTIONAL', N'DAMAGED', N'OUT_OF_ORDER', N'RETIRED')),
    CONSTRAINT UQ_Equipment_barcode UNIQUE (barcode), -- diagram assumption: every equipment item is identified by a unique barcode
    CONSTRAINT FK_Equipment_Rooms
        FOREIGN KEY (room_id) REFERENCES Rooms(room_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE TrainingSessions (
    training_session_id  INT NOT NULL PRIMARY KEY,
    room_id                INT NOT NULL,
    workout_type_id        INT NOT NULL,
    trainer_id              INT NOT NULL, -- inferred FK, not explicit in the transcribed diagram; see "TrainingSession -> Trainer" note in CLAUDE.md
    date                    DATE NOT NULL,
    startTime               TIME NOT NULL,
    endTime                 TIME NOT NULL,
    sessionCategory         NVARCHAR(20) NOT NULL
        CHECK (sessionCategory IN (N'GROUP', N'PERSONAL', N'COUPLE')),
    availableSlots          INT NOT NULL,
    CONSTRAINT FK_TrainingSessions_Rooms
        FOREIGN KEY (room_id) REFERENCES Rooms(room_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_TrainingSessions_WorkoutTypes
        FOREIGN KEY (workout_type_id) REFERENCES WorkoutTypes(workout_type_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_TrainingSessions_Trainers
        FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

-- ============================================================================
-- 3. Entities with FK references to level-2 entities
-- ============================================================================

CREATE TABLE Payments (
    payment_id       INT NOT NULL PRIMARY KEY,
    membership_id     INT NOT NULL,
    amount            DECIMAL(10,2) NOT NULL,
    paymentDate       DATE NOT NULL,
    paymentMethod     NVARCHAR(50) NOT NULL, -- TODO: diagram doesn't enumerate allowed values for this attribute; convert to a CHECK-constrained enum once the set of payment methods is confirmed
    paymentStatus     NVARCHAR(50) NOT NULL, -- TODO: same as above — allowed values not confirmed in the source diagram
    receiptNumber     NVARCHAR(50) NULL, -- nullable: not every payment has a receipt number recorded
    CONSTRAINT FK_Payments_Memberships
        FOREIGN KEY (membership_id) REFERENCES Memberships(membership_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE RoomBookings (
    room_booking_id        INT NOT NULL PRIMARY KEY,
    room_id                 INT NOT NULL,
    trainer_id              INT NOT NULL,
    date                    DATE NOT NULL,
    startTime               TIME NOT NULL,
    endTime                 TIME NOT NULL,
    status                  NVARCHAR(20) NOT NULL
        CHECK (status IN (N'CONFIRMED', N'CANCELLED_ON_TIME', N'CANCELLED_LATE', N'RESCHEDULED')),
    cancellationFee         DECIMAL(10,2) NOT NULL DEFAULT 0,
    cancellationTimestamp   DATETIME2 NULL, -- nullable: only set once the booking is actually cancelled
    CONSTRAINT FK_RoomBookings_Rooms
        FOREIGN KEY (room_id) REFERENCES Rooms(room_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_RoomBookings_Trainers
        FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE FaultReports (
    fault_report_id  INT NOT NULL PRIMARY KEY,
    equipment_id      INT NOT NULL,
    trainer_id        INT NOT NULL, -- diagram flags [UNCLEAR] whether Trainee can also report faults; schema follows the FaultReport(Equipment, Trainer) FK pair given in CLAUDE.md's load order
    reportDate        DATE NOT NULL,
    description       NVARCHAR(MAX) NOT NULL,
    status            NVARCHAR(50) NOT NULL, -- TODO: diagram flags this attribute as possibly-enum ("String [UNCLEAR]"); no allowed values given — left as plain string until confirmed
    CONSTRAINT FK_FaultReports_Equipment
        FOREIGN KEY (equipment_id) REFERENCES Equipment(equipment_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_FaultReports_Trainers
        FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

-- ============================================================================
-- 4. Association classes (FK to two or more entities above)
-- ============================================================================

CREATE TABLE Registrations (
    registration_id      INT NOT NULL PRIMARY KEY,
    trainee_id             INT NOT NULL,
    training_session_id    INT NOT NULL,
    registrationDate       DATETIME2 NOT NULL,
    cancellationTimestamp  DATETIME2 NULL, -- nullable: only set once the registration is actually cancelled
    cancellationReason     NVARCHAR(200) NULL, -- nullable: not every cancellation has a reason recorded; see CLAUDE.md transcription-gap note (Class Diagram declares cancel(reason:String) but no matching attribute)
    status                 NVARCHAR(20) NOT NULL
        CHECK (status IN (N'PENDING', N'CONFIRMED', N'ACTIVE', N'COMPLETED', N'WAITLISTED', N'CANCELLED', N'NO_SHOW')),
    CONSTRAINT UQ_Registrations_Trainee_Session UNIQUE (trainee_id, training_session_id), -- diagram assumption: a trainee cannot be registered to the same session occurrence more than once
    CONSTRAINT FK_Registrations_Trainees
        FOREIGN KEY (trainee_id) REFERENCES Trainees(trainee_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_Registrations_TrainingSessions
        FOREIGN KEY (training_session_id) REFERENCES TrainingSessions(training_session_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE WaitlistEntries (
    waitlist_entry_id     INT NOT NULL PRIMARY KEY,
    trainee_id              INT NOT NULL,
    training_session_id     INT NOT NULL,
    position                INT NOT NULL,
    joinTimestamp           DATETIME2 NOT NULL,
    status                  NVARCHAR(20) NOT NULL
        CHECK (status IN (N'CONFIRMED', N'WAITLISTED', N'CANCELLED', N'NO_SHOW')), -- unchanged: backed by WaitlistStatus, not RegistrationStatus (which now also has PENDING/ACTIVE/COMPLETED)
    expirationTimestamp     DATETIME2 NULL, -- nullable: only set once the trainee is offered a spot and the confirmation window starts
    CONSTRAINT FK_WaitlistEntries_Trainees
        FOREIGN KEY (trainee_id) REFERENCES Trainees(trainee_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_WaitlistEntries_TrainingSessions
        FOREIGN KEY (training_session_id) REFERENCES TrainingSessions(training_session_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE WorkoutPerformances (
    workout_performance_id  INT NOT NULL PRIMARY KEY,
    trainee_id                INT NOT NULL,
    training_session_id       INT NOT NULL,
    trainer_id                 INT NOT NULL,
    date                        DATE NOT NULL,
    weight                      DECIMAL(10,2) NOT NULL, -- TODO: diagram types this as "double" (a physical measurement, not money); reused the project's DECIMAL(10,2) default — confirm precision/scale is adequate
    sets                        INT NOT NULL,
    reps                        INT NOT NULL,
    calorieTarget               INT NOT NULL,
    proteinTarget               INT NOT NULL DEFAULT 0,
    dailyStepGoal               INT NOT NULL DEFAULT 0,
    notes                       NVARCHAR(MAX) NULL, -- nullable: a trainer may not always enter notes for a given session
    CONSTRAINT FK_WorkoutPerformances_Trainees
        FOREIGN KEY (trainee_id) REFERENCES Trainees(trainee_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_WorkoutPerformances_TrainingSessions
        FOREIGN KEY (training_session_id) REFERENCES TrainingSessions(training_session_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_WorkoutPerformances_Trainers
        FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);
