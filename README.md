<div align="center">

<img src="docs/images/banner.svg" alt="LEVEL UP Studio Management System" width="100%">

<br>

![C#](https://img.shields.io/badge/C%23-.NET%208-B8A9F0?style=flat-square&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Azure%20SQL-8FD3E8?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Google APIs](https://img.shields.io/badge/Google-Calendar%20%26%20Tasks-A8E0C5?style=flat-square&logo=googlecalendar&logoColor=white)
![UML](https://img.shields.io/badge/UML-Visual%20Paradigm-FFC9B0?style=flat-square)
![Team](https://img.shields.io/badge/Team%20of-4-F7D794?style=flat-square)

**One system that replaces a whiteboard, WhatsApp reminders, and a side Excel sheet.**
Class registration · waitlists · memberships · payments · room rentals

</div>

> [!NOTE]
> **Software Analysis & Design course, Ben-Gurion University · 2026**
> **My role:** I took part in every stage of the project, end to end:
> - **Current-state analysis:** interviewed the studio manager, trainers, and trainees, and ran a comprehensive as-is study that included on-site observation and sampling of the existing tools.
> - **System design:** designed the new system from the findings of that analysis, turning the identified problems into requirements, use cases, and a data model.
> - **Implementation:** built the system together with Claude (AI-assisted development), working from the design artifacts.

<!-- Add 2-3 screenshots here: home dashboard, registration panel, waitlist panel.
     <img src="docs/images/dashboard.png" width="80%"> -->

## The problem

LEVEL UP is a small fitness studio in Be'er Sheva (personal, couples, and small-group training, plus room rental to independent trainers). It ran on a commercial booking app plus manual work: a whiteboard for training plans, WhatsApp for reminders, and a parallel Excel sheet for scheduling.

Interviews and on-site observation pointed to the same root causes:

| | Root cause | What it looked like |
|---|---|---|
| 🧍 | **One person carrying everything** | The owner, who is also the lead trainer, scheduled rooms and trainers by hand |
| 👀 | **No visibility** | Empty classes were cancelled while others had long waitlists, and trainees dropped out when no class fit their routine |
| 📋 | **Missing records** | Health declarations, training history, and equipment faults were verbal or undocumented |
| 💳 | **Billing friction** | Renting trainers' bookings and cancellation fees were tracked by hand every month |

## The solution

One system that connects trainees, studio trainers, renting trainers, and management.

| Area | What it does |
|---|---|
| 🗓️ **Registration & waitlist** | Self-registration into classes, automatic waitlist, promotion when a spot frees up, late-cancellation handling |
| 🎟️ **Memberships & payments** | Membership and punch-card plans, freeze / suspend / expire lifecycle, payment tracking |
| 🩺 **Health & performance** | Digital health declaration before the first class, per-session workout performance records |
| 🏠 **Rooms & renters** | Room booking with conflict prevention, monthly billing report for renting trainers |
| 🔧 **Equipment** | Fault reporting and equipment condition tracking |
| 🔗 **Integrations** | Google Calendar and Google Tasks sync, email notifications |

### Registration lifecycle

The registration entity implements the state machine from the design phase, with a guard on every transition.

```mermaid
flowchart LR
    P([Pending]) -->|payment processed| C([Confirmed])
    P -->|class full| W([Waitlisted])
    W -->|spot frees up| C
    C -->|class starts| A([Active])
    A -->|class ends| D([Completed])
    C -->|missed class| N([No-show])
    C -->|cancel, 12h+ ahead| X([Cancelled])
    C -->|cancel, under 12h| L[Late fee] --> X
    W -->|cancel| X

    classDef start fill:#FFF3C4,stroke:#E8C86A,color:#4A3F12
    classDef good fill:#C8F1E4,stroke:#7FCBB2,color:#1F4A3D
    classDef wait fill:#DCD6F7,stroke:#A99BE0,color:#2F2760
    classDef bad fill:#FFDCCB,stroke:#F0A98A,color:#5A2E18
    class P start
    class C,A,D good
    class W wait
    class N,X,L bad
```

## From analysis to code

The project follows the order **organization analysis → requirements → use cases → design → code**, and every stage is documented in [`docs/`](docs).

| Stage | Artifact |
|---|---|
| 🔍 Current-state analysis | [`docs/org-analysis/`](docs/org-analysis): organization, interviews, problems, business processes |
| 📝 Requirements | [`docs/00-requirements.md`](docs/00-requirements.md): 35 user stories with MUST / SHOULD / COULD priorities, functional and non-functional |
| 🎯 Use cases | [`docs/00e-use-cases.md`](docs/00e-use-cases.md) |
| 🧩 Design | [`docs/Class Diagram.png`](docs/Class%20Diagram.png), [`docs/design/`](docs/design) |
| 🗄️ Database | [`scripts/`](scripts): schema, stored procedures, seed data |

## Technical highlights

- **Layered, entity-based architecture.** Each entity class owns its fields, validation, and stored-procedure calls. All database access goes through stored procedures.
- **State machines from the design.** `Membership` and `Registration` implement the lifecycle transitions modeled in the UML state diagrams.
- **Resilience to Azure SQL Serverless auto-pause.** The data layer retries transient connection errors with growing back-off, so the first request after idle does not fail in front of the user.
- **Controlled enumerations.** Fixed value sets use `CHECK` constraints instead of lookup tables.
- **Role-based access.** Users see only the screens relevant to their role.

## Tech stack

C# · .NET 8 (Windows Forms) · SQL Server / Azure SQL · T-SQL stored procedures · Google Calendar & Tasks APIs · MailKit · UML (Visual Paradigm)

## How to run

1. **Requirements:** Windows, .NET 8 SDK, SQL Server or Azure SQL.
2. **Create the database:** run `scripts/create_database.sql`, then `scripts/stored_procedures.sql`, then optionally `scripts/seed_data.sql` for sample data.
3. **Configure** an `App.config` next to the project (it is git-ignored) with your own connection string and SMTP settings. Never commit credentials.
4. **Optional Google integration:** place your own `credentials.json` (Google OAuth client) in the output folder.
5. **Run:** open `LevelUp_Management.sln` in Visual Studio and press F5.

> [!TIP]
> The seed data is entirely fictional. The sample login `admin / admin123` is for local testing only.

## Notes

- The studio's data, names, and interview participants are anonymized in this repository.
- Course materials and graded submissions are intentionally not included.
