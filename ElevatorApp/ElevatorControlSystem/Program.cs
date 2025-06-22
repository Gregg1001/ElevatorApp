// ==============================
// Program.cs
// ==============================

using System;
using System.Collections.Generic;

/// <summary>
/// This is where the app starts running.
/// It shows a main menu where you choose what to do: manual run, test case, or exit.
/// </summary>
class Program
{
    // This dictionary links user menu choices (like "1", "2", "e") to actual actions (functions).
    private static readonly Dictionary<string, Action> MainMenuActions = new()
    {
        { "1", RunManualInput },          // Option 1: You control the elevator manually
        { "2", ShowTestCaseMenu },        // Option 2: Run preset examples
        { "e", () => Environment.Exit(0) } // Option e: Exit the program
    };

    // This dictionary connects test case numbers to their matching strategy class
    private static readonly Dictionary<string, ITestCaseStrategy> TestCaseStrategies = new()
    {
        { "1", new TestCase1Strategy() },
        { "2", new TestCase2Strategy() },
        { "3", new TestCase3Strategy() },
        { "4", new TestCase4Strategy() }
    };

    // Main function: this runs first when the program starts
    static void Main(string[] args)
    {
        RunMainMenu();  // Show the main menu loop
    }

    // Main menu screen where user picks what to do
    private static void RunMainMenu()
    {
        while (true) // Loop until the user exits
        {
            Console.WriteLine("\n=== Elevator Simulation ===");
            Console.WriteLine("1. Manual Input");
            Console.WriteLine("2. Run Test Case");
            Console.WriteLine("e. Exit");
            Console.Write("> ");

            var choice = Console.ReadLine()?.Trim().ToLower();

            // Check if choice is valid and run the matching action
            if (choice != null && MainMenuActions.TryGetValue(choice, out var action))
            {
                action.Invoke(); // Run the chosen function
            }
            else
            {
                Console.WriteLine("Invalid option."); // Wrong input
            }
        }
    }

    // This is where the user controls the elevator manually
    private static void RunManualInput()
    {
        var elevator = Elevator.Instance; // Get the singleton instance
        elevator.ResetToGround(); // Reset to floor 0

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

            // STEP 1: Ask which floor the person is on
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

            // STEP 2: Ask which direction they want to go
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

        // First, handle all external pickups
        Console.WriteLine("\nRunning elevator...");
        elevator.Run();

        // THEN ask for internal destinations
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

        // Now run the elevator again to handle internal requests
        Console.WriteLine("\nRunning elevator...");
        elevator.Run();
    }


    // Shows a submenu of preset examples (test cases)
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

            // If user types "b", go back to main menu
            if (input == "b")
                return;

            // Run selected test case strategy
            if (TestCaseStrategies.TryGetValue(input, out var strategy))
            {
                strategy.Execute(); // Execute that test case
                Console.WriteLine($"\nTest Case {input} completed.");
            }
            else
            {
                Console.WriteLine("Invalid test case selection.");
            }
        }
    }


}
