-- ============================================================================
-- LEVEL UP Studio Management System (Group 10) — Seed / Test Data
-- ============================================================================
-- Realistic sample data for every table in scripts/create_database.sql, in the
-- same load order (base entities -> FK-referencing entities -> association
-- classes last), per CLAUDE.md.
--
-- Notes:
--   - Uses plain INSERT statements. This is a one-time data-loading script, not
--     application code — the "No ad-hoc SQL" rule in CLAUDE.md governs the C#
--     application's DB access (which goes through stored procedures), not
--     seed/test scripts like this one.
--   - Primary keys are assigned explicitly (no IDENTITY columns), matching the
--     project's Primary Key Strategy — these are the same values the C# entity
--     constructors would have produced via getNextXYZId().
--   - One test StudioManager (admin / admin123) is seeded for the Login screen.
--   - "Today" for relative freshness (ACTIVE vs EXPIRED memberships, past vs
--     upcoming training sessions) is treated as 2026-09-14.
--   - Run this only after scripts/create_database.sql has been executed
--     against an empty levelup_studio database.
-- ============================================================================

-- ============================================================================
-- 1. Base entities
-- ============================================================================

INSERT INTO StudioManagers (studio_manager_id, username, password, fullName, email, phone, status)
VALUES (1, N'admin', N'admin123', N'מנהל הסטודיו', N'user@example.com', N'050-0000000', N'ACTIVE');

INSERT INTO Trainees (trainee_id, fullName, phone, email, dateOfBirth, experienceLevel, status, registrationDate)
VALUES
    (1, N'נועה כהן',        N'050-1234567', N'user@example.com',        '1990-03-14', N'בינוני',  N'ACTIVE',   '2024-01-10'),
    (2, N'איתי לוי',        N'052-2345678', N'user@example.com',          '1985-07-22', N'מתקדם',   N'ACTIVE',   '2024-02-05'),
    (3, N'מיכל ברק',        N'054-3456789', N'user@example.com',   '1998-11-02', N'מתחיל',   N'ACTIVE',   '2024-03-18'),
    (4, N'דניאל אברהם',      N'053-4567890', N'user@example.com',   '1979-05-30', N'מתקדם',   N'ACTIVE',   '2023-11-22'),
    (5, N'שירה מזרחי',      N'050-5678901', N'user@example.com',      '1995-09-09', N'בינוני',  N'ACTIVE',   '2024-04-01'),
    (6, N'עומר פרץ',        N'052-6789012', N'user@example.com',      '1988-01-17', N'בינוני',  N'INACTIVE', '2022-06-14'),
    (7, N'יעל שרון',        N'054-7890123', N'user@example.com',        '2001-12-25', N'מתחיל',   N'ACTIVE',   '2024-05-20'),
    (8, N'רון גולדברג',      N'053-8901234', N'user@example.com',       '1992-04-08', N'מתקדם',   N'ACTIVE',   '2024-06-11');

INSERT INTO Trainers (trainer_id, fullName, phone, email, employmentType, hourlyRate, status)
VALUES
    (1, N'מנהל הסטודיו מלכה',     N'050-9998888', N'user@example.com',        N'INTERNAL', 150.00, N'ACTIVE'),
    (2, N'מאמנת רוזן',        N'052-8887777', N'user@example.com',     N'INTERNAL', 120.00, N'ACTIVE'),
    (3, N'יובל אשכנזי',     N'054-7776666', N'user@example.com', N'INTERNAL', 110.00, N'ACTIVE'),
    (4, N'טל בן-דוד',       N'053-6665555', N'user@example.com',           N'RENTING',  80.00,  N'ACTIVE'),
    (5, N'קרן אזולאי',      N'050-5554444', N'user@example.com',         N'RENTING',  85.00,  N'ACTIVE'),
    (6, N'אלון נחום',       N'052-4443333', N'user@example.com',      N'INTERNAL', 115.00, N'ACTIVE');

INSERT INTO WorkoutTypes (workout_type_id, name, maxDurationMinutes, maxCapacity, difficultyLevel, status)
VALUES
    (1, N'אימון כוח קבוצתי',        60, 10, N'INTERMEDIATE', N'ACTIVE'),
    (2, N'אימון פונקציונלי קבוצתי', 50, 10, N'ADVANCED',     N'ACTIVE'),
    (3, N'פילאטיס קבוצתי',          55, 8,  N'BEGINNER',     N'ACTIVE'),
    (4, N'אימון אישי',              60, 1,  N'INTERMEDIATE', N'ACTIVE'),
    (5, N'אימון זוגי',              60, 2,  N'BEGINNER',     N'ACTIVE');

INSERT INTO Rooms (room_id, roomName, maxCapacity, status)
VALUES
    (1, N'אולם כוח ראשי',              12, N'ACTIVE'),
    (2, N'סטודיו פונקציונלי',          10, N'ACTIVE'),
    (3, N'סטודיו פילאטיס',             8,  N'ACTIVE'),
    (4, N'חדר אימונים אישיים',         2,  N'ACTIVE'),
    (5, N'חדר רב-תכליתי להשכרה',       6,  N'ACTIVE');

-- ============================================================================
-- 2. Entities with FK references to base entities
-- ============================================================================

INSERT INTO Memberships (membership_id, trainee_id, purchaseDate, planName, planType, price, allowedEntries, validityDays, startDate, endDate, remainingEntries, status)
VALUES
    (1,  1, '2026-09-01', N'מנוי חודשי בסיסי',   N'MONTHLY',    250.00, 12, 30,  '2026-09-01', '2026-10-01', 7,  N'ACTIVE'),
    (2,  2, '2026-08-20', N'מנוי חודשי פרימיום', N'MONTHLY',    400.00, 30, 30,  '2026-08-20', '2026-09-19', 15, N'ACTIVE'),
    (3,  3, '2026-06-01', N'כרטיסייה 10 אימונים', N'PUNCH_CARD', 350.00, 10, 90,  '2026-06-01', '2026-08-30', 0,  N'EXPIRED'),
    (4,  4, '2026-01-15', N'כרטיסייה 20 אימונים', N'PUNCH_CARD', 620.00, 20, 180, '2026-01-15', '2026-07-14', 3,  N'EXPIRED'),
    (5,  5, '2026-09-05', N'מנוי חודשי בסיסי',   N'MONTHLY',    250.00, 12, 30,  '2026-09-05', '2026-10-05', 10, N'ACTIVE'),
    (6,  6, '2025-01-01', N'מנוי חודשי בסיסי',   N'MONTHLY',    250.00, 12, 30,  '2025-01-01', '2025-01-31', 0,  N'EXPIRED'),
    (7,  7, '2026-09-01', N'מנוי חודשי פרימיום', N'MONTHLY',    400.00, 30, 30,  '2026-09-01', '2026-10-01', 20, N'SUSPENDED'),
    (8,  8, '2026-07-01', N'כרטיסייה 10 אימונים', N'PUNCH_CARD', 350.00, 10, 90,  '2026-07-01', '2026-09-29', 4,  N'ACTIVE'),
    (9,  1, '2026-06-01', N'מנוי חודשי בסיסי',   N'MONTHLY',    250.00, 12, 30,  '2026-06-01', '2026-07-01', 0,  N'EXPIRED'),
    (10, 4, '2025-09-01', N'מנוי סטודנט',        N'MONTHLY',    180.00, 8,  30,  '2025-09-01', '2025-09-30', 0,  N'SUSPENDED');

INSERT INTO HealthDeclarations (health_declaration_id, trainee_id, declarationDate, status, content)
VALUES
    (1, 1, '2024-01-10', N'APPROVED', N'אין הגבלות ידועות'),
    (2, 2, '2024-02-05', N'APPROVED', N'כאבי גב תחתון בעבר, יש להימנע מהרמות כבדות מדי'),
    (3, 3, '2024-03-18', N'APPROVED', N'אין הגבלות ידועות'),
    (4, 4, '2023-11-22', N'PENDING',  N'ממתין לאישור רופא בעקבות ניתוח ברך'),
    (5, 5, '2024-04-01', N'APPROVED', N'אסתמה קלה, יש להחזיק משאף בקרבת מקום'),
    (6, 7, '2024-05-20', N'APPROVED', N'אין הגבלות ידועות');

INSERT INTO Equipment (equipment_id, room_id, barcode, itemName, purchaseDate, conditionStatus)
VALUES
    (1, 1, N'EQ-1001', N'מוט ברזל אולימפי 20 ק"ג',                     '2022-03-01', N'FUNCTIONAL'),
    (2, 1, N'EQ-1002', N'סט משקולות דמבל 2-24 ק"ג',                    '2022-03-01', N'FUNCTIONAL'),
    (3, 1, N'EQ-1003', N'מכונת סמיט (Smith Machine)',                  '2021-09-15', N'DAMAGED'),
    (4, 2, N'EQ-2001', N'חבל קרב (Battle Rope)',                       '2023-01-20', N'FUNCTIONAL'),
    (5, 2, N'EQ-2002', N'ארגז קפיצה מדורג (Plyo Box)',                 '2023-01-20', N'FUNCTIONAL'),
    (6, 3, N'EQ-3001', N'מזרני פילאטיס',                               '2022-06-10', N'FUNCTIONAL'),
    (7, 3, N'EQ-3002', N'טבעת פילאטיס (Pilates Ring)',                 '2020-05-05', N'RETIRED'),
    (8, 4, N'EQ-4001', N'מכונת פולי צולב (Cable Crossover)',           '2019-11-11', N'OUT_OF_ORDER');

INSERT INTO TrainingSessions (training_session_id, room_id, workout_type_id, trainer_id, date, startTime, endTime, sessionCategory, availableSlots)
VALUES
    (1,  1, 1, 2, '2026-09-15', '08:00', '09:00', N'GROUP',    8),
    (2,  2, 2, 3, '2026-09-15', '18:00', '19:00', N'GROUP',    8),
    (3,  3, 3, 6, '2026-09-16', '09:00', '10:00', N'GROUP',    7),
    (4,  4, 4, 1, '2026-09-15', '07:00', '08:00', N'PERSONAL', 0),
    (5,  4, 4, 2, '2026-09-16', '16:00', '17:00', N'PERSONAL', 0),
    (6,  4, 5, 1, '2026-09-17', '19:00', '20:00', N'COUPLE',   0),
    (7,  1, 1, 2, '2026-09-17', '08:00', '09:00', N'GROUP',    0),
    (8,  2, 2, 3, '2026-09-18', '18:00', '19:00', N'GROUP',    5),
    (9,  3, 3, 6, '2026-09-18', '09:00', '10:00', N'GROUP',    6),
    (10, 4, 4, 1, '2026-09-19', '07:00', '08:00', N'PERSONAL', 1),
    (11, 1, 1, 3, '2026-09-20', '10:00', '11:00', N'GROUP',    7),
    (12, 4, 5, 2, '2026-09-21', '17:00', '18:00', N'COUPLE',   1),
    (13, 1, 1, 2, '2026-09-08', '08:00', '09:00', N'GROUP',    3),
    (14, 4, 4, 1, '2026-09-07', '07:00', '08:00', N'PERSONAL', 0),
    (15, 2, 2, 3, '2026-09-06', '18:00', '19:00', N'GROUP',    4),
    (16, 3, 3, 6, '2026-09-05', '09:00', '10:00', N'GROUP',    5);

-- ============================================================================
-- 3. Entities with FK references to level-2 entities
-- ============================================================================

INSERT INTO Payments (payment_id, membership_id, amount, paymentDate, paymentMethod, paymentStatus, receiptNumber)
VALUES
    (1,  1,  250.00, '2026-09-01', N'אשראי',           N'שולם',        NULL),
    (2,  2,  400.00, '2026-08-20', N'אשראי',           N'שולם',        NULL),
    (3,  3,  350.00, '2026-06-01', N'מזומן',           N'שולם',        NULL),
    (4,  4,  620.00, '2026-01-15', N'העברה בנקאית',    N'שולם',        NULL),
    (5,  5,  250.00, '2026-09-05', N'אשראי',           N'שולם',        NULL),
    (6,  6,  250.00, '2025-01-01', N'מזומן',           N'שולם',        NULL),
    (7,  7,  400.00, '2026-09-01', N'אשראי',           N'ממתין לחיוב', NULL),
    (8,  8,  350.00, '2026-07-01', N'ביט',             N'שולם',        NULL),
    (9,  9,  250.00, '2026-06-01', N'אשראי',           N'שולם',        NULL),
    (10, 10, 180.00, '2025-09-01', N'אשראי',           N'נכשל',        NULL),
    (11, 1,  30.00,  '2026-09-08', N'מזומן',           N'שולם',        NULL),
    (12, 6,  50.00,  '2025-01-15', N'אשראי',           N'הוחזר',       NULL);

INSERT INTO RoomBookings (room_booking_id, room_id, trainer_id, date, startTime, endTime, status, cancellationFee, cancellationTimestamp)
VALUES
    (1,  5, 4, '2026-09-15', '12:00', '13:00', N'CONFIRMED',         0,     NULL),
    (2,  5, 5, '2026-09-15', '14:00', '15:00', N'CONFIRMED',         0,     NULL),
    (3,  5, 4, '2026-09-16', '10:00', '11:00', N'CANCELLED_ON_TIME', 0,     '2026-09-15T09:00:00'),
    (4,  5, 5, '2026-09-16', '16:00', '17:00', N'CANCELLED_LATE',    42.50, '2026-09-16T08:00:00'),
    (5,  5, 4, '2026-09-17', '09:00', '10:00', N'RESCHEDULED',       0,     NULL),
    (6,  4, 5, '2026-09-18', '11:00', '12:00', N'CONFIRMED',         0,     NULL),
    (7,  5, 4, '2026-09-19', '13:00', '14:00', N'CANCELLED_ON_TIME', 0,     '2026-09-18T10:00:00'),
    (8,  5, 5, '2026-09-20', '15:00', '16:00', N'CONFIRMED',         0,     NULL),
    (9,  4, 4, '2026-09-21', '08:00', '09:00', N'CANCELLED_LATE',    40.00, '2026-09-21T04:00:00'),
    (10, 5, 5, '2026-09-22', '12:00', '13:00', N'CONFIRMED',         0,     NULL);

INSERT INTO FaultReports (fault_report_id, equipment_id, trainer_id, reportDate, description, status)
VALUES
    (1, 3, 2, '2026-09-09', N'המכונה משמיעה רעש חריקה בעת הרמה ונתקעת בגובה מסוים',   N'בטיפול'),
    (2, 7, 3, '2023-12-20', N'הטבעת נסדקה ואינה בטוחה לשימוש',                         N'סגור'),
    (3, 8, 1, '2026-07-30', N'הכבל נתקע ולא חוזר למקומו, מסוכן לשימוש',                N'פתוח'),
    (4, 5, 6, '2026-06-15', N'אחד המדרגות בארגז הקפיצה רופפת',                          N'סגור'),
    (5, 1, 2, '2025-11-02', N'ציפוי המוט מתקלף בקצה אחד',                              N'סגור'),
    (6, 3, 1, '2026-09-12', N'תקלה חוזרת לאחר הטיפול הקודם, יש לבדוק שוב',              N'פתוח');

-- ============================================================================
-- 4. Association classes
-- ============================================================================

INSERT INTO Registrations (registration_id, trainee_id, training_session_id, registrationDate, cancellationTimestamp, status)
VALUES
    (1,  1, 1,  '2026-09-10T09:00:00', NULL,                  N'CONFIRMED'),
    (2,  2, 1,  '2026-09-10T10:15:00', NULL,                  N'CONFIRMED'),
    (3,  3, 2,  '2026-09-11T08:00:00', NULL,                  N'CONFIRMED'),
    (4,  5, 2,  '2026-09-11T09:30:00', NULL,                  N'CONFIRMED'),
    (5,  1, 7,  '2026-09-12T07:00:00', NULL,                  N'CONFIRMED'),
    (6,  2, 7,  '2026-09-12T07:05:00', NULL,                  N'WAITLISTED'),
    (7,  7, 3,  '2026-09-12T12:00:00', NULL,                  N'CONFIRMED'),
    (8,  8, 3,  '2026-09-12T12:10:00', '2026-09-13T09:00:00', N'CANCELLED'),
    (9,  4, 8,  '2026-09-13T08:00:00', NULL,                  N'CONFIRMED'),
    (10, 5, 9,  '2026-09-13T08:30:00', NULL,                  N'CONFIRMED'),
    (11, 1, 4,  '2026-09-09T07:00:00', NULL,                  N'CONFIRMED'),
    (12, 3, 13, '2026-09-05T10:00:00', NULL,                  N'NO_SHOW'),
    (13, 6, 13, '2026-09-05T11:00:00', NULL,                  N'CONFIRMED'),
    (14, 1, 14, '2026-09-01T07:00:00', NULL,                  N'CONFIRMED'),
    (15, 2, 15, '2026-09-01T08:00:00', NULL,                  N'CONFIRMED'),
    (16, 5, 16, '2026-09-01T09:00:00', NULL,                  N'CONFIRMED'),
    (17, 1, 6,  '2026-09-10T19:00:00', NULL,                  N'CONFIRMED'),
    (18, 2, 6,  '2026-09-10T19:05:00', NULL,                  N'CONFIRMED'),
    (19, 4, 5,  '2026-09-14T07:15:00', NULL,                  N'CONFIRMED'),
    (20, 3, 11, '2026-09-13T10:35:00', NULL,                  N'CONFIRMED');

INSERT INTO WaitlistEntries (waitlist_entry_id, trainee_id, training_session_id, position, joinTimestamp, status, expirationTimestamp)
VALUES
    (1,  2, 7,  1, '2026-09-12T07:05:00', N'WAITLISTED', NULL),
    (2,  3, 7,  2, '2026-09-12T08:00:00', N'WAITLISTED', NULL),
    (3,  8, 6,  1, '2026-09-13T19:00:00', N'WAITLISTED', NULL),
    (4,  4, 5,  1, '2026-09-14T07:00:00', N'CONFIRMED',  '2026-09-14T07:30:00'),
    (5,  5, 7,  3, '2026-09-12T09:00:00', N'CANCELLED',  NULL),
    (6,  7, 6,  2, '2026-09-13T20:00:00', N'NO_SHOW',    '2026-09-13T20:30:00'),
    (7,  6, 8,  1, '2026-09-13T08:00:00', N'WAITLISTED', NULL),
    (8,  8, 8,  2, '2026-09-13T08:05:00', N'WAITLISTED', NULL),
    (9,  3, 11, 1, '2026-09-13T10:00:00', N'CONFIRMED',  '2026-09-13T10:30:00'),
    (10, 5, 3,  1, '2026-09-12T12:30:00', N'CANCELLED',  NULL);

INSERT INTO WorkoutPerformances (workout_performance_id, trainee_id, training_session_id, trainer_id, date, weight, sets, reps, calorieTarget, proteinTarget, dailyStepGoal, notes)
VALUES
    (1, 6, 13, 2, '2026-09-08', 40.00, 4, 10, 160, 120, 8000, N'סקוואט - טכניקה משתפרת מאימון לאימון'),
    (2, 6, 13, 2, '2026-09-08', 20.00, 3, 12, 120, 120, 8000, N'לחיצת חזה במוט'),
    (3, 1, 14, 1, '2026-09-07', 25.00, 3, 12, 140, 130, 9000, NULL),
    (4, 1, 14, 1, '2026-09-07', 15.00, 3, 15, 110, 130, 9000, N'משיכת גב בפולי עליון'),
    (5, 2, 15, 3, '2026-09-06', 0.00,  4, 30, 200, 110, 10000, N'חבל קרב - סטים אינטרוולים'),
    (6, 2, 15, 3, '2026-09-06', 0.00,  3, 10, 90,  110, 10000, N'קפיצות לארגז פליו'),
    (7, 5, 16, 6, '2026-09-05', 0.00,  3, 20, 130, 100, 7000, N'תרגילי ליבה על המזרן'),
    (8, 5, 16, 6, '2026-09-05', 0.00,  2, 15, 80,  100, 7000, NULL);
