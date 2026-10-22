Source: `Membership State Diagram.pdf` and `Registration State Diagram.pdf` (docs/), both
Visual Paradigm exports (Ben Chouraqui, Ben Gurion University). Both diagrams render at
full legibility — no [UNCLEAR] items on state names, entry/do actions, or event/guard
text. A small number of transitions are drawn as **group transitions** (arrow leaves the
border of a composite state rather than an individual substate box); where the diagram is
genuinely ambiguous about which specific substate a group transition fires from, that is
called out explicitly below rather than silently resolved.

## Cross-Artifact Note — Enum Value Sets Are Out of Sync

Both diagrams use more status values than the enums currently declared in `CLAUDE.md` /
`docs/design/class-diagram.md` and implemented in `MembershipStatus.cs` /
`RegistrationStatus.cs`:

- **`MembershipStatus`** — code/CLAUDE.md today: `ACTIVE, EXPIRED, SUSPENDED`. The state
  diagram adds **`FROZEN`** as a distinct status (entered via `freeze [userRequested]`,
  left via `unfreeze`).
- **`RegistrationStatus`** — code/CLAUDE.md today: `CONFIRMED, WAITLISTED, CANCELLED,
  NO_SHOW`. The state diagram adds **`PENDING`** (initial status, before payment/capacity
  is resolved), **`ACTIVE`** (class in progress, distinct from `CONFIRMED`), and
  **`COMPLETED`** (class finished and attended).

Per the Enumerations pattern in `CLAUDE.md`, the C# `enum` and the DB `CHECK` constraint
must stay identical, and this transcription is not authorization to change either — it's
a record of the mismatch for the group to resolve deliberately (either update the enums
and CHECK constraints to match the finalized state diagrams, or treat the diagrams as
aspirational/future scope and note the gap). Flagging here rather than editing
`MembershipStatus.cs` / `RegistrationStatus.cs` / `class-diagram.md` directly.

---

## 1. Membership State Diagram

Root composite state **Registered Account** contains nested composite **Valid
Membership** (substates `Active`, `FROZEN`) plus sibling state `SUSPENDED`. `EXPIRED`
sits outside `Registered Account` as a top-level state, with `Final` outside that again.

### States

| State | Entry / Do |
|---|---|
| **Registered Account** *(composite)* | — (container only; own nested initial routes to `Valid Membership`) |
| **Valid Membership** *(composite, nested in Registered Account)* | — (container only; own nested initial routes to `Active`) |
| **Active** *(nested in Valid Membership)* | entry `/ setStatus(ACTIVE)` — do `/ calculateValidity()` |
| **FROZEN** *(nested in Valid Membership)* | entry `/ setStatus(FROZEN); freezeAccess()` |
| **SUSPENDED** *(sibling of Valid Membership, inside Registered Account)* | entry `/ setStatus(SUSPENDED)` — do `/ checkPayment()` |
| **EXPIRED** *(top-level, outside Registered Account)* | entry `/ setStatus(EXPIRED); archiveMembership()` |
| **Final** | — |

### Transitions

| # | Source | Target | Trigger | Guard | Effect |
|---|---|---|---|---|---|
| 1 | *(initial)* | Registered Account | Initiate | — | — |
| 2 | *(nested initial, inside Registered Account)* | Valid Membership | Initiate | — | — |
| 3 | *(nested initial, inside Valid Membership)* | Active | Initiate | — | — |
| 4 | Active | FROZEN | freeze | `[userRequested]` | — |
| 5 | FROZEN | Active | unfreeze | — | — |
| 6 | Valid Membership *(group transition — fires from either Active or FROZEN)* | SUSPENDED | suspend | `[paymentFailed]` | — |
| 7 | Valid Membership *(group transition — fires from either Active or FROZEN)* | EXPIRED | expire | `[endDateReached]` | — |
| 8 | EXPIRED | Final | after(7 years) | — | `/ purge` |
| 9 | SUSPENDED | Final | after(7 years) | — | `/ purge` |

Notes:
- Transitions 6 and 7 are drawn leaving the border of the **Valid Membership** composite
  box (not from `Active` or `FROZEN` individually), which in UML state-machine notation is
  a group transition — it applies uniformly to every substate of `Valid Membership`. In
  practice: payment failure suspends the membership whether it's currently active or
  frozen, and reaching the end date expires it either way.
- There is no transition back out of `SUSPENDED` to `Active`/`FROZEN` in the diagram —
  once suspended, the only outgoing transition shown is the 7-year purge to `Final`. If
  a "reinstate after payment" path is intended, it isn't represented in this diagram and
  should be raised with the group before implementation.
- `Registered Account` and `Valid Membership` have no `entry`/`do` of their own — all
  behavior lives on the leaf states.

---

## 2. Registration State Diagram

Top-level states `Pending`, `Waitlisted`, `Cancelled`, `No_Show`, `Completed`, and
composite **ActiveBooking** (substates `Confirmed`, `Active`), with `Final` outside all
of them.

### States

| State | Entry / Do |
|---|---|
| **Pending** | entry `/ setStatus(PENDING); createRegistration()` |
| **Waitlisted** | entry `/ setStatus(WAITLISTED)` |
| **ActiveBooking** *(composite)* | — (container only; own nested initial routes to `Confirmed`) |
| **Confirmed** *(nested in ActiveBooking)* | entry `/ setStatus(CONFIRMED)` |
| **Active** *(nested in ActiveBooking)* | entry `/ setStatus(ACTIVE)` — do `/ trackAttendance()` |
| **Cancelled** | entry `/ setStatus(CANCELLED)` |
| **No_Show** | entry `/ setStatus(NO_SHOW)` |
| **Completed** | entry `/ setStatus(COMPLETED)` |
| **Final** | — |

### Transitions

| # | Source | Target | Trigger | Guard | Effect |
|---|---|---|---|---|---|
| 1 | *(initial)* | Pending | — | — | — |
| 2 | Pending | Waitlisted | classFull | — | — |
| 3 | Pending | Cancelled | cancel | — | — |
| 4 | Waitlisted | Cancelled | cancel | — | — |
| 5 | Pending | ActiveBooking *(enters at nested initial → Confirmed)* | confirm | `[paymentProcessed]` | — |
| 6 | *(nested initial, inside ActiveBooking)* | Confirmed | — | — | — |
| 7 | Confirmed | Active | startClass | `[isClassTime]` | — |
| 8 | Confirmed | No_Show | missClass | — | — |
| 9 | ActiveBooking *(group transition — see note)* | Cancelled | cancel | `[over 12 hours]` | — |
| 10 | ActiveBooking *(group transition — see note)* | Cancelled | cancel | `[under 12 hours]` | `/ chargeLateFee` |
| 11 | Active | Completed | finishClass | — | — |
| 12 | Cancelled | Final | after(7 years) | — | `/ purge` |
| 13 | No_Show | Final | after(7 years) | — | `/ purge` |
| 14 | Completed | Final | after(7 years) | — | `/ purge` |

Notes:
- **Verified against the original Visual Paradigm PDF at high zoom (resolves the
  ambiguity previously flagged here for #8–#10).** `startClass` (#7) and `missClass`
  (#8) both originate from the exact same point on `Confirmed`'s own bottom border —
  they are sibling transitions out of `Confirmed`, drawn running visually
  alongside/behind the `Active` box, with `missClass`'s line continuing down past the
  composite's boundary to `No_Show`. Transition #8's source is therefore `Confirmed`,
  not a group transition off `ActiveBooking` — corrected in the table above. This also
  makes semantic sense: `Confirmed` splits into either attending (`startClass` →
  `Active`) or missing the class entirely (`missClass` → `No_Show`); a trainee already
  inside `Active` (attendance tracking underway via `do / trackAttendance()`) can no
  longer be marked a no-show for a class they're actively attending.
- **Transitions 9–10 remain genuine group transitions off `ActiveBooking`** — verified
  separately: their lines originate from the composite's own border (bottom-left
  corner), a point that doesn't align with either inner box. Taken literally, that means
  a trainee can still rack up a guarded `cancel` (with or without `chargeLateFee`) from
  either `Confirmed` or `Active`.
- **Implementation-scoping decision for #9–10 (not a transcription correction):** the
  diagram genuinely draws #9–10 as a group transition off `ActiveBooking` per the note
  above — that has not changed. However, the group has decided to scope both to
  `Confirmed` only when this is implemented, because the guards measure time remaining
  before class start (`[over 12 hours]` / `[under 12 hours]`): once a booking reaches
  `Active`, the class has already started, so "hours until start" is already negative —
  `[over 12 hours]` could never fire from `Active`, and cancelling an already-in-progress,
  attendance-tracked class doesn't fit the domain. This also matches the resolved
  `missClass` picture (#8 above): `Confirmed` is the single decision point with four
  possible branches (`startClass`, `missClass`, `cancel` early, `cancel` late), while
  `Active` has exactly one exit (`finishClass`). Any code/SP built from this diagram
  should therefore implement #9–10 as originating from `Confirmed`, while this table
  keeps recording what the source diagram literally draws.
- Transitions 3 and 4 are both a bare, unguarded `cancel` — one from `Pending` (before
  capacity/payment is resolved), one from `Waitlisted` (while queued). Neither charges a
  late fee; that only appears on the guarded `cancel [under 12 hours]` out of
  `ActiveBooking` (#10).
- `ActiveBooking` has no `entry`/`do` of its own — all behavior lives on `Confirmed` and
  `Active`.
