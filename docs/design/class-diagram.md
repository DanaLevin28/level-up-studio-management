![class diagram](class-diagram.png)

Source: PartB_Group10.pdf, pages 1–3. The class diagram itself is a Visual Paradigm export embedded as an image on page 3. **The source document itself notes, next to the diagram: *"יש להגדיל רזולוציה על מנת לראות בבירור"* ("resolution needs to be increased to see clearly").** The transcription below is a best-effort reading of that image. Class names, enumeration values, association labels, and most attributes/methods are read with high confidence; a number of exact multiplicities and a few method signatures / return types are at the edge of legibility at the available resolution and are marked **[UNCLEAR]**. If precise fidelity on the flagged items matters, please provide a PNG/SVG export or the original `.vpp` file and I will correct them.

## Selected Use Cases Modeled (from PartB, page 1)

The class diagram was built to support 6 selected use cases:

1. **Register to Workout** — a scenario allowing a trainee to independently register for an available workout from the weekly schedule. The goal is to enable quick and efficient seat reservation, while the system automatically verifies the validity of the customer's subscription and updates the number of remaining spots in the workout in real time.
2. **Manage Waitlist** — a scenario triggered when a trainee tries to register for a workout that has reached full capacity, placing them into a chronological waiting queue. Its goal is to maximize workout occupancy: the moment a spot opens up, the system automatically sends a notification to the first person waiting with a time-limited window to confirm, and passes the right to the next in line if there is no response.
3. **Document Workout Performance** — a scenario allowing a trainer to enter performance data (such as weights, sets, and reps) for each trainee immediately at the end of the class, in order to produce a documented and continuous workout history saved on the customer's card, which allows trainers to track the trainee's progress and tailor safer and more effective training programs.
4. **Renew & Pay Membership** — a scenario allowing trainees to renew their punch card or membership and make a secure self-service payment. The goal is to save coordination time with the studio manager, while automatically updating the entries balance in the database and producing a digital invoice for the customer.
5. **Generate Renters Billing Report** — a scenario allowing the studio manager to produce a monthly financial report consolidating the activity hours of external trainers (who rent the studio from them). The goal is to perform an accurate and automatic calculation of renters' debts based on actual reservation hours and their hourly rate, thereby preventing human error and end-of-month disputes.
6. **Manage Trainees** — a scenario allowing the studio manager to create, update, read, and delete trainee records in the system. Its goal is to consolidate all personal and medical information of clients in one place, while enforcing complex logic to prevent duplicates in the system (such as preventing entry of an existing email) and managing "soft delete" (changing status to inactive) in order to preserve the studio's financial data history.

## Diagram Assumptions (from PartB, page 1–2)

- A trainee cannot be registered simultaneously to the same occurrence of a scheduled workout more than once.
- Every training room has a maximum capacity, and therefore a workout scheduled into it cannot contain a number of trainees exceeding that capacity.
- A punch card or membership belongs exclusively to a single trainee and cannot be transferred or shared with other trainees.
- A trainer can be scheduled to at most one workout in the same time slot; overlapping hours are not permitted.
- A full or partial inventory movement, or a report of faulty equipment, refers to a specific and unique equipment item identified by a barcode.

## Rationale for Association Classes (from PartB, page 2)

- **Registration** (association class): connects a Trainee to a scheduled Workout (TrainingSession). We chose to model this as an association class because the pair Trainee–Workout uniquely identifies the occurrence, but the relationship itself has its own distinct attributes that must be persisted in the database (registration date, cancellation date, and participation status such as confirmed / cancelled / no-show). This data does not belong exclusively to either the trainee or the workout alone — it is created from the relationship between them.
- **WaitlistEntry** (association class): connects a Trainee to a scheduled Workout (similarly to the Registration association class), but for the case of full occupancy. Modeling it as an association class is required in order to document the trainee's specific position in the queue, the timestamp at which they entered the waitlist, and the expiration time of the confirmation window.
- **WorkoutPerformance** (association class): connects a Trainee to a specific Workout (and to the Trainer who conducted it). We chose this modeling because the actual performance data (weights lifted, number of reps and sets) is a direct product of the meeting between a specific trainee and a specific workout at a specific point in time, and cannot be stored only on the trainee's record without the context of the workout in which it was performed.

## Class Model

> Legend: `-` = private, `+` = public. Attributes/methods marked **[UNCLEAR]** are at the edge of legibility in the source image.

### Enumerations

- **RegistrationStatus** «enumeration»: PENDING, CONFIRMED, ACTIVE, COMPLETED, WAITLISTED, CANCELLED, NO_SHOW
- **WaitlistStatus** «enumeration»: CONFIRMED, WAITLISTED, CANCELLED, NO_SHOW
- **EquipmentCondition** «enumeration»: FUNCTIONAL, UNDER_REPAIR, OUT_OF_ORDER, RETIRED
- **BookingStatus** «enumeration»: CONFIRMED, ON_TIME, CANCELLED_LATE, RESCHEDULED
- **SessionCategory** «enumeration»: GROUP, PERSONAL, COUPLE
- **MembershipStatus** «enumeration»: ACTIVE, FROZEN, EXPIRED, SUSPENDED

### PaymentGateway «external system»

Interface / external system boundary (dependency target from `Payment`).

- +processPayment() : boolean
- +refund() : String **[UNCLEAR — return type could be boolean]**

### Payment

- -amount : double
- -paymentDate : Date
- -paymentMethod : String
- -paymentStatus : String
- +processPayment() : boolean
- +refund() : String **[UNCLEAR]**

### MembershipPlan

- -planName : String
- -monthlyPrice : double
- -sessionsIncluded : int
- -validityDays : int
- -isActive : boolean
- +getPlanDetails() : String **[UNCLEAR return type]**
- +calculateRenewalPrice() : double **[UNCLEAR — source glyph is close to "int"; a price is more plausibly a double]**

### Membership

- -startDate : Date
- -endDate : Date
- -remainingEntries : int
- -status : MembershipStatus
- +renew(newEndDate : Date) : boolean **[UNCLEAR exact parameter list — a second Date-typed parameter appears to be present in the image]**
- +isExpiringSoon() : boolean
- +deactivate() : void

### HealthDeclaration

- -declarationDate : Date
- -isApproved : boolean
- -riskFactors : String
- +submit() : void
- +getDeclaration() : String **[UNCLEAR return type]**

### Trainee

- -fullName : String
- -phone : String
- -email : String
- -dateOfBirth : Date
- -experienceLevel : String
- -isActive : boolean
- -registrationDate : Date
- +isEligibleForRegistration() : boolean
- +getWorkoutHistory() : List\<WorkoutPerformance\>
- +getMembershipHistory() : List\<Membership\> **[UNCLEAR return type]**
- +updateProfile(...) : void **[UNCLEAR exact parameter list]**

### Trainer

- -fullName : String
- -phone : String
- -email : String
- -employmentType : String
- -hourlyRate : double
- +getSchedule(date : Date) : List\<TrainingSession\> **[UNCLEAR exact parameter list]**
- +isAvailable(date : Date, time : Time) : boolean

### WorkoutPerformance (association class — Trainee ⇄ TrainingSession, taught by Trainer)

- -date : Date
- -weight : double
- -sets : int
- -reps : int
- -caloriesBurned : int
- -notes : String
- +recordScore() : void **[UNCLEAR exact method name]**
- +getWorkoutProgressTrend() : List\<WorkoutPerformance\>

### Registration (association class — Trainee ⇄ TrainingSession)

- -registrationDate : DateTime
- -cancellationDate : DateTime
- -status : RegistrationStatus **[UNCLEAR — attribute may be typed String rather than the enum]**
- +cancel(cancellationDate : DateTime, reason : String) : void

### WaitlistEntry (association class — Trainee ⇄ TrainingSession)

- -position : int
- -joinTimestamp : DateTime
- -status : WaitlistStatus
- -expirationTime : DateTime **[UNCLEAR attribute name]**
- +modify() : void **[UNCLEAR exact method name/signature]**
- +expire() : void

### TrainingSession

- -date : Date
- -startTime : Time
- -sessionCategory : SessionCategory
- -availableSlots : int
- +getRegisteredTrainees() : List\<Trainee\>
- +cancelSession(reason : String) : void

### WorkoutType

- -name : String
- -description : String
- -maxDurationMinutes : int
- -maxCapacity : int
- -isActive : boolean
- +getActiveSessionCount() : int

### Room

- -roomName : String
- -capacity : int
- -isActive : boolean
- +isAvailable(date : Date, time : Time) : boolean
- +getEquipmentList() : List\<Equipment\>

### RoomBooking

- -date : Date
- -startTime : Time
- -endTime : Time
- -status : BookingStatus
- +cancel() : void
- +calculateCancellationFee() : double

### Equipment

- -barcode : String
- -description : String
- -acquisitionDate : Date
- -isActive : boolean
- -condition : EquipmentCondition
- **[UNCLEAR — no methods legible directly on this class; `resolve()`/`escalate()` are attributed to FaultReport below, but could belong to Equipment instead]**

### FaultReport

- -reportDate : Date
- -description : String
- -status : String **[UNCLEAR — may be an enum]**
- +resolve() : void
- +escalate() : void

### MaintenanceRecord

- -startDate : Date
- -endDate : Date
- -status : String **[UNCLEAR type/enum name]**
- **[UNCLEAR — no methods confidently legible on this class]**

## Relationships

> Multiplicities below are read from the diagram where legible; items marked **[UNCLEAR]** could not be confirmed with confidence at the available resolution.

| From | To | Label | Multiplicity (From : To) |
|---|---|---|---|
| Trainee | Membership | "owns" | 1 : 0..* **[UNCLEAR]** |
| Membership | MembershipPlan | "Based on" | 0..* : 1 |
| Membership | Payment | "Paid by" | 1 : 0..* |
| Payment | PaymentGateway | (dependency, external system) | — |
| Trainee | HealthDeclaration | "Submit" | 1 : 0..1 **[UNCLEAR — could be 1:1]** |
| Trainee | Registration | "Registers to" / "Join" (assoc. class over Trainee ⇄ TrainingSession) | 1 : 0..* |
| TrainingSession | Registration | (assoc. class) | 1 : 0..* |
| Trainee | WaitlistEntry | (assoc. class over Trainee ⇄ TrainingSession) | 1 : 0..* |
| TrainingSession | WaitlistEntry | "Has a waitlist" (assoc. class) | 1 : 0..* |
| Trainee | WorkoutPerformance | "Work by" **[UNCLEAR label]** (assoc. class over Trainee ⇄ TrainingSession) | 1 : 0..* |
| TrainingSession | WorkoutPerformance | "Recorded in" (assoc. class) | 1 : 0..* |
| Trainer | WorkoutPerformance | "Taught by" (assoc. class) | 1 : 0..* |
| TrainingSession | Room | "Takes place" | 0..* : 1 |
| TrainingSession | WorkoutType | "Type of" | 0..* : 1 |
| Trainer | RoomBooking | "Booked by" / "Billed to" **[UNCLEAR — two labels appear near this association; may denote two separate associations]** | 1 : 0..* |
| Room | RoomBooking | "Reserved" | 1 : 0..* |
| Room | Equipment | "Contains" | 1 : 0..* |
| Equipment | MaintenanceRecord | "Includes" | 1 : 0..* |
| Equipment | FaultReport | "Has faults" | 1 : 0..* |
| Trainer | FaultReport | "Reported by" **[UNCLEAR — Trainee can also report faults per UC04 in the requirements; the diagram may show this association from Trainer only, or from both]** | 1 : 0..* |
| Trainee | HealthDeclaration | "Monitored by" **[UNCLEAR — label/direction uncertain]** | — |

## Notes on Confidence

The class names, enumeration literals, and the three association-class rationales above are read with high confidence (the surrounding prose on PartB pages 1–2 is plain text, not an image, and corroborates them). The items flagged **[UNCLEAR]** are all within the diagram image itself (page 3), which the source document explicitly flags as needing a higher resolution to read clearly. To fully resolve them, a PNG/SVG export or the original Visual Paradigm `.vpp` file would let us correct: exact multiplicities on several associations, a few method return types/parameter lists, and whether `status` attributes on `Registration`/`WaitlistEntry`/`FaultReport`/`MaintenanceRecord` are typed as the corresponding enumeration or as plain `String`.
