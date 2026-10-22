# LEVEL UP Studio — Use Case Specifications

Source: PartA_Group10.pdf, pages 25–34. Content is in English in the source document.

## Actors Table

| # | Actor | Description |
|---|---|---|
| 1 | Studio Manager | Responsible for managing the system, setting the hours schedule, approving exceptions, and generating financial and operational reports. |
| 2 | Trainer (Team Trainer) | Conducts group and personal workouts, documents training plans, records trainee data, and reports on equipment condition. |
| 3 | Trainee | The studio's client; registers for workouts, pays for memberships/punch cards, etc. |
| 4 | Renting Trainer | An external freelance trainer who rents the studio's rooms for their own private workouts, and pays the studio by the hour. |
| 5 | Time System | An automatic system actor. Runs time-based triggers, such as sending reminders before a workout, alerts on membership expiration, and automatic promotion of waitlisted people. |

## Use Case Package Diagrams

![use case diagrams](use-case-diagrams.png)

> We modeled the system as 3 Use Case diagrams: (1) System Administration, (2) Training & Registration, (3) Billing & Reports.

### Diagram 1 — System Administration (Requirements: FR1–FR6)

- **Actor:** Studio Manager
- **Use cases:** Manage Equipment, Manage Trainees, Manage Trainers, Manage Workout Types, Manage Rooms, Manage Memberships
- **Relationships:** Studio Manager is associated with all six use cases above (unnamed associations).

### Diagram 2 — LEVEL UP Management System / Training & Registration (Requirements: FR7–FR9)

- **Actors:** Trainee, Team Trainer, Studio Manager, System Time
- **Use cases:** Register to workout, Check Membership Validity, Join Waitlist, Document Workout Performance, Report Faulty Equipment, Confirm Waitlist Spot
- **Relationships:**
  - Trainee — Register to workout (association)
  - Register to workout — Check Membership Validity (**«Include»**)
  - Register to workout — Join Waitlist (**«Extend»**, extension point: "Workout is Full")
  - Team Trainer — Document Workout Performance (association)
  - Studio Manager — Document Workout Performance (association)
  - Team Trainer — Report Faulty Equipment (association)
  - Studio Manager — Report Faulty Equipment (association)
  - System Time — Confirm Waitlist Spot (association)

### Diagram 3 — Billing & Reports System (Requirements: FR10–FR12)

- **Actors:** Trainee, Studio Manager, Renting Trainer
- **Use cases:** Renew & Pay Membership, Generate Occupancy & Profitability Report, Generate Renters Billing Report
- **Relationships:**
  - Trainee — Renew & Pay Membership (association)
  - Studio Manager — Generate Occupancy & Profitability Report (association)
  - Studio Manager — Generate Renters Billing Report (association)
  - Renting Trainer — Generate Renters Billing Report (association)

---

## Detailed Use Case Specifications

The following 5 use cases are fully specified in the source document.

### 1. Manage Trainees

**ID:** UC01

#### Behavioral Specification

**Primary Actors:** Studio Manager

**Preconditions:** The user is logged in as a Studio Manager.

**Post-conditions:**
- (Creation / Update): The trainee record is saved in the system.
- (Deactivation): The trainee status is marked "Inactive" (not physically deleted, for historical reporting purposes).

**Scenarios:**

*Create*
1. Studio Manager requests to manage trainee record.
2. SYSTEM displays a creation form requiring the following fields: (Full Name, Phone, Email, Medical Limitations, Experience Level).
3. Studio Manager inputs the trainee's details and submits the form.
4. SYSTEM validates that the phone number and email are not already in use.
5. SYSTEM creates the trainee record and assigns a unique identifier.
6. SYSTEM displays a confirmation message: "The trainee has been successfully created."

Extension:
- 3.a. A required field is left empty.
  1. The System displays an error message and highlights the missing required fields.
  2. jump to 3. Studio Manager inputs the trainee's details and submits the form.
- 4.a. Phone number or email already exists in the system.
  1. The System displays an error message: "A trainee with this phone number or email already exists."
  2. jump to 3. Studio Manager inputs the trainee's details and submits the form.

*Read*
1. Studio Manager requests to view or search the trainee list, optionally filtering by (Status, Membership Type).
2. SYSTEM retrieves and displays the Trainee Table, including the fields: (Name, Phone, Membership Type, Status).
3. Studio Manager selects a specific trainee row to view the full profile.
4. SYSTEM displays the trainee's full profile, including workout history and current membership details.

Extension:
- 2.a. No trainee matches the search criteria.
  1. displays an empty result with a no-matches message.
  2. jump to 1. Studio Manager requests to view or search the trainee list, optionally filtering by (Status, Membership Type).
- 3.a. The selected trainee record no longer exists (removed by another Studio Manager session).
  1. displays a "record not found" message and refreshes the trainee list.
  2. jump to 1. Studio Manager requests to view or search the trainee list, optionally filtering by (Status, Membership Type).

*Update*
1. Studio Manager selects a specific trainee from the list and clicks "Update".
2. SYSTEM displays the current field details in an editable form.
3. Studio Manager modifies one or more fields, excluding the unique identifier, and submits.
4. SYSTEM validates the updated fields using the same rules as creation.
5. SYSTEM saves the updated trainee record and stamps the record with the last-modified date.

Extension:
- 4.a. Updated data fails validation.
  1. displays an error message and highlights the invalid fields.
  2. jump to 3. Studio Manager modifies one or more fields, excluding the unique identifier, and submits.
- 4.b. Updated phone number or email matches another existing trainee.
  1. displays an error message: "A trainee with this phone number or email already exists."
  2. jump to 3. Studio Manager modifies one or more fields, excluding the unique identifier, and submits.

*Delete*
1. Studio Manager selects an active trainee from the list and clicks "Deactivate".
2. SYSTEM validates that the trainee has no active future registrations.
3. SYSTEM marks the trainee status as "Inactive".
4. SYSTEM displays a deactivation confirmation message.

Extension:
- 2.a. Trainee has an active future registration.
  1. displays an error message: "Cannot deactivate — trainee has an active upcoming registration."
  2. jump to 1. Studio Manager selects an active trainee from the list and clicks "Deactivate".
- 2.b. Trainee is already marked "Inactive".
  1. displays a message: "This trainee is already inactive."
  2. jump to 1. Studio Manager selects an active trainee from the list and clicks "Deactivate".

**Relationships:**

| Relationship | From | To |
|---|---|---|
| unnamed | Studio Manager | Manage Trainees |

#### Implementation Notes

> TODO — the source document does not include implementation notes (data model / API / stored-procedure level detail) for this use case.

---

### 2. Register to Workout

**ID:** UC02

#### Behavioral Specification

**Primary Actors:** Trainee

**Preconditions:**
- The Trainee is logged into the application.
- The weekly workout schedule is published and available in the system.

**Post-conditions:**
- The Trainee is successfully registered for the selected workout.
- The system decreases the available spots for the specific workout by 1.
- A confirmation notification is sent to the Trainee.

**Scenario:**
1. The Trainee selects a specific workout from the weekly schedule.
2. SYSTEM displays the workout details (type, trainer, time) and the number of available spots.
3. The Trainee clicks the "Register" button.
4. SYSTEM verifies the Trainee's membership validity and remaining entries (Include: Check Membership Validity).
5. SYSTEM verifies that there is at least 1 available spot for the requested workout.
6. SYSTEM confirms the registration and decreases the available spots by 1.
7. SYSTEM adds the workout to the Trainee's personal schedule.
8. SYSTEM sends a registration confirmation notification to the Trainee.

Extension:
- 4.a. Membership is invalid or empty:
  1. System displays an error message: "Insufficient entries or expired membership."
  2. System redirects the Trainee to the membership purchasing screen.
  3. Use case ends.
- 5.a. Workout is full (Extension Point: Workout is full):
  1. System informs the Trainee that the workout is at maximum capacity.
  2. System prompts the Trainee to join the waitlist.
  3. The Trainee confirms (Extend: Join Waitlist).
  4. Use case ends.

**Relationships:**

| Relationship | From | To |
|---|---|---|
| unnamed | Register to workout | Check Membership Validity |
| unnamed | Register to workout | Join Waitlist |
| unnamed | Trainee | Register to workout |

#### Implementation Notes

> TODO — the source document does not include implementation notes (data model / API / stored-procedure level detail) for this use case.

#### UI Mockups (Reference)

Source: PartA_Group10.pdf, page 34. These two screens were selected by the group to demonstrate the user interface for a non-CRUD use case, showing the flow from the trainee's point of view: choosing a workout from the weekly schedule (Screen 1), followed by the registration confirmation screen reflecting the resulting change in system state — the reduced number of available spots (Screen 2). Steps 1–3 of the MSS are realized in Screen 1; steps 4–8 in Screen 2.

**Screen 1 — Weekly Time Schedule:** displays the list of planned workouts (name, type, day/time, trainer, room, tag green/yellow/red according to occupancy) with real-time available spots. The Trainee selects a workout and presses "Continue" to proceed to the next step (MSS steps 1–3).

**Screen 2 — Registration Confirmation:** shown after the system has checked membership validity and available capacity (MSS steps 4–8). Displays the chosen workout's details, the Trainee's membership status, and the confirmation notice together with the updated real-time spot count.

Example content shown in the mockup (LEVEL UP trainee app, user "Maxim K."):

| # | Workout | Type | Trainer | Day/Time | Room | Available Spots | |
|---|---|---|---|---|---|---|---|
| 1 | Morning Strength | Strength | Dana | Sun 07:00 | Functional Room | 4/10 | Select |
| 2 | Pilates Mat | Pilates | Dana | Sun 19:00 | Studio B | 1/10 | Select |
| 3 | Functional Circuit | Functional | Eviatar | Mon 18:00 | Functional Room | 0/10 | Waitlist |
| 4 | Personal Training | Personal | Eviatar | Mon 09:00 | Studio A | 1/1 | Select |
| 5 | Evening Strength | Strength | Dana | Tue 19:00 | Functional Room | 6/10 | Select |

Registration Confirmation screen (for Pilates Mat, Sunday 19:00): Trainer — Dana; Room — Studio B; Available spots before registration — 1/10; Membership status — Active, 7 entries remaining; confirmation message — "Registration confirmed — you are booked for this workout. Available spots updated to 0/10."

---

### 3. Document Workout Performance

**ID:** UC03

#### Behavioral Specification

**Primary Actors:** Studio Manager, Team Trainer

**Preconditions:** The Trainer is logged into the application.

**Post-conditions:** The trainee's workout and nutrition targets are successfully updated in the database.

**Scenario:**
1. Team Trainer requests to document a trainee's workout performance.
2. THE SYSTEM displays a search field to locate the trainee.
3. Team Trainer inputs the trainee's name or ID and submits.
4. THE SYSTEM displays a documentation form requiring the following fields: Strength Training metrics, Sets, Reps, Calorie & Protein Targets, and Daily Step Goals.
5. Team Trainer inputs the recorded data and submits the form.
6. THE SYSTEM saves the performance record and displays a success message.

Extension:
- 4.a. Trainee Not Found.
  1. detects that no active trainee matches the search criteria.
  2. displays an error message: "Trainee not found. Please verify the details."
  3. jump to 2. THE SYSTEM displays a search field to locate the trainee.

**Relationships:**

| Relationship | From | To |
|---|---|---|
| unnamed | Team Trainer | Document Workout Performance |
| unnamed | Studio Manager | Document Workout Performance |

#### Implementation Notes

> TODO — the source document does not include implementation notes (data model / API / stored-procedure level detail) for this use case.

---

### 4. Report Faulty Equipment

**ID:** UC04

#### Behavioral Specification

**Primary Actors:** Studio Manager, Team Trainer

**Preconditions:** The Team Trainer or Studio Manager is logged into the application.

**Post-conditions:** The fault report is successfully saved, and the equipment status is updated to "Out of Order".

**Scenario:**
1. Team Trainer requests to report a faulty equipment item.
2. The SYSTEM displays an equipment reporting form.
3. Team Trainer selects the equipment ID, inputs a description of the fault, and submits the form.
4. The SYSTEM validates that the selected equipment ID exists and is currently active.
5. The SYSTEM saves the fault report, updates the equipment status to "Out of Order", and displays a confirmation message.

Extension:
- 4.a. Invalid Equipment ID.
  1. detects that the equipment ID does not exist in the database.
  2. displays an error message: "Equipment not found. Please select a valid ID."
  3. jump to 3. Team Trainer selects the equipment ID, inputs a description of the fault, and submits the form.

**Relationships:**

| Relationship | From | To |
|---|---|---|
| unnamed | Team Trainer | Report Faulty Equipment |
| unnamed | Studio Manager | Report Faulty Equipment |

#### Implementation Notes

> TODO — the source document does not include implementation notes (data model / API / stored-procedure level detail) for this use case.

---

### 4b. Manage Waitlist

**ID:** UC06

> **Note:** Not part of the 5 use cases fully specified in the source PDF. Drafted by AI from FR-8 and FR-20 in `docs/00-requirements.md` (per CLAUDE.md's "AI can accelerate: writing VP18-style UC specs from a brief description") to satisfy the "write the missing VP18 spec before implementation" rule in CLAUDE.md. Pending group review.

#### Behavioral Specification

**Primary Actors:** Trainee, Time System (automatic promotion), Studio Manager (manual override)

**Preconditions:**
- (Join) The requested workout has reached its defined maximum capacity.
- (Promotion / Override) At least one confirmed registration exists for the workout, and at least one waitlist entry exists for it.

**Post-conditions:**
- (Join) The Trainee holds a waitlist entry with a chronological position.
- (Promotion) The first waitlist entry is offered the freed spot for a 30-minute confirmation window; on confirmation it becomes a CONFIRMED registration, on expiry or decline the offer passes to the next entry.
- (Manual Override) A specific Trainee chosen by the Studio Manager is promoted directly to a CONFIRMED registration, bypassing chronological order.

**Scenarios:**

*Join Waitlist*
1. Trainee attempts to register for a workout that is at full capacity (extension of Register to Workout, extension point "Workout is Full").
2. SYSTEM informs the Trainee the workout is full and offers to join the waitlist.
3. Trainee confirms.
4. SYSTEM creates a waitlist entry, assigning the next chronological position and a join timestamp.
5. SYSTEM displays a confirmation message with the Trainee's queue position.

*Automatic Promotion (Time System)*
1. A confirmed registration for the workout is cancelled, freeing a spot.
2. Time System identifies the first waitlist entry (lowest position) for that workout.
3. Time System allocates that entry a temporary open spot and sets a 30-minute expiration time.
4. Time System sends a confirmation alert to the Trainee.
5. If the Trainee confirms within the window: SYSTEM converts the waitlist entry into a CONFIRMED registration and removes it from the waitlist.
6. If the window expires without confirmation: SYSTEM marks the entry CANCELLED and repeats from step 2 for the next entry in position order.

*Manual Override*
1. Studio Manager selects a workout's waitlist and a specific Trainee entry on it, regardless of position.
2. SYSTEM validates a spot is available (or is being freed) for the workout.
3. SYSTEM promotes the selected entry directly to a CONFIRMED registration.
4. SYSTEM displays a confirmation message noting the manual override.

Extension:
- 2.a (Automatic Promotion / Manual Override) No spot becomes available.
  1. SYSTEM leaves all waitlist entries unchanged.
  2. Use case ends.

**Relationships:**

| Relationship | From | To |
|---|---|---|
| «Extend» (extension point: Workout is Full) | Join Waitlist | Register to Workout |
| unnamed | Trainee | Join Waitlist |
| unnamed | Time System | Confirm Waitlist Spot |
| unnamed | Studio Manager | Manage Waitlist (override) |

#### Implementation Notes

- Entity: `WaitlistEntry` (association class, Trainee ⇄ TrainingSession) — fields `position`, `joinTimestamp`, `status` (`RegistrationStatus` enum), `expirationTime`.
- Current implementation scope: CRUD panel (`WaitlistEntryPanel`) over `WaitlistEntry` for manual create/update/delete by the Studio Manager, covering the *Join* and *Manual Override* scenarios' data effects directly. The *Automatic Promotion* scenario's timer-driven behavior (Time System actor) is not implemented as a background process in this pass — it would require a scheduled task that scans `Program.WaitlistEntries` for expired `expirationTime` values and calls `Registration`'s constructor / `WaitlistEntry.deleteWaitlistEntry()`, outside the scope of a single CRUD panel.

---

### 5. Renew & Pay Membership

**ID:** UC05

#### Behavioral Specification

**Primary Actors:** Trainee

**Preconditions:** The Trainee is logged into the application.

**Post-conditions:** The membership is renewed, payment is processed, and a receipt is generated.

**Scenario:**
1. Trainee requests to renew their membership.
2. SYSTEM displays available membership plans and prices.
3. Trainee selects a plan, inputs payment details, and submits the request.
4. SYSTEM validates the payment details with the external billing system.
5. SYSTEM updates the membership expiration date, generates a receipt, and displays a success message.

Extension:
- 4.a. Payment Declined.
  1. detects that the payment was declined.
  2. displays an error message: "Payment declined. Please try another method."
  3. jump to 3. Trainee selects a plan, inputs payment details, and submits the request.

**Relationships:**

| Relationship | From | To |
|---|---|---|
| unnamed | Trainee | Renew & Pay Membership |

#### Implementation Notes

> TODO — the source document does not include implementation notes (data model / API / stored-procedure level detail) for this use case.

- Entity: `Membership` (FK: Trainee, MembershipPlan) — `status` (`MembershipStatus` enum: ACTIVE/EXPIRED/SUSPENDED) covers renewal, freeze (SUSPENDED) and debtor-block flows referenced in CLAUDE.md.
- Entity: `Payment` (FK: Membership) — `amount`, `paymentDate`, `paymentMethod`, `paymentStatus`; `PaymentGateway` is an external boundary, not a persisted entity (see CLAUDE.md).
- Current implementation scope: `MembershipPanel` (CRUD over `Membership`, wired to the "חידוש ותשלום מנוי" menu entry) with a "תשלומים" button opening `PaymentPanel` (CRUD over `Payment`) for the selected membership's payment history. External billing-gateway validation (MSS step 4) is out of scope — `paymentStatus` is recorded as entered, not verified against a real gateway.

---

### 6. Generate Renters Billing Report

**ID:** UC07

> **Note:** Not part of the 5 use cases fully specified in the source PDF. Drafted by AI from FR-12, FR-24, FR-25, FR-28 and FR-29 in `docs/00-requirements.md` (per CLAUDE.md's "AI can accelerate: writing VP18-style UC specs from a brief description") to satisfy the "write the missing VP18 spec before implementation" rule in CLAUDE.md. Pending group review.

#### Behavioral Specification

**Primary Actors:** Studio Manager, Renting Trainer

**Preconditions:** At least one room booking exists for the selected renting trainer within the reporting period.

**Post-conditions:** A billing report is generated, listing each booking's charge and a summary of total hours, cancellations, and amount due.

**Scenario:**
1. Studio Manager or Renting Trainer requests the monthly billing report for a specific renting trainer and month.
2. SYSTEM retrieves all room bookings for that trainer within the selected period.
3. SYSTEM classifies each booking by its status (confirmed / on-time / cancelled-late / rescheduled) and, for bookings cancelled less than 12 hours before the start time, applies an automatic late-cancellation charge of 50% of the rental cost (FR-24).
4. SYSTEM computes, per booking, the amount charged (hours × the trainer's hourly rate, or the late-cancellation fee where applicable).
5. SYSTEM displays the report: date, start/end time, room, status, and amount charged per row, with a summary line totaling workouts, cancellations, and amount due (FR-25).

Extension:
- 2.a. No bookings exist for the selected trainer/period.
  1. SYSTEM displays an empty report with a no-data message.
  2. Use case ends.

**Relationships:**

| Relationship | From | To |
|---|---|---|
| unnamed | Studio Manager | Generate Renters Billing Report |
| unnamed | Renting Trainer | Generate Renters Billing Report |

#### Implementation Notes

- Entity: `RoomBooking` (FK: Room, Trainer) — fields `date`, `startTime`, `endTime`, `status` (`BookingStatus` enum: CONFIRMED/ON_TIME/CANCELLED_LATE/RESCHEDULED).
- Current implementation scope: `RoomBookingPanel` (CRUD over `RoomBooking`, wired to the "דוח חיוב למאמנים שוכרים" menu entry) covering booking data entry/maintenance (the data the report is computed from). The computed report itself — per-row charge calculation (`Trainer.hourlyRate` × duration, late-cancellation fee per FR-24) and the monthly summary line (FR-25) — is **not implemented in this pass**; it is a read-only aggregation over `Program.RoomBookings` layered on top of this CRUD panel, left as follow-up work.
- FR-26 (equipment reservation alongside room), FR-28 (room-booking waitlist), FR-29 (approaching end-of-slot alert), FR-30 (room-vacated status) are out of scope: no supporting entity/field exists yet (no waitlist-for-rooms, no equipment-booking link, no session-in-progress state on `RoomBooking`).
