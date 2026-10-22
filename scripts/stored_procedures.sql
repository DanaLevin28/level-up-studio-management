-- ============================================================================
-- LEVEL UP Studio Management System (Group 10) — CRUD Stored Procedures
-- ============================================================================
-- Generated from scripts/create_database.sql (table/column definitions) and
-- CLAUDE.md (Primary Key Strategy, No Ad-hoc SQL / DB Operations pattern).
--
-- For every table, five mechanical CRUD procedures:
--   sp_<entity>_create     — inserts a row. Primary key is the first parameter
--                            (@<entity>_id). Does NOT use SCOPE_IDENTITY() —
--                            the PK is assigned in C# before this call.
--   sp_<entity>_update     — updates a row by primary key.
--   sp_<entity>_delete     — deletes a row by primary key.
--   sp_<entity>_get_all    — returns all rows.
--   sp_<entity>_get_by_id  — returns one row by primary key.
--
-- Parameter names match column names exactly. No business logic, no
-- validation, no multi-table joins, no state transitions — mechanical CRUD
-- only, per "No Service Layer" / "DB Operations" in CLAUDE.md.
--
-- Do not run this script by hand against the shared database — it is applied
-- from source control alongside scripts/create_database.sql (see "Git" in
-- CLAUDE.md). Requires create_database.sql to have already been run.
-- ============================================================================

-- ============================================================================
-- Trainees
-- ============================================================================

CREATE PROCEDURE sp_trainee_create
    @trainee_id INT,
    @fullName NVARCHAR(50),
    @phone NVARCHAR(50),
    @email NVARCHAR(50),
    @dateOfBirth DATE,
    @experienceLevel NVARCHAR(50),
    @status NVARCHAR(20),
    @registrationDate DATE,
    @medicalLimitations NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Trainees (trainee_id, fullName, phone, email, dateOfBirth, experienceLevel, status, registrationDate, medicalLimitations)
    VALUES (@trainee_id, @fullName, @phone, @email, @dateOfBirth, @experienceLevel, @status, @registrationDate, @medicalLimitations);
END
GO

CREATE PROCEDURE sp_trainee_update
    @trainee_id INT,
    @fullName NVARCHAR(50),
    @phone NVARCHAR(50),
    @email NVARCHAR(50),
    @dateOfBirth DATE,
    @experienceLevel NVARCHAR(50),
    @status NVARCHAR(20),
    @registrationDate DATE,
    @medicalLimitations NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Trainees
    SET fullName = @fullName,
        phone = @phone,
        email = @email,
        dateOfBirth = @dateOfBirth,
        experienceLevel = @experienceLevel,
        status = @status,
        registrationDate = @registrationDate,
        medicalLimitations = @medicalLimitations
    WHERE trainee_id = @trainee_id;
END
GO

CREATE PROCEDURE sp_trainee_delete
    @trainee_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Trainees
    WHERE trainee_id = @trainee_id;
END
GO

CREATE PROCEDURE sp_trainee_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT trainee_id, fullName, phone, email, dateOfBirth, experienceLevel, status, registrationDate, medicalLimitations
    FROM Trainees;
END
GO

CREATE PROCEDURE sp_trainee_get_by_id
    @trainee_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT trainee_id, fullName, phone, email, dateOfBirth, experienceLevel, status, registrationDate, medicalLimitations
    FROM Trainees
    WHERE trainee_id = @trainee_id;
END
GO

-- ============================================================================
-- Trainers
-- ============================================================================

CREATE PROCEDURE sp_trainer_create
    @trainer_id INT,
    @fullName NVARCHAR(50),
    @phone NVARCHAR(50),
    @email NVARCHAR(50),
    @employmentType NVARCHAR(20),
    @hourlyRate DECIMAL(10,2),
    @specialization NVARCHAR(50),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Trainers (trainer_id, fullName, phone, email, employmentType, hourlyRate, specialization, status)
    VALUES (@trainer_id, @fullName, @phone, @email, @employmentType, @hourlyRate, @specialization, @status);
END
GO

CREATE PROCEDURE sp_trainer_update
    @trainer_id INT,
    @fullName NVARCHAR(50),
    @phone NVARCHAR(50),
    @email NVARCHAR(50),
    @employmentType NVARCHAR(20),
    @hourlyRate DECIMAL(10,2),
    @specialization NVARCHAR(50),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Trainers
    SET fullName = @fullName,
        phone = @phone,
        email = @email,
        employmentType = @employmentType,
        hourlyRate = @hourlyRate,
        specialization = @specialization,
        status = @status
    WHERE trainer_id = @trainer_id;
END
GO

CREATE PROCEDURE sp_trainer_delete
    @trainer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Trainers
    WHERE trainer_id = @trainer_id;
END
GO

CREATE PROCEDURE sp_trainer_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT trainer_id, fullName, phone, email, employmentType, hourlyRate, specialization, status
    FROM Trainers;
END
GO

CREATE PROCEDURE sp_trainer_get_by_id
    @trainer_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT trainer_id, fullName, phone, email, employmentType, hourlyRate, specialization, status
    FROM Trainers
    WHERE trainer_id = @trainer_id;
END
GO

-- ============================================================================
-- WorkoutTypes
-- ============================================================================

CREATE PROCEDURE sp_workout_type_create
    @workout_type_id INT,
    @name NVARCHAR(50),
    @maxDurationMinutes INT,
    @maxCapacity INT,
    @difficultyLevel NVARCHAR(50),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO WorkoutTypes (workout_type_id, name, maxDurationMinutes, maxCapacity, difficultyLevel, status)
    VALUES (@workout_type_id, @name, @maxDurationMinutes, @maxCapacity, @difficultyLevel, @status);
END
GO

CREATE PROCEDURE sp_workout_type_update
    @workout_type_id INT,
    @name NVARCHAR(50),
    @maxDurationMinutes INT,
    @maxCapacity INT,
    @difficultyLevel NVARCHAR(50),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE WorkoutTypes
    SET name = @name,
        maxDurationMinutes = @maxDurationMinutes,
        maxCapacity = @maxCapacity,
        difficultyLevel = @difficultyLevel,
        status = @status
    WHERE workout_type_id = @workout_type_id;
END
GO

CREATE PROCEDURE sp_workout_type_delete
    @workout_type_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM WorkoutTypes
    WHERE workout_type_id = @workout_type_id;
END
GO

CREATE PROCEDURE sp_workout_type_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT workout_type_id, name, maxDurationMinutes, maxCapacity, difficultyLevel, status
    FROM WorkoutTypes;
END
GO

CREATE PROCEDURE sp_workout_type_get_by_id
    @workout_type_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT workout_type_id, name, maxDurationMinutes, maxCapacity, difficultyLevel, status
    FROM WorkoutTypes
    WHERE workout_type_id = @workout_type_id;
END
GO

-- ============================================================================
-- Rooms
-- ============================================================================

CREATE PROCEDURE sp_room_create
    @room_id INT,
    @roomName NVARCHAR(50),
    @maxCapacity INT,
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Rooms (room_id, roomName, maxCapacity, status)
    VALUES (@room_id, @roomName, @maxCapacity, @status);
END
GO

CREATE PROCEDURE sp_room_update
    @room_id INT,
    @roomName NVARCHAR(50),
    @maxCapacity INT,
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Rooms
    SET roomName = @roomName,
        maxCapacity = @maxCapacity,
        status = @status
    WHERE room_id = @room_id;
END
GO

CREATE PROCEDURE sp_room_delete
    @room_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Rooms
    WHERE room_id = @room_id;
END
GO

CREATE PROCEDURE sp_room_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT room_id, roomName, maxCapacity, status
    FROM Rooms;
END
GO

CREATE PROCEDURE sp_room_get_by_id
    @room_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT room_id, roomName, maxCapacity, status
    FROM Rooms
    WHERE room_id = @room_id;
END
GO

-- ============================================================================
-- StudioManagers
-- ============================================================================

CREATE PROCEDURE sp_studio_manager_create
    @studio_manager_id INT,
    @username NVARCHAR(50),
    @password NVARCHAR(50),
    @fullName NVARCHAR(50),
    @email NVARCHAR(50),
    @phone NVARCHAR(50),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO StudioManagers (studio_manager_id, username, password, fullName, email, phone, status)
    VALUES (@studio_manager_id, @username, @password, @fullName, @email, @phone, @status);
END
GO

CREATE PROCEDURE sp_studio_manager_update
    @studio_manager_id INT,
    @username NVARCHAR(50),
    @password NVARCHAR(50),
    @fullName NVARCHAR(50),
    @email NVARCHAR(50),
    @phone NVARCHAR(50),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE StudioManagers
    SET username = @username,
        password = @password,
        fullName = @fullName,
        email = @email,
        phone = @phone,
        status = @status
    WHERE studio_manager_id = @studio_manager_id;
END
GO

CREATE PROCEDURE sp_studio_manager_delete
    @studio_manager_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM StudioManagers
    WHERE studio_manager_id = @studio_manager_id;
END
GO

CREATE PROCEDURE sp_studio_manager_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT studio_manager_id, username, password, fullName, email, phone, status
    FROM StudioManagers;
END
GO

-- ============================================================================
-- Memberships
-- ============================================================================

CREATE PROCEDURE sp_membership_create
    @membership_id INT,
    @trainee_id INT,
    @purchaseDate DATE,
    @planName NVARCHAR(50),
    @planType NVARCHAR(50),
    @price DECIMAL(10,2),
    @allowedEntries INT,
    @validityDays INT,
    @startDate DATE,
    @endDate DATE,
    @remainingEntries INT,
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Memberships (membership_id, trainee_id, purchaseDate, planName, planType, price, allowedEntries, validityDays, startDate, endDate, remainingEntries, status)
    VALUES (@membership_id, @trainee_id, @purchaseDate, @planName, @planType, @price, @allowedEntries, @validityDays, @startDate, @endDate, @remainingEntries, @status);
END
GO

CREATE PROCEDURE sp_membership_update
    @membership_id INT,
    @trainee_id INT,
    @purchaseDate DATE,
    @planName NVARCHAR(50),
    @planType NVARCHAR(50),
    @price DECIMAL(10,2),
    @allowedEntries INT,
    @validityDays INT,
    @startDate DATE,
    @endDate DATE,
    @remainingEntries INT,
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Memberships
    SET trainee_id = @trainee_id,
        purchaseDate = @purchaseDate,
        planName = @planName,
        planType = @planType,
        price = @price,
        allowedEntries = @allowedEntries,
        validityDays = @validityDays,
        startDate = @startDate,
        endDate = @endDate,
        remainingEntries = @remainingEntries,
        status = @status
    WHERE membership_id = @membership_id;
END
GO

CREATE PROCEDURE sp_membership_delete
    @membership_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Memberships
    WHERE membership_id = @membership_id;
END
GO

CREATE PROCEDURE sp_membership_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT membership_id, trainee_id, purchaseDate, planName, planType, price, allowedEntries, validityDays, startDate, endDate, remainingEntries, status
    FROM Memberships;
END
GO

CREATE PROCEDURE sp_membership_get_by_id
    @membership_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT membership_id, trainee_id, purchaseDate, planName, planType, price, allowedEntries, validityDays, startDate, endDate, remainingEntries, status
    FROM Memberships
    WHERE membership_id = @membership_id;
END
GO

-- ============================================================================
-- HealthDeclarations
-- ============================================================================

CREATE PROCEDURE sp_health_declaration_create
    @health_declaration_id INT,
    @trainee_id INT,
    @declarationDate DATE,
    @status NVARCHAR(20),
    @content NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO HealthDeclarations (health_declaration_id, trainee_id, declarationDate, status, content)
    VALUES (@health_declaration_id, @trainee_id, @declarationDate, @status, @content);
END
GO

CREATE PROCEDURE sp_health_declaration_update
    @health_declaration_id INT,
    @trainee_id INT,
    @declarationDate DATE,
    @status NVARCHAR(20),
    @content NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE HealthDeclarations
    SET trainee_id = @trainee_id,
        declarationDate = @declarationDate,
        status = @status,
        content = @content
    WHERE health_declaration_id = @health_declaration_id;
END
GO

CREATE PROCEDURE sp_health_declaration_delete
    @health_declaration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM HealthDeclarations
    WHERE health_declaration_id = @health_declaration_id;
END
GO

CREATE PROCEDURE sp_health_declaration_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT health_declaration_id, trainee_id, declarationDate, status, content
    FROM HealthDeclarations;
END
GO

CREATE PROCEDURE sp_health_declaration_get_by_id
    @health_declaration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT health_declaration_id, trainee_id, declarationDate, status, content
    FROM HealthDeclarations
    WHERE health_declaration_id = @health_declaration_id;
END
GO

-- ============================================================================
-- Equipment
-- ============================================================================

CREATE PROCEDURE sp_equipment_create
    @equipment_id INT,
    @room_id INT,
    @barcode NVARCHAR(50),
    @itemName NVARCHAR(50),
    @purchaseDate DATE,
    @conditionStatus NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Equipment (equipment_id, room_id, barcode, itemName, purchaseDate, conditionStatus)
    VALUES (@equipment_id, @room_id, @barcode, @itemName, @purchaseDate, @conditionStatus);
END
GO

CREATE PROCEDURE sp_equipment_update
    @equipment_id INT,
    @room_id INT,
    @barcode NVARCHAR(50),
    @itemName NVARCHAR(50),
    @purchaseDate DATE,
    @conditionStatus NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Equipment
    SET room_id = @room_id,
        barcode = @barcode,
        itemName = @itemName,
        purchaseDate = @purchaseDate,
        conditionStatus = @conditionStatus
    WHERE equipment_id = @equipment_id;
END
GO

CREATE PROCEDURE sp_equipment_delete
    @equipment_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Equipment
    WHERE equipment_id = @equipment_id;
END
GO

CREATE PROCEDURE sp_equipment_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT equipment_id, room_id, barcode, itemName, purchaseDate, conditionStatus
    FROM Equipment;
END
GO

CREATE PROCEDURE sp_equipment_get_by_id
    @equipment_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT equipment_id, room_id, barcode, itemName, purchaseDate, conditionStatus
    FROM Equipment
    WHERE equipment_id = @equipment_id;
END
GO

-- ============================================================================
-- TrainingSessions
-- ============================================================================

CREATE PROCEDURE sp_training_session_create
    @training_session_id INT,
    @room_id INT,
    @workout_type_id INT,
    @trainer_id INT,
    @date DATE,
    @startTime TIME,
    @endTime TIME,
    @sessionCategory NVARCHAR(20),
    @availableSlots INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO TrainingSessions (training_session_id, room_id, workout_type_id, trainer_id, date, startTime, endTime, sessionCategory, availableSlots)
    VALUES (@training_session_id, @room_id, @workout_type_id, @trainer_id, @date, @startTime, @endTime, @sessionCategory, @availableSlots);
END
GO

CREATE PROCEDURE sp_training_session_update
    @training_session_id INT,
    @room_id INT,
    @workout_type_id INT,
    @trainer_id INT,
    @date DATE,
    @startTime TIME,
    @endTime TIME,
    @sessionCategory NVARCHAR(20),
    @availableSlots INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TrainingSessions
    SET room_id = @room_id,
        workout_type_id = @workout_type_id,
        trainer_id = @trainer_id,
        date = @date,
        startTime = @startTime,
        endTime = @endTime,
        sessionCategory = @sessionCategory,
        availableSlots = @availableSlots
    WHERE training_session_id = @training_session_id;
END
GO

CREATE PROCEDURE sp_training_session_delete
    @training_session_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM TrainingSessions
    WHERE training_session_id = @training_session_id;
END
GO

CREATE PROCEDURE sp_training_session_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT training_session_id, room_id, workout_type_id, trainer_id, date, startTime, endTime, sessionCategory, availableSlots
    FROM TrainingSessions;
END
GO

CREATE PROCEDURE sp_training_session_get_by_id
    @training_session_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT training_session_id, room_id, workout_type_id, trainer_id, date, startTime, endTime, sessionCategory, availableSlots
    FROM TrainingSessions
    WHERE training_session_id = @training_session_id;
END
GO

-- ============================================================================
-- Payments
-- ============================================================================

CREATE PROCEDURE sp_payment_create
    @payment_id INT,
    @membership_id INT,
    @amount DECIMAL(10,2),
    @paymentDate DATE,
    @paymentMethod NVARCHAR(50),
    @paymentStatus NVARCHAR(50),
    @receiptNumber NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Payments (payment_id, membership_id, amount, paymentDate, paymentMethod, paymentStatus, receiptNumber)
    VALUES (@payment_id, @membership_id, @amount, @paymentDate, @paymentMethod, @paymentStatus, @receiptNumber);
END
GO

CREATE PROCEDURE sp_payment_update
    @payment_id INT,
    @membership_id INT,
    @amount DECIMAL(10,2),
    @paymentDate DATE,
    @paymentMethod NVARCHAR(50),
    @paymentStatus NVARCHAR(50),
    @receiptNumber NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Payments
    SET membership_id = @membership_id,
        amount = @amount,
        paymentDate = @paymentDate,
        paymentMethod = @paymentMethod,
        paymentStatus = @paymentStatus,
        receiptNumber = @receiptNumber
    WHERE payment_id = @payment_id;
END
GO

CREATE PROCEDURE sp_payment_delete
    @payment_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Payments
    WHERE payment_id = @payment_id;
END
GO

CREATE PROCEDURE sp_payment_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT payment_id, membership_id, amount, paymentDate, paymentMethod, paymentStatus, receiptNumber
    FROM Payments;
END
GO

CREATE PROCEDURE sp_payment_get_by_id
    @payment_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT payment_id, membership_id, amount, paymentDate, paymentMethod, paymentStatus, receiptNumber
    FROM Payments
    WHERE payment_id = @payment_id;
END
GO

-- ============================================================================
-- RoomBookings
-- ============================================================================

CREATE PROCEDURE sp_room_booking_create
    @room_booking_id INT,
    @room_id INT,
    @trainer_id INT,
    @date DATE,
    @startTime TIME,
    @endTime TIME,
    @status NVARCHAR(20),
    @cancellationFee DECIMAL(10,2),
    @cancellationTimestamp DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO RoomBookings (room_booking_id, room_id, trainer_id, date, startTime, endTime, status, cancellationFee, cancellationTimestamp)
    VALUES (@room_booking_id, @room_id, @trainer_id, @date, @startTime, @endTime, @status, @cancellationFee, @cancellationTimestamp);
END
GO

CREATE PROCEDURE sp_room_booking_update
    @room_booking_id INT,
    @room_id INT,
    @trainer_id INT,
    @date DATE,
    @startTime TIME,
    @endTime TIME,
    @status NVARCHAR(20),
    @cancellationFee DECIMAL(10,2),
    @cancellationTimestamp DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RoomBookings
    SET room_id = @room_id,
        trainer_id = @trainer_id,
        date = @date,
        startTime = @startTime,
        endTime = @endTime,
        status = @status,
        cancellationFee = @cancellationFee,
        cancellationTimestamp = @cancellationTimestamp
    WHERE room_booking_id = @room_booking_id;
END
GO

CREATE PROCEDURE sp_room_booking_delete
    @room_booking_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM RoomBookings
    WHERE room_booking_id = @room_booking_id;
END
GO

CREATE PROCEDURE sp_room_booking_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT room_booking_id, room_id, trainer_id, date, startTime, endTime, status, cancellationFee, cancellationTimestamp
    FROM RoomBookings;
END
GO

CREATE PROCEDURE sp_room_booking_get_by_id
    @room_booking_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT room_booking_id, room_id, trainer_id, date, startTime, endTime, status, cancellationFee, cancellationTimestamp
    FROM RoomBookings
    WHERE room_booking_id = @room_booking_id;
END
GO

-- ============================================================================
-- FaultReports
-- ============================================================================

CREATE PROCEDURE sp_fault_report_create
    @fault_report_id INT,
    @equipment_id INT,
    @trainer_id INT,
    @reportDate DATE,
    @description NVARCHAR(MAX),
    @status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO FaultReports (fault_report_id, equipment_id, trainer_id, reportDate, description, status)
    VALUES (@fault_report_id, @equipment_id, @trainer_id, @reportDate, @description, @status);
END
GO

CREATE PROCEDURE sp_fault_report_update
    @fault_report_id INT,
    @equipment_id INT,
    @trainer_id INT,
    @reportDate DATE,
    @description NVARCHAR(MAX),
    @status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE FaultReports
    SET equipment_id = @equipment_id,
        trainer_id = @trainer_id,
        reportDate = @reportDate,
        description = @description,
        status = @status
    WHERE fault_report_id = @fault_report_id;
END
GO

CREATE PROCEDURE sp_fault_report_delete
    @fault_report_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM FaultReports
    WHERE fault_report_id = @fault_report_id;
END
GO

CREATE PROCEDURE sp_fault_report_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT fault_report_id, equipment_id, trainer_id, reportDate, description, status
    FROM FaultReports;
END
GO

CREATE PROCEDURE sp_fault_report_get_by_id
    @fault_report_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT fault_report_id, equipment_id, trainer_id, reportDate, description, status
    FROM FaultReports
    WHERE fault_report_id = @fault_report_id;
END
GO

-- ============================================================================
-- Registrations
-- ============================================================================

CREATE PROCEDURE sp_registration_create
    @registration_id INT,
    @trainee_id INT,
    @training_session_id INT,
    @registrationDate DATETIME2,
    @cancellationTimestamp DATETIME2,
    @cancellationReason NVARCHAR(200),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Registrations (registration_id, trainee_id, training_session_id, registrationDate, cancellationTimestamp, cancellationReason, status)
    VALUES (@registration_id, @trainee_id, @training_session_id, @registrationDate, @cancellationTimestamp, @cancellationReason, @status);
END
GO

CREATE PROCEDURE sp_registration_update
    @registration_id INT,
    @trainee_id INT,
    @training_session_id INT,
    @registrationDate DATETIME2,
    @cancellationTimestamp DATETIME2,
    @cancellationReason NVARCHAR(200),
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Registrations
    SET trainee_id = @trainee_id,
        training_session_id = @training_session_id,
        registrationDate = @registrationDate,
        cancellationTimestamp = @cancellationTimestamp,
        cancellationReason = @cancellationReason,
        status = @status
    WHERE registration_id = @registration_id;
END
GO

CREATE PROCEDURE sp_registration_delete
    @registration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Registrations
    WHERE registration_id = @registration_id;
END
GO

CREATE PROCEDURE sp_registration_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT registration_id, trainee_id, training_session_id, registrationDate, cancellationTimestamp, cancellationReason, status
    FROM Registrations;
END
GO

CREATE PROCEDURE sp_registration_get_by_id
    @registration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT registration_id, trainee_id, training_session_id, registrationDate, cancellationTimestamp, cancellationReason, status
    FROM Registrations
    WHERE registration_id = @registration_id;
END
GO

-- ============================================================================
-- WaitlistEntries
-- ============================================================================

CREATE PROCEDURE sp_waitlist_entry_create
    @waitlist_entry_id INT,
    @trainee_id INT,
    @training_session_id INT,
    @position INT,
    @joinTimestamp DATETIME2,
    @status NVARCHAR(20),
    @expirationTimestamp DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO WaitlistEntries (waitlist_entry_id, trainee_id, training_session_id, position, joinTimestamp, status, expirationTimestamp)
    VALUES (@waitlist_entry_id, @trainee_id, @training_session_id, @position, @joinTimestamp, @status, @expirationTimestamp);
END
GO

CREATE PROCEDURE sp_waitlist_entry_update
    @waitlist_entry_id INT,
    @trainee_id INT,
    @training_session_id INT,
    @position INT,
    @joinTimestamp DATETIME2,
    @status NVARCHAR(20),
    @expirationTimestamp DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE WaitlistEntries
    SET trainee_id = @trainee_id,
        training_session_id = @training_session_id,
        position = @position,
        joinTimestamp = @joinTimestamp,
        status = @status,
        expirationTimestamp = @expirationTimestamp
    WHERE waitlist_entry_id = @waitlist_entry_id;
END
GO

CREATE PROCEDURE sp_waitlist_entry_delete
    @waitlist_entry_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM WaitlistEntries
    WHERE waitlist_entry_id = @waitlist_entry_id;
END
GO

CREATE PROCEDURE sp_waitlist_entry_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT waitlist_entry_id, trainee_id, training_session_id, position, joinTimestamp, status, expirationTimestamp
    FROM WaitlistEntries;
END
GO

CREATE PROCEDURE sp_waitlist_entry_get_by_id
    @waitlist_entry_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT waitlist_entry_id, trainee_id, training_session_id, position, joinTimestamp, status, expirationTimestamp
    FROM WaitlistEntries
    WHERE waitlist_entry_id = @waitlist_entry_id;
END
GO

-- ============================================================================
-- WorkoutPerformances
-- ============================================================================

CREATE PROCEDURE sp_workout_performance_create
    @workout_performance_id INT,
    @trainee_id INT,
    @training_session_id INT,
    @trainer_id INT,
    @date DATE,
    @weight DECIMAL(10,2),
    @sets INT,
    @reps INT,
    @calorieTarget INT,
    @proteinTarget INT,
    @dailyStepGoal INT,
    @notes NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO WorkoutPerformances (workout_performance_id, trainee_id, training_session_id, trainer_id, date, weight, sets, reps, calorieTarget, proteinTarget, dailyStepGoal, notes)
    VALUES (@workout_performance_id, @trainee_id, @training_session_id, @trainer_id, @date, @weight, @sets, @reps, @calorieTarget, @proteinTarget, @dailyStepGoal, @notes);
END
GO

CREATE PROCEDURE sp_workout_performance_update
    @workout_performance_id INT,
    @trainee_id INT,
    @training_session_id INT,
    @trainer_id INT,
    @date DATE,
    @weight DECIMAL(10,2),
    @sets INT,
    @reps INT,
    @calorieTarget INT,
    @proteinTarget INT,
    @dailyStepGoal INT,
    @notes NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE WorkoutPerformances
    SET trainee_id = @trainee_id,
        training_session_id = @training_session_id,
        trainer_id = @trainer_id,
        date = @date,
        weight = @weight,
        sets = @sets,
        reps = @reps,
        calorieTarget = @calorieTarget,
        proteinTarget = @proteinTarget,
        dailyStepGoal = @dailyStepGoal,
        notes = @notes
    WHERE workout_performance_id = @workout_performance_id;
END
GO

CREATE PROCEDURE sp_workout_performance_delete
    @workout_performance_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM WorkoutPerformances
    WHERE workout_performance_id = @workout_performance_id;
END
GO

CREATE PROCEDURE sp_workout_performance_get_all
AS
BEGIN
    SET NOCOUNT ON;
    SELECT workout_performance_id, trainee_id, training_session_id, trainer_id, date, weight, sets, reps, calorieTarget, proteinTarget, dailyStepGoal, notes
    FROM WorkoutPerformances;
END
GO

CREATE PROCEDURE sp_workout_performance_get_by_id
    @workout_performance_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT workout_performance_id, trainee_id, training_session_id, trainer_id, date, weight, sets, reps, calorieTarget, proteinTarget, dailyStepGoal, notes
    FROM WorkoutPerformances
    WHERE workout_performance_id = @workout_performance_id;
END
GO

-- ============================================================================
-- Reports (LESSON_STEPS.md step 8)
-- ============================================================================
-- Read-only, aggregated result sets for report panels — no Save/Update/Delete
-- counterpart, per "no additional forms/dialogs" and "no service layer" but
-- with JOIN/GROUP BY allowed here unlike the mechanical CRUD procedures above.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- sp_renters_billing_report — UC07 "Generate Renters Billing Report"
-- (docs/00e-use-cases.md UC07; FR-12/FR-24/FR-25 in docs/00-requirements.md)
--
-- One result set: one row per RoomBooking for @trainer_id within
-- [@start_date, @end_date], followed by one synthetic totals row
-- ([חדר] = N'סה"כ') carrying the period's summary figures. Detail rows leave
-- the two totals columns NULL; the totals row leaves the per-booking columns
-- NULL. Single result set so it binds directly to one DataGridView (step 8.3
-- describes a single grid, not a grid + separate summary control).
--
-- Per-row charge: duration (hours) x Trainer.hourlyRate; halved when
-- status = CANCELLED_LATE (FR-24's 50% late-cancellation fee). The 12-hour
-- late-cancellation test itself is assumed already applied when the booking's
-- status was set to CANCELLED_LATE elsewhere (sp_room_booking_update) — this
-- report does not re-derive it from a cancellation timestamp.
--
-- FR-25's "cancellation timestamp" output column is now sourced from
-- RoomBookings.cancellationTimestamp (added when RoomBooking was aligned to
-- the class diagram) — no longer a gap.
--
-- No GROUP BY: @trainer_id is a required single-trainer filter (matches the
-- UC — "the monthly billing report for the renter", singular), so the WHERE
-- clause already reduces the totals query to one implicit group.
-- ----------------------------------------------------------------------------
CREATE PROCEDURE sp_renters_billing_report
    @trainer_id INT,
    @start_date DATE,
    @end_date DATE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH BookingCharges AS (
        SELECT
            rb.date,
            rb.startTime,
            rb.endTime,
            r.roomName,
            rb.status,
            rb.cancellationTimestamp,
            CAST(
                DATEDIFF(MINUTE, rb.startTime, rb.endTime) / 60.0 * t.hourlyRate
                * CASE WHEN rb.status = N'CANCELLED_LATE' THEN 0.5 ELSE 1.0 END
            AS DECIMAL(10,2)) AS amountCharged
        FROM RoomBookings rb
        JOIN Rooms r    ON r.room_id = rb.room_id
        JOIN Trainers t ON t.trainer_id = rb.trainer_id
        WHERE rb.trainer_id = @trainer_id
          AND rb.date BETWEEN @start_date AND @end_date
    )
    SELECT
        date                  AS [תאריך],
        startTime             AS [שעת_התחלה],
        endTime               AS [שעת_סיום],
        roomName              AS [חדר],
        CAST(CASE status
            WHEN N'CONFIRMED'      THEN N'מאושר'
            WHEN N'CANCELLED_ON_TIME' THEN N'התקיים בזמן'
            WHEN N'CANCELLED_LATE' THEN N'בוטל באיחור'
            WHEN N'RESCHEDULED'    THEN N'נדחה'
        END AS NVARCHAR(50))         AS [סטטוס],
        cancellationTimestamp        AS [מועד_ביטול],
        amountCharged                AS [סכום_לחיוב],
        CAST(NULL AS INT)            AS [סך_הכל_אימונים],
        CAST(NULL AS INT)            AS [סך_הכל_ביטולים]
    FROM BookingCharges

    UNION ALL

    SELECT
        CAST(NULL AS DATE),
        CAST(NULL AS TIME),
        CAST(NULL AS TIME),
        N'סה"כ',
        CAST(NULL AS NVARCHAR(50)),
        CAST(NULL AS DATETIME2),
        SUM(amountCharged),
        COUNT(*),
        SUM(CASE WHEN status = N'CANCELLED_LATE' THEN 1 ELSE 0 END)
    FROM BookingCharges;
END
GO

-- ============================================================================
-- Business Process Flows (LESSON_STEPS.md step 9 — orchestrated transactions)
-- ============================================================================
-- Unlike the mechanical CRUD procedures above, a "flow" procedure implements
-- one UC's full main-success-scenario end to end: every guard named in the
-- UC's preconditions / extensions is validated (no write yet) before any
-- write happens, then every write the UC's postconditions require happens
-- together inside one BEGIN TRAN ... COMMIT TRAN, with ROLLBACK on any
-- error. The caller gets every side effect or none of them — nothing is
-- left for the C# layer to sequence or remember.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- sp_register_to_workout — UC02 "Register to Workout"
-- (docs/00e-use-cases.md UC02, MSS steps 4-7; FR-7/FR-17/FR-27 in
-- docs/00-requirements.md)
--
-- Guards, evaluated in this order, before any write:
--   51001  Trainee does not exist
--   51002  Trainee exists but is not active (status <> ACTIVE)
--   51003  TrainingSession does not exist
--   51004  Trainee already holds a CONFIRMED/WAITLISTED registration for
--          this exact session occurrence (diagram assumption in
--          docs/design/class-diagram.md — a trainee cannot be registered
--          to the same occurrence twice; also enforced redundantly by
--          UQ_Registrations_Trainee_Session, but checked explicitly first
--          for a specific, catchable error instead of a generic
--          constraint-violation exception)
--   51005  No membership belonging to the trainee is ACTIVE, unexpired
--          (endDate >= today), and has remainingEntries > 0
--          (UC02 step 4 / extension 4.a, "Include: Check Membership
--          Validity")
--   51006  TrainingSession.availableSlots < 1
--          (UC02 step 5 / extension 5.a, "Workout is Full" — extends to
--          Join Waitlist / UC06, a separate SP and out of scope here;
--          this procedure only ever produces a CONFIRMED registration)
--
-- Writes, once every guard above has passed, in this order:
--   a. INSERT Registrations   (status = CONFIRMED, cancellationTimestamp = NULL)
--   b. UPDATE TrainingSessions.availableSlots -= 1
--   c. UPDATE Memberships.remainingEntries    -= 1
--
-- @out_membership_id OUTPUT returns which Membership row was validated and
-- decremented. The caller (Trainee.registerToWorkout in C#) needs this to
-- mirror the write onto the correct in-memory Membership object without
-- re-deriving "latest endDate" selection logic a second time in C# — one
-- place decides which membership is used, and that is this procedure.
--
-- Deliberately NOT done here: UC02 step 8's confirmation notification.
-- Per NFR-10 that is a call to an external notification gateway, not a DB
-- write (same treatment as PaymentGateway elsewhere in this file) — the
-- application layer sends it after this procedure returns successfully.
--
-- Concurrency: the Trainees / TrainingSessions / Memberships rows read for
-- guard-checking are locked WITH (UPDLOCK, HOLDLOCK) from the guard check
-- through COMMIT/ROLLBACK, so two concurrent calls racing for the last
-- open slot (or the last remaining entry on one membership) cannot both
-- read "1 left" and both succeed (NFR-2/NFR-6: 200 concurrent users).
-- Every call acquires these three locks in the same fixed order (Trainee,
-- then TrainingSession, then Membership), so concurrent calls cannot
-- deadlock against each other.
-- ----------------------------------------------------------------------------
CREATE PROCEDURE sp_register_to_workout
    @registration_id     INT,
    @trainee_id           INT,
    @training_session_id  INT,
    @registrationDate     DATETIME2,
    @out_membership_id    INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        DECLARE @traineeStatus    NVARCHAR(20);
        DECLARE @availableSlots   INT;
        DECLARE @membership_id    INT;
        DECLARE @membershipStatus NVARCHAR(20);
        DECLARE @endDate          DATE;
        DECLARE @remainingEntries INT;

        -- Guards 51001/51002: trainee exists and is active
        SELECT @traineeStatus = status
        FROM Trainees WITH (UPDLOCK, HOLDLOCK)
        WHERE trainee_id = @trainee_id;

        IF @traineeStatus IS NULL
            THROW 51001, N'המתאמן/ת המבוקש/ת אינו/ה קיים/ת במערכת.', 1;

        IF @traineeStatus <> N'ACTIVE'
            THROW 51002, N'לא ניתן לרשום מתאמן/ת לא פעיל/ה לאימון.', 1;

        -- Guard 51003: training session exists (lock its slot count for the rest of this transaction)
        SELECT @availableSlots = availableSlots
        FROM TrainingSessions WITH (UPDLOCK, HOLDLOCK)
        WHERE training_session_id = @training_session_id;

        IF @availableSlots IS NULL
            THROW 51003, N'האימון המבוקש אינו קיים במערכת.', 1;

        -- Guard 51004: not already registered to this occurrence
        IF EXISTS (
            SELECT 1 FROM Registrations
            WHERE trainee_id = @trainee_id
              AND training_session_id = @training_session_id
              AND status IN (N'CONFIRMED', N'WAITLISTED')
        )
            THROW 51004, N'המתאמן/ת כבר רשום/ה לאימון זה.', 1;

        -- Guard 51005: membership validity + remaining entries ("Check Membership Validity")
        SELECT TOP 1
            @membership_id = membership_id,
            @membershipStatus = status,
            @endDate = endDate,
            @remainingEntries = remainingEntries
        FROM Memberships WITH (UPDLOCK, HOLDLOCK)
        WHERE trainee_id = @trainee_id
        ORDER BY endDate DESC;

        IF @membership_id IS NULL
           OR @membershipStatus <> N'ACTIVE'
           OR @endDate < CAST(GETDATE() AS DATE)
           OR @remainingEntries <= 0
            THROW 51005, N'אין למתאמן/ת מנוי בתוקף, או שלא נותרו כניסות במנוי.', 1;

        -- Guard 51006: at least one available slot
        IF @availableSlots < 1
            THROW 51006, N'האימון מלא, אין מקומות פנויים.', 1;

        -- All guards passed — perform every write for this UC atomically
        INSERT INTO Registrations (registration_id, trainee_id, training_session_id, registrationDate, cancellationTimestamp, status)
        VALUES (@registration_id, @trainee_id, @training_session_id, @registrationDate, NULL, N'CONFIRMED');

        UPDATE TrainingSessions
        SET availableSlots = availableSlots - 1
        WHERE training_session_id = @training_session_id;

        UPDATE Memberships
        SET remainingEntries = remainingEntries - 1
        WHERE membership_id = @membership_id;

        SET @out_membership_id = @membership_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- Membership state transitions (docs/design/state-diagram.md — "1. Membership
-- State Diagram", transitions 4-7). Domain-verb procedures, not CRUD: each is
-- called by the matching Membership.<verb>() method in C#, which checks the
-- transition's guard against the in-memory object *before* calling here — a
-- guard failure never reaches the database. These procedures trust that guard
-- and do only the transactional write: the status column, plus the one side
-- effect in another table called for by that transition's entry action in the
-- diagram. First explicit BEGIN TRAN/COMMIT/ROLLBACK usage in this project
-- outside sp_register_to_workout above.
-- ----------------------------------------------------------------------------

-- Transition 4: Active -> FROZEN (trigger freeze, guard [userRequested])
CREATE PROCEDURE sp_membership_freeze
    @membership_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Memberships
        SET status = N'FROZEN'
        WHERE membership_id = @membership_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transition 5: FROZEN -> Active (trigger unfreeze, no guard)
CREATE PROCEDURE sp_membership_unfreeze
    @membership_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Memberships
        SET status = N'ACTIVE'
        WHERE membership_id = @membership_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transition 6: Valid Membership (Active or FROZEN) -> SUSPENDED
-- (trigger suspend, guard [paymentFailed] — checked in C# by
-- Membership.suspend(bool paymentFailed), not here).
-- TODO: state-diagram.md's SUSPENDED entry lists `do / checkPayment()` but
-- does not document what that action actually does. No side effect is
-- implemented here until the group decides what it should be.
CREATE PROCEDURE sp_membership_suspend
    @membership_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Memberships
        SET status = N'SUSPENDED'
        WHERE membership_id = @membership_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transition 7: Valid Membership (Active or FROZEN) -> EXPIRED
-- (trigger expire, guard [endDateReached] — checked in C# by
-- Membership.expire() against endDate/DateTime.Today, not here).
-- TODO: state-diagram.md's EXPIRED entry lists `/ archiveMembership()` but
-- does not document what that action actually does. No side effect is
-- implemented here until the group decides what it should be.
CREATE PROCEDURE sp_membership_expire
    @membership_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Memberships
        SET status = N'EXPIRED'
        WHERE membership_id = @membership_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- Registration state transitions (docs/design/state-diagram.md — "2.
-- Registration State Diagram"). Domain-verb procedures, not CRUD: each is
-- called by the matching Registration.<verb>() method in C#, which checks
-- the transition's guard against the in-memory object — and, where the
-- diagram's guard references outside data (membership validity, time until
-- class start), against that data too — before calling here. These
-- procedures trust that guard and do only the transactional write.
-- Transitions 12-14 (purge, after 7 years) are time-triggered, not
-- user-triggered, and have no procedure here — see Registration.purge().
-- ----------------------------------------------------------------------------

-- Transition 5: Pending -> Confirmed (trigger confirm, guard [paymentProcessed])
CREATE PROCEDURE sp_registration_confirm
    @registration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Registrations
        SET status = N'CONFIRMED'
        WHERE registration_id = @registration_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transitions 3/4/9: Pending, Waitlisted, or Confirmed (>12h before class
-- start) -> Cancelled (trigger cancel). Per state-diagram.md's
-- implementation-scoping note, #9 is treated as scoped to Confirmed only,
-- not the group transition off ActiveBooking that the source diagram draws.
CREATE PROCEDURE sp_registration_cancel
    @registration_id INT,
    @cancellationReason NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Registrations
        SET status = N'CANCELLED',
            cancellationTimestamp = SYSDATETIME(),
            cancellationReason = @cancellationReason
        WHERE registration_id = @registration_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transition 10: Confirmed (<=12h before class start) -> Cancelled (trigger
-- cancel, guard [under 12 hours], effect / chargeLateFee).
-- TODO: chargeLateFee's fee amount and target table (a Payment row against
-- the trainee's membership? a dedicated fee table?) are undocumented in
-- state-diagram.md. No side effect is implemented here until the group
-- decides what it should be.
CREATE PROCEDURE sp_registration_late_cancel
    @registration_id INT,
    @cancellationReason NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Registrations
        SET status = N'CANCELLED',
            cancellationTimestamp = SYSDATETIME(),
            cancellationReason = @cancellationReason
        WHERE registration_id = @registration_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transition 7: Confirmed -> Active (trigger startClass, guard [isClassTime])
CREATE PROCEDURE sp_registration_start_class
    @registration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Registrations
        SET status = N'ACTIVE'
        WHERE registration_id = @registration_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transition 8: Confirmed -> No_Show (trigger missClass). Per
-- state-diagram.md's corrected transcription, this fires from Confirmed
-- only — verified against the original Visual Paradigm PDF, not a group
-- transition off ActiveBooking.
CREATE PROCEDURE sp_registration_miss_class
    @registration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Registrations
        SET status = N'NO_SHOW'
        WHERE registration_id = @registration_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

-- Transition 11: Active -> Completed (trigger finishClass, no guard)
CREATE PROCEDURE sp_registration_finish_class
    @registration_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Registrations
        SET status = N'COMPLETED'
        WHERE registration_id = @registration_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO
