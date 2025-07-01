# Elevator Control System

This is a C# console application that simulates an elevator system for a 10-storey building. It supports both **manual mode** and **test case mode** for simulating elevator requests.

---

## Folder Structure

```
ElevatorControlSystem/
    ├── Interface/
    │   └── IRequest.cs             // Interface for requests
    ├── Program.cs                  // Entry point and UI navigation
    ├── Elevator.cs                 // Core logic, elevator state & SCAN algorithm
    ├── ElevatorRunner.cs           // Main executor for test strategy selection
    ├── Request.cs                  // POCO for external requests
    ├── Passenger.cs                // POCO for passenger with start/destination
    ├── Direction.cs                // Enum: Up, Down, Idle
    ├── ManualInputHandler.cs       // Manual user input logic

TestCases/
    ├── Interface/
    │   └── ITestCaseStrategy.cs    // Strategy pattern interface for test execution
    ├── TestCase1Strategy.cs        // Ground floor → Level 5
    ├── TestCase2Strategy.cs        // Level 6 + Level 4 → Level 1
    ├── TestCase3Strategy.cs        // L2 → L6, L4 → Ground
    ├── TestCase4Strategy.cs        // Ground → L5, L4 + L10 → Ground

README.md                           // Project documentation

```

---

## How to Run

1. **Open the solution in Visual Studio** (or run via terminal if using `dotnet` CLI).

2. **Build and run the application**:

```
> dotnet run
```

---

## Main Menu Options

When the app starts, you'll see:

```
Select mode:
1 - Manual Input
2 - Run Test Case
E - Exit Program
> 
```

You can type:
- `1` to enter **Manual Input Mode**
- `2` to run a **predefined test case**
- `E` to **exit** the application

---

## Manual Input Mode

You're asked:
```
=== Elevator Simulation ===
1. Manual Input
2. Run Test Case
e. Exit
> 1
Enter number of external requests: 1
Request 1 - Floor you're on (0-10): 0
Request 1 - Direction (up/down): up
Passenger externally pressed "Up" at floor 0.
Opening doors at Floor 0
Enter number of internal destinations: 1
Destination 1 (0-10): 10
Passenger internally selected "10".

Running elevator...

Moving from Floor 0 to Floor 10
Opening doors at Floor 10

Elevator is now idle at floor 10
```

3. The elevator then simulates the movement, logging each stop and direction change.

---

## Test Case Mode

After choosing option `2`, you’ll be asked:

```
Select Test Case:
=== Test Cases ===
1. Passenger summons lift on ground floor and selects L5.
2. Passengers at L6 and L4 summon down and go to L1.
3. L2 up to L6 and L4 down to Ground.
4. G to L5, L4 and L10 down to G.
b. Back to Main Menu
> 
```

Type `1`, `2`, `3`, or `4` to run one of the built-in simulations. The elevator will move through floors, and you’ll see output like:

```

> 1
Test Case 1: Passenger summons lift on the ground floor. Once in, choose to go to level 5.
Moving from Floor 10 to Floor 0
Opening doors at Floor 0

Elevator is now idle at floor 0
Moving from Floor 0 to Floor 5
Opening doors at Floor 5

Elevator is now idle at floor 5

Test Case 1 completed.


```

---

## Features

- Supports **multiple passengers**
- Models both **external requests** and **internal destinations**
- Uses SCAN-style logic to optimize elevator path
- Rejects invalid directions (e.g. down from Ground)

---

## Summary

This app demonstrates scheduling, sorting, and user input handling in a console UI. It reflects realistic elevator decision-making logic while keeping the interface simple.

Test cases are easily extensible and modular, and the system is suitable for future upgrades like handling multiple elevators, better scheduling heuristics, or GUI integration.

---

## Requirements

- .NET 6 or later
- A C# compiler or IDE like Visual Studio, VS Code

---

## End