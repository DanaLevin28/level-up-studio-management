# LEVEL UP Studio — Requirements Specification

Source: PartA_Group10.pdf, pages 14–19 (functional/non-functional requirements), 20–24 (AI-assisted elicitation and resulting requirements), 35 (traceability matrix).

> Translation note: the source requirements (User Stories, FR, NFR) were written in Hebrew in the original document. This file is a faithful translation into English, preserving IDs, structure (MUST/SHOULD/COULD priority, CRUD sub-breakdown) and content. The Hebrew analysis-stage documents (`docs/org-analysis/`) retain the original Hebrew wording verbatim; this file does not.

## Priority Scale

- **MUST**: A critical requirement without which the system cannot function within the current development scope.
- **SHOULD**: A requirement of high value, very desirable to implement, but temporary omission is possible.
- **COULD**: A requirement that improves efficiency and quality; can be waived without a substantive impact on functionality.

## Functional Requirements (FR)

### US-1: As the Studio Manager, I want to manage trainee cards, so that I can consolidate all personal and medical information of my clients in one place.

**FR-1 (Priority: MUST):** The system shall allow the Studio Manager to manage trainee records in the system.

- **Create (FR-1.1):**
  - FR-1.1.1: The system shall allow the manager to add a new trainee with the mandatory fields: full name, phone, email, medical limitations, and experience level.
  - FR-1.1.2: The system shall assign a unique identifier to each new trainee and shall reject registration if the ID number or email address already exists in the database.
  - FR-1.1.3: If a required field is missing or the entered data does not match the required format, the system shall not create the record and shall display to the manager a focused error message about the invalid fields.
- **Read (FR-1.2):**
  - FR-1.2.1: The system shall display to the Studio Manager the full trainee card with all its fields.
  - FR-1.2.2: The system shall allow the Studio Manager to search and filter the trainee list by name, phone number, membership type, or activity status.
- **Update (FR-1.3):**
  - FR-1.3.1: The system shall allow the manager to edit and update the trainee card data (except the unique identifier, which cannot be changed).
  - FR-1.3.2: The system shall enforce the same validation rules on the updated fields as during creation.
- **Delete (FR-1.4):**
  - FR-1.4.1: Upon transition to "Inactive" status, the system shall block the trainee's access to workout registration and remove them from distribution lists, but shall retain their details and workout history in the database for analysis and financial reporting purposes.
  - FR-1.4.2: The system shall permanently remove a trainee record from the database only under a primary Studio Manager's authorization, and after entering a documented reason (such as a duplicate record).

### US-2: As the Studio Manager, I want to manage trainer cards (internal and rented), so that I can schedule them to workouts and track their availability.

**FR-2 (Priority: MUST):** The system shall allow the Studio Manager to manage trainer records in the system.

- **Create (FR-2.1):**
  - FR-2.1.1: The system shall allow adding a new trainer with mandatory fields: name, phone, specialization, employment type.
  - FR-2.1.2: The system shall assign a unique identifier to each trainer and reject registration if the ID number or email already exists in the database.
  - FR-2.1.3: If a mandatory field is missing or the data entered is invalid, the system shall not create the record and shall display a focused error message.
- **Read (FR-2.2):**
  - FR-2.2.1: The system shall display to the manager the full trainer card with all its fields.
  - FR-2.2.2: The system shall allow the manager to search and filter the trainer list by specialization and employment type.
- **Update (FR-2.3):**
  - FR-2.3.1: The system shall allow editing and updating trainer data (except the unique identifier).
  - FR-2.3.2: The system shall enforce validation rules on updated fields exactly as at creation.
- **Delete (FR-2.4):**
  - FR-2.4.1: Upon transition to "Inactive" status, the system shall prevent future scheduling of the trainer to workouts, but shall retain the history of workouts they conducted in the database.
  - FR-2.4.2: The system shall permanently remove a trainer record only under primary manager authorization and after entering a documented reason.

### US-3: As the Studio Manager, I want to manage workout types, so that I can diversify the schedule according to demand.

**FR-3 (Priority: MUST):** The system shall allow the manager to manage workout types in the system.

- **Create (FR-3.1):**
  - FR-3.1.1: The system shall allow creating a new workout type with mandatory fields: name, difficulty level, duration (minutes), maximum trainee capacity.
  - FR-3.1.2: The system shall prevent creating two workout types with an identical name.
  - FR-3.1.3: If data is missing, the system shall reject the creation and display an error message to the manager.
- **Read (FR-3.2):**
  - FR-3.2.1: The system shall display the list of existing workout types with all their details.
  - FR-3.2.2: The system shall allow filtering the workout list by difficulty level or duration.
- **Update (FR-3.3):**
  - FR-3.3.1: The system shall allow editing workout type details.
  - FR-3.3.2: The system shall enforce validation rules at update time to prevent invalid data (e.g., negative capacity).
- **Delete (FR-3.4):**
  - FR-3.4.1: Upon transition to "Archived" status (soft delete), the system shall prevent assigning new workouts of this type to the weekly schedule, but shall retain it for historical reports.
  - FR-3.4.2: A hard delete of a workout type shall be allowed only if it has no linked workout history, and only under manager authorization.

### US-4: As the Studio Manager, I want to manage the room inventory, so that I have an accurate and up-to-date record of the physical spaces and their capacity, for building the schedule.

**FR-4 (Priority: SHOULD):** The system shall allow the Studio Manager to manage the studio's rooms.

- **Create (FR-4.1):**
  - FR-4.1.1: The system shall allow adding a new room with mandatory fields: room name (unique) and maximum capacity.
  - FR-4.1.2: The system shall reject creating a room with a name that already exists in the database.
  - FR-4.1.3: The system shall display a focused error message if data is missing or does not match the format (e.g., text in the capacity number field).
- **Read (FR-4.2):**
  - FR-4.2.1: The system shall display the list of existing rooms in the studio with their details.
  - FR-4.2.2: The system shall allow searching and filtering this list by minimum or maximum capacity.
- **Update (FR-4.3):**
  - FR-4.3.1: The system shall allow the manager to edit the details of an existing room.
  - FR-4.3.2: The system shall enforce all validation rules defined at the creation stage also during the update.
- **Delete (FR-4.4):**
  - FR-4.4.1: Changing status to "Inactive" shall block the room from selection for future workout scheduling in the weekly schedule, but the system shall retain the room's usage history in the database.

### US-5: As the Studio Manager, I want to manage membership and punch-card types, so that I can define the payment tracks available for purchase by trainees.

**FR-5 (Priority: MUST):** The system shall allow the Studio Manager to manage the catalog of membership types in the system.

- **Create (FR-5.1):**
  - FR-5.1.1: The system shall allow creating a new payment track with mandatory fields: track name, price, number of allowed entries, and validity in days.
  - FR-5.1.2: The system shall prevent creating two active tracks with an identical name.
  - FR-5.1.3: The system shall display an error if missing or invalid data is entered.
- **Read (FR-5.2):**
  - FR-5.2.1: The system shall display the full catalog of tracks to the manager.
  - FR-5.2.2: The system shall allow filtering by track type (punch card / monthly subscription) or searching by price range.
- **Update (FR-5.3):**
  - FR-5.3.1: The system shall allow the manager to update the pricing and terms of existing tracks.
  - FR-5.3.2: Validation rules from the creation stage shall also be enforced during an update.
- **Delete (FR-5.4):**
  - FR-5.4.1: The system shall allow changing a track's status to "Inactive" (soft delete), which shall remove it from the purchase catalog for trainees, but shall retain it in purchase history.

### US-6: As the Studio Manager, I want to manage equipment items, so that I can track their condition and the studio's inventory.

**FR-6 (Priority: COULD):** The system shall allow the Studio Manager to manage the equipment inventory in the system.

- **Create (FR-6.1):**
  - FR-6.1.1: The system shall allow adding a new equipment item to the inventory with mandatory fields: item name, assigned room, purchase date, and baseline condition status.
  - FR-6.1.2: The system shall assign a unique identifier/barcode to each item.
  - FR-6.1.3: The system shall not save an item without assignment to a valid room from the list, and shall display an error.
- **Read (FR-6.2):**
  - FR-6.2.1: The system shall display the full equipment list.
  - FR-6.2.2: The system shall allow filtering by a specific assigned room or by condition status (e.g., displaying only faulty equipment).
- **Update (FR-6.3):**
  - FR-6.3.1: The system shall allow the manager to edit item details and update its condition status on an ongoing basis.
  - FR-6.3.2: Any condition-status change shall be validated against the list of permitted statuses.
- **Delete (FR-6.4):**
  - FR-6.4.1: The system shall allow retiring (soft delete) an equipment item in case of a fault or complete removal from use, while retaining documentation in the database for future expense calculations.

### US-7: As a trainee, I want to perform independent registration for workouts on the schedule, so that I can secure my spot in the workout easily, at my own time and from anywhere.

**FR-7 (Priority: MUST):** The system shall allow the trainee to perform independent registration for workouts.

- FR-7.1: The system shall display to the trainee the weekly workout schedule, including workout type, trainer name, and the number of available spots in real time.
- FR-7.2: The system shall allow the trainee to select an open workout, shall check the membership validity and remaining entries in the database, and shall confirm the registration.
- FR-7.3: If the registration is approved, the system shall send the trainee an immediate confirmation notification and update the number of available spots for that workout downward.

### US-8: As a trainee, I want to join the waitlist when a workout is full, so that I can get in in case of cancellations.

**FR-8 (Priority: MUST):** The system shall manage automatic waitlists for workouts at full capacity.

- FR-8.1: The system shall allow a trainee to join the waitlist for a workout that has reached its defined maximum capacity, and shall preserve their position chronologically.
- FR-8.2: Upon a registration cancellation by another trainee, the system shall identify the first person on the waitlist and allocate them a temporary open spot for 30 minutes.
- FR-8.3: The system shall send an alert to the first person on the waitlist to confirm their arrival. If not confirmed within the time frame, the system shall automatically transfer the right to the next person on the list.

### US-9: As a trainer, I want to document trainee progress and their medical limitations, so that I can tailor a safe and effective training program for them.

**FR-9 (Priority: MUST):** The system shall allow trainers to document and view medical records and performance of trainees.

- FR-9.1: The system shall require a new trainee to approve a digital health declaration as a blocking condition for registering to their first workout.
- FR-9.2: The system shall display to the guiding trainer, before the start of the class, the list of registrants accompanied by a prominent indication of medical limitations entered on their profile.
- FR-9.3: The system shall allow the trainer to enter performance data (weights and reps) for each trainee at the end of the class, and shall save this data as a continuous workout history in the customer's card.

### US-10: As a trainee, I want to pay and renew my subscription independently, to save coordination time with the studio's reception.

**FR-10 (Priority: MUST):** The system shall enable secure self-service membership payment and renewal.

- FR-10.1: The system shall notify a trainee when their punch card's remaining balance stands at 2 or fewer entries, or when the membership validity expires within 7 days.
- FR-10.2: The system shall present to the trainee the catalog of memberships available for purchase and enable credit clearing.
- FR-10.3: After a successful clearing, the system shall automatically update the trainee's remaining workout balance on their card and issue a digital receipt.

### US-11: As a studio manager, I want to receive a monthly analysis of studio activity, so that I can make data-based decisions on occupancy and profitability.

**FR-11 (Priority: SHOULD)** — Composite report: Occupancy and Profitability:

- FR-11.1: The system shall produce a report combining occupancy data against revenue, including a calculation of the average occupancy percentage (registrants actually attended out of maximum capacity), for each workout type and trainer, over a requested date range.
- FR-11.2: The system shall allow filtering the report by workout types, date ranges, and trainer names, and shall display rising/declining trends relative to the previous comparable period.

### US-12: As a studio manager, I want to track charges of renting trainers, to streamline the monthly billing process.

**FR-12 (Priority: SHOULD)** — Composite report: Monthly Renter Billing:

- FR-12.1: The system shall produce a report consolidating the total room-rental hours conducted by each renting trainer over the course of the month.
- FR-12.2: The system shall automatically calculate the amount owed for payment on the basis of actual rental hours multiplied by the hourly rate defined in the renting trainer's card, and shall display a breakdown of dates and exact hours for each charge.

### US-13: As a trainee, I want to freeze my membership for a defined period in case of vacation or injury, so that I don't lose the validity days I paid for.

**FR-13 (Priority: MUST):** The system shall enable temporary freezing of memberships. The system shall allow entering start and end dates for the freeze; during this period the membership status shall change to "Frozen", validity days shall not count down, and the client shall be blocked from registration.

### US-14: As a studio manager, I want to automatically block trainees whose payment has failed, to prevent debt accumulation.

**FR-14 (Priority: MUST):** The system shall enforce blocking of debtors. The system shall identify a declined credit clearing, change the status to "Suspended", and block future registration for workouts.

### US-15: As a studio manager, I want to create and distribute promo codes, to encourage new registrations during campaign periods.

**FR-15 (Priority: SHOULD):** The system shall manage a promo-code mechanism. The system shall enable entering a unique code, defining a discount percentage and expiration date, which shall be automatically updated at the payment stage.

### US-16: As a studio manager, I want the system to document inquiries from new interested parties (leads), so that I can get back to them and make sure they book a trial workout.

**FR-16 (Priority: SHOULD):** The system shall manage "interested party" (lead) records. The system shall enable creating a lead card without an active membership, including name, phone, and a date for a future follow-up call.

### US-17: As a studio manager, I want to send a feedback survey to a new trainee immediately after their trial workout, to make sure their onboarding experience was positive.

**FR-17 (Priority: SHOULD):** The system shall automatically send a feedback request for a first workout. The system shall identify a trainee's first attendance and send a notification with a link to a survey two hours after the workout ends.

### US-18: As a studio manager, I want to document summaries of conversations and personal notes in the customer card, so that the entire management team is aware of trainees' special requests.

**FR-18 (Priority: COULD):** The system shall enable managing a "contact log" for the customer. The system shall enable adding free-text entries carrying a date and the entering user's name, within the trainee's card.

### US-19: As a studio manager, I want to automatically send a birthday benefit to trainees, to strengthen their sense of belonging and loyalty to the studio.

**FR-19 (Priority: COULD):** The system shall automatically send birthday benefits. The system shall automatically send a good-luck message accompanied by a financial credit or a free punch on the card, once a year.

### US-20: As a studio manager, I want to manually promote a specific trainee on the waitlist, to give priority for emergency cases or as a service compensation.

**FR-20 (Priority: SHOULD):** The system shall enable manual intervention in waitlist management. The system shall allow the manager to bypass the chronological order and assign a specific trainee directly to a workout at the expense of other waiting trainees.

## Non-Functional Requirements (NFR)

### US-21: As a trainee, I want to access the weekly workout schedule directly from the home screen, to reserve a spot for a nearby workout.

**NFR-1 (Priority: MUST):** The loading time of the weekly schedule screen shall not exceed 3 seconds for 95% of accesses under a normal network connection.

### US-22: As a trainee, I want the system to operate at all hours of the day, so that I can cancel or register for workouts late at night.

**NFR-2 (Priority: MUST):** The system shall operate 24 hours a day, 7 days a week.

### US-23: As a trainee, I want to access the system from any device I own, so that I am not dependent on a computer.

**NFR-3 (Priority: MUST):** The user interface shall be responsive and shall fully support up-to-date mobile browsers on the iOS and Android operating systems.

### US-24: As a trainee, I want my medical declaration to remain confidential, to preserve my privacy.

**NFR-4 (Priority: MUST):** Medical declaration data shall be encrypted and shall be readable only to users authorized as "Trainer" or "Manager."

### US-25: As a studio manager, I want financial and historical data to be preserved even in case of a failure, to prevent loss of critical business information.

**NFR-5 (Priority: SHOULD):** The system shall perform a full automatic data backup to an external cloud server once every 24 hours.

### US-26: As a studio manager, I want the system to remain stable even at the moment the weekly registration opens, to prevent frustrating crashes.

**NFR-6 (Priority: MUST):** The system shall support 200 concurrently connected users performing read/write operations simultaneously, without deviating from the performance requirement (NFR-1).

### US-27: As a user, I want to read and enter data in my native language, to avoid misunderstandings.

**NFR-7 (Priority: MUST):** All screens of the system, alerts, and input/output processes shall support full Hebrew, with right-to-left alignment.

### US-28: As a new trainee, I want to succeed in registering for a workout easily, so that I don't need to ask the team for help.

**NFR-8 (Priority: SHOULD):** The registration-for-workout process for a connected user shall require no more than 3–4 operations from clicking the home screen to completing the operation.

### US-29: As a studio manager, I want each user to see only the screens relevant to them, to prevent errors or exposure of business data.

**NFR-9 (Priority: MUST):** The system shall implement a role-based authorization model comprising 4 separate layers (manager, trainer, renter, trainee), and shall block API access and report screens to non-manager users.

### US-30: As a trainee, I want to receive notifications directly to my mobile, to stay updated in real time on cancellations or vacated spots.

**NFR-10 (Priority: MUST):** The system shall interface with an external notification-distribution service (such as an SMS gateway or WhatsApp API) for sending automatic alerts as defined in the functional requirements.

## Requirements gathered from an AI-assisted elicitation session

> See Appendix A below for the elicitation method. The following requirements (US-31…US-40 / FR-21…FR-30) were distilled from that session and were originally recorded in the source document in English.

### US-31: As an independent trainer, I want to select a free room, date, and time and reserve them in one click, so I can confirm to the client immediately and appear professional.

**FR-21 (Priority: MUST):** The system shall enable room reservation and prevent conflicts. The system shall change the room's status to "Occupied" immediately upon reservation confirmation, and shall block any attempt by another user to reserve the same room in the same time slot by displaying an error message.

### US-32: As an independent trainer, I want to reserve a recurring series of workouts in one action, so I don't need to create a separate booking for regular clients.

**FR-22 (Priority: SHOULD):** The system shall enable reserving a series of sessions. The system shall allow a user to define a reservation as a "recurring series" by day and fixed time for a defined date range, and shall perform an automatic reservation for all available dates in sequence in the database.

### US-33: As an independent trainer, I want to change the date or room of an existing booking, so the change is not counted as a cancellation and does not incur penalties or duplication in my monthly billing report.

**FR-23 (Priority: SHOULD):** The system shall enable a reschedule/move operation. The system shall allow updating the date, time, or room of an existing reservation record, and shall document the action under a "Rescheduled" status instead of creating a cancellation record and a new record.

### US-34: As an independent trainer, I want the cancellation-fee policy to be calculated automatically at the time of cancellation of a booking, so the charge is transparent, known in advance, and without disputes.

**FR-24 (Priority: MUST):** The system shall enforce an automatic cancellation-fee policy based on the timing of the cancellation. A cancellation performed 12 hours or more before the workout's start time shall be classified as "cancelled in time" (no charge). A cancellation performed less than 12 hours before the start time shall be classified as "late cancellation" and shall trigger an automatic charge of 50% of the rental cost.

### US-35: As an independent trainer, I want to receive a monthly report presenting details for each booking and a summary line, so the manager and I see the same, exact figures and there are no disputes.

**FR-25 (Priority: MUST):** The system shall produce a monthly billing report for the renter. The report shall present a table with the columns: date, start and end time, room, status, cancellation timestamp, and amount to be charged. At the bottom of the report the system shall display a summary line calculating the total number of workouts, total cancellations, and the total amount due for payment.

### US-36: As an independent trainer, I want to mark, at the time of a room reservation, which equipment I need, so the equipment is reserved for me and another trainer cannot occupy it in the same time slot.

**FR-26 (Priority: SHOULD):** The system shall enable reserving equipment items in parallel to the room. The system shall display an equipment list for selection at the time of reservation, and shall block selection of an item already assigned to another reservation in exactly the same time slot.

### US-37: As an independent trainer, I want to receive a push notification every morning at 07:00 with a summary of my workouts for the day, so I can start the day with a clear picture.

**FR-27 (Priority: COULD):** The system shall produce a daily summary notification. The system shall automatically send a push notification to the trainer's device every day at 07:00, containing a text string detailing the number of workouts, hours, and rooms approved for them for that day.

### US-38: As an independent trainer, I want the right to reserve a room that was vacated to be given by request order with a time window for confirmation, so the process is fair and efficient without a room remaining empty in case of no response.

**FR-28 (Priority: MUST):** The system shall manage a chronological waiting list for room reservations. Upon cancelling a reservation, the system shall send an alert to the first person on the waitlist and allocate them an exclusive 30-minute time window for confirmation. If the waiting person declines the active offer, or the time window expires, the system shall automatically transfer the reservation right to the next person on the list.

### US-39: As an independent trainer, I want the system to send a push notification to a trainer found in a room 5 minutes before the end of their slot, so they finish on time and don't cause a delay.

**FR-29 (Priority: SHOULD):** The system shall alert on an approaching workout end. The system shall identify the existence of consecutive events in the same room, and shall send a precise push notification to the currently present trainer 5 minutes before the end time of their defined reservation.

### US-40: As an independent trainer, I want to receive an alert the moment the trainer's slot before mine has officially ended, so I can prepare for entry or update my client in case the trainer has not yet vacated the room.

**FR-30 (Priority: COULD):** The system shall allow a trainer to update the "room vacated" status in the system upon the end of their workout. Immediately upon receiving the status update, and if a subsequent reservation exists for the same room, the system shall automatically send a push notification to the incoming trainer indicating that the room is free for entry.

## Traceability Matrix

Source: PartA_Group10.pdf, page 35 — a checkbox grid image mapping all 30 functional requirements to the 15 use cases. Transcribed below as a table (✓ = FR is realized by / traced to that use case).

> Confidence note: this is a dense 30-row × 15-column checkbox grid embedded as an image. The mapping below is a best-effort visual transcription; rows FR-1 through FR-20 (one checkmark per row, clearly aligned with their originating CRUD-style use case) are transcribed with high confidence. A few rows in FR-21…FR-30 sit close to column boundaries in the source image and should be spot-checked against the original PDF page if the exact use-case attribution matters for grading.

| FR | Traced-to Use Case |
|---|---|
| FR-1 – manage trainee records | Manage Trainees |
| FR-2 – manage trainer records | Manage Trainers |
| FR-3 – manage workout types | Manage Workout Types |
| FR-4 – manage room inventory | Manage Rooms |
| FR-5 – manage membership types | Manage Memberships |
| FR-6 – manage equipment inventory | Manage Equipment |
| FR-7 – self-service workout registration | Register To Workout |
| FR-8 – automatic waitlist management | Join Waitlist |
| FR-9 – document performance / medical profile | Document Workout Performance |
| FR-10 – self-service membership payment | Renew & Pay Membership |
| FR-11 – complex report: occupancy & profitability | Generate Occupancy & Profitability Report |
| FR-12 – complex report: renters billing | Generate Renters Billing Report |
| FR-13 – temporary membership freeze | Renew & Pay Membership |
| FR-14 – enforce debtor lock | Renew & Pay Membership |
| FR-15 – manage promo codes | Manage Memberships |
| FR-16 – manage lead (prospect) records | Manage Trainees |
| FR-17 – automatic feedback request | Register To Workout |
| FR-18 – manage contact log | Manage Trainees |
| FR-19 – automatic birthday benefit | Manage Trainees |
| FR-20 – manual waitlist intervention | Join Waitlist |
| FR-21 – room booking, conflict prevention | Manage Rooms |
| FR-22 – recurring session series | Manage Rooms |
| FR-23 – reschedule/move booking | Manage Rooms |
| FR-24 – automatic cancellation-fee policy | Generate Renters Billing Report |
| FR-25 – monthly renter billing report | Generate Renters Billing Report |
| FR-26 – reserve equipment with room | Manage Equipment |
| FR-27 – daily summary notification | Register To Workout |
| FR-28 – chronological room waitlist | Manage Rooms |
| FR-29 – approaching room end-of-slot alert | Manage Rooms |
| FR-30 – room-vacated status update | Manage Rooms |

The source document's own commentary on the matrix (translated):

> The centrality of the traceability matrix, in our view, is significant because it is not merely an organizational tool, but forms the basis for correct and reliable planning of the system's architecture. It allows us to verify unambiguously that there is full engineering coverage for every requirement defined. Even more importantly, it helps in identifying gaps at early stages of the requirement specification: for example, identifying a requirement that is not linked to any use case indicates a gap in translating it into actual system operation, whereas a use case with no requirement linked to it raises doubts as to its necessity. In addition, the matrix serves us as a strategic tool for quality control and risk management: when a future change is required in a certain requirement, the traceability thread the matrix provides will allow us to identify precisely and immediately which processes and parts of the system are affected by it. This mapping ability maps forward influences, saves precious time, significantly eases the continuation of development, maintenance, and testing processes, while ensuring that the final product indeed answers the complete needs of the system.

## Appendix A — AI-assisted requirements elicitation session

Source: PartA_Group10.pdf, pages 20–22. The group used a conversation with Claude to elicit additional requirements from the perspective of an independent renting trainer, by role-playing as "Aviatar" (owner of LEVEL UP). The original prompt to Claude (translated from Hebrew) was:

> "For a Software Analysis and Design course project, I need to build a persona representing an independent trainer (a renter) who wants to book rooms in a fitness studio at LEVEL UP. Interview the character Aviatar about his current business processes. I documented the organization's existing processes accurately, but the following business needs were not clear to me yet: 1) Room-booking conflicts (double bookings) are prevented and resolved somehow. 2) There is no documented standard for how a renting trainer knows in advance how much he must pay at the end of the month, when he can charge his clients, and how he can dispute an incorrect report he received. Talk to me in first person as if you were Aviatar, and via a suitable persona and simulated conversation help me formulate a new set of requirements that will help you run your business better?"

> Confidence note: the chat transcript on pages 20–22 renders with heavier OCR/RTL distortion than the rest of the document. The quotes below are a faithful reconstruction of their substance and meaning, translated into English, but should not be treated as a word-for-word transcript — verify against the original PDF pages if exact wording is needed.

Claude, role-playing as "Aviatar," raised (translated, reconstructed from a partially garbled transcript — see confidence note above) two clusters of pain points that were then converted into the formal user stories US-31…US-40 above:

**Room-coordination pain points:**
- Double bookings: "I have worked here for over a year. I use 15 fixed rooms weekly, and each week I request three-four times a room I already requested a moment before — over the phone, that's just how it works."
- No single source of truth: "I sometimes discover, only after arriving with a client, that another trainer is already in 'my' room. There is no single place that shows all the reservations at once — only phone calls are what I get."

**Payment-tracking pain points:**
- No visibility into monthly charges: "At the end of every month I am told how much I owe based on the studio's activity in practice. I have no way to know myself. I try to reconstruct via texts and phone calls, and it's often too late to dispute."
- No cancellation clarity: "It's unclear what happens if I cancel: sometimes I'm charged as if the session happened, sometimes I'm not — it depends on who I spoke to and when."
- No booking record for reference: "I have no single, ongoing document of my bookings; I currently keep everything in messages and memory, which is a source of errors and disagreements."

These pain points map directly to FR-21 (double-booking prevention), FR-23/FR-24 (reschedule/cancellation clarity), FR-25 (monthly billing report), FR-26 (equipment reservation), and FR-28/FR-29/FR-30 (room-handoff visibility).
