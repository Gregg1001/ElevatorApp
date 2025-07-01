// ====================================
// Program.cs (Strategy + Command + SRP
// ===================================

using System;
using System.Collections.Generic;

/// <summary>
/// Implements Strategy Pattern for test cases, Command Pattern for menu actions,
/// and uses clean SRP structure with dictionary-based routing.
/// </summary>
class Program
{
    // === Command Pattern ===
    // Dictionary maps menu inputs to executable commands (methods).
    private static readonly Dictionary<string, Action> MainMenuActions = new()
    {
        { "1", RunManualInput },          // Command: Run manual elevator simulation
        { "2", ShowTestCaseMenu },        // Command: Launch test case selection menu
        { "e", () => Environment.Exit(0) } // Command: Exit application
    };

    // === Strategy Pattern ===
    // Each test case implements a different strategy via ITestCaseStrategy.
    private static readonly Dictionary<string, ITestCaseStrategy> TestCaseStrategies = new()
    {
        { "1", new TestCase1Strategy() },
        { "2", new TestCase2Strategy() },
        { "3", new TestCase3Strategy() },
        { "4", new TestCase4Strategy() }
    };

    // === SRP ===
    // Main method only starts the program, delegates control to menu loop.
    static void Main(string[] args)
    {
        RunMainMenu();  // SRP: Separated from logic of what happens next
    }

    // === SRP + Dictionary-Based Routing ===
    // Displays main menu and invokes corresponding action via dictionary
    private static void RunMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Elevator Simulation ===");
            Console.WriteLine("1. Manual Input");
            Console.WriteLine("2. Run Test Case");
            Console.WriteLine("e. Exit");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim().ToLower();

            // === Command Execution ===
            if (choice != null && MainMenuActions.TryGetValue(choice, out var action))
            {
                action.Invoke(); // Invokes bound command (Command Pattern)
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }

    // === SRP ===
    // Handles all logic for manually entering elevator requests
    private static void RunManualInput()
    {
        var elevator = Elevator.Instance; // === Singleton Pattern ===
        elevator.ResetToGround();

        Console.Write("Enter number of external requests: ");
        var input = Console.ReadLine()?.Trim();

        if (!int.TryParse(input, out int externalCount) || externalCount <= 0)
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        var externalRequests = new List<Request>();

        for (int i = 0; i < externalCount; i++)
        {
            int floor;
            Direction direction;

            // Input: Floor
            while (true)
            {
                Console.Write($"Request {i + 1} - Floor you're on (0-10): ");
                var floorInput = Console.ReadLine()?.Trim();
                if (floorInput == "e") return;

                if (!int.TryParse(floorInput, out floor) || floor < 0 || floor > 10)
                {
                    Console.WriteLine("Invalid floor.");
                    continue;
                }

                break;
            }

            // Input: Direction
            while (true)
            {
                Console.Write($"Request {i + 1} - Direction (up/down): ");
                var dirInput = Console.ReadLine()?.Trim().ToLower();

                if (dirInput != "up" && dirInput != "down")
                {
                    Console.WriteLine("Invalid direction.");
                    continue;
                }

                direction = dirInput == "up" ? Direction.Up : Direction.Down;

                if (direction == Direction.Down && floor == 0)
                {
                    Console.WriteLine("You're on Ground Floor. Can't go down.");
                    continue;
                }

                if (direction == Direction.Up && floor == 10)
                {
                    Console.WriteLine("You're on Top Floor. Can't go up.");
                    continue;
                }

                break;
            }

            var request = new Request { Floor = floor, Direction = direction };
            elevator.AddRequest(request);
            Console.WriteLine($"Passenger externally pressed \"{direction}\" at floor {floor}.");
        }

        // Run SCAN algorithm with external requests
        Console.WriteLine("\nRunning elevator...");
        elevator.Run();

        // Input internal destinations
        Console.Write("\nEnter number of internal destinations: ");
        var destInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(destInput, out int internalCount) || internalCount <= 0) return;

        for (int i = 0; i < internalCount; i++)
        {
            while (true)
            {
                Console.Write($"Destination {i + 1} (0-10): ");
                var destFloorInput = Console.ReadLine()?.Trim();
                if (!int.TryParse(destFloorInput, out int destFloor) || destFloor < 0 || destFloor > 10)
                {
                    Console.WriteLine("Invalid floor.");
                    continue;
                }

                if (destFloor == elevator.CurrentFloor)
                {
                    Console.WriteLine("Already at that floor. Choose another.");
                    continue;
                }

                elevator.AddInternalDestination(destFloor);
                Console.WriteLine($"Passenger internally selected \"{destFloor}\".");
                break;
            }
        }

        Console.WriteLine("\nRunning elevator...");
        elevator.Run();
    }

    // === Strategy Pattern ===  
    // Runs pre-defined test case scenarios by executing selected strategy  
    private static void ShowTestCaseMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Test Cases ===");
            Console.WriteLine("1. Passenger summons lift on ground floor and selects L5.");
            Console.WriteLine("2. Passengers at L6 and L4 summon down and go to L1.");
            Console.WriteLine("3. L2 up to L6 and L4 down to Ground.");
            Console.WriteLine("4. G to L5, L4 and L10 down to G.");
            Console.WriteLine("b. Back to Main Menu");
            Console.Write("> ");

            var input = Console.ReadLine()?.Trim().ToLower();

            // Back out  
            if (input == "b")
                return;

            // === Strategy Execution ===  
            if (!string.IsNullOrEmpty(input) && TestCaseStrategies.TryGetValue(input, out var strategy))
            {
                strategy.Execute(); // Executes selected strategy  
                Console.WriteLine($"\nTest Case {input} completed.");
            }
            else
            {
                Console.WriteLine("Invalid test case selection.");
            }
        }
    }
}
