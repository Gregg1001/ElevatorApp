// ==================================================================
// Elevator.cs (Singleton + SCAN Algorithm + Open/Closed Principle)
// ==================================================================
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This is the one and only Elevator in the building (Singleton pattern).
/// It follows the SCAN algorithm to pick up and drop off people logically.
/// </summary>
public sealed class Elevator
{
    // This ensures only one Elevator exists (Singleton pattern).
    private static readonly Lazy<Elevator> _instance = new(() => new Elevator());
    public static Elevator Instance => _instance.Value;

    // Private constructor so no one else can create a new Elevator.
    private Elevator() { }

    // This keeps track of where the elevator is right now.
    public int CurrentFloor { get; private set; } = 0;

    // This tells us which direction the elevator is going (up, down, or stopped).
    public Direction CurrentDirection { get; private set; } = Direction.Idle;

    // People who want to go UP (list of floors)
    private readonly List<int> UpQueue = new();

    // People who want to go DOWN (list of floors)
    private readonly List<int> DownQueue = new();

    // Handles a request using Open/Closed Principle.
    // Accepts any IRequest implementation (e.g., TimeRequest, EmergencyRequest) without modifying Elevator logic.
    public void AddRequest(IRequest request)
    {
        if (request.Floor > CurrentFloor)
            UpQueue.Add(request.Floor); // Going up
        else if (request.Floor < CurrentFloor)
            DownQueue.Add(request.Floor); // Going down
        else
            OpenDoor(); // Already on this floor, just open the door

        // Sort the Up list from lowest to highest
        UpQueue.Sort();

        // Sort the Down list from highest to lowest
        DownQueue.Sort((a, b) => b.CompareTo(a));
    }

    // Someone *inside* the elevator pressed a button for a floor
    public void AddInternalDestination(int floor)
    {
        // Ignore if we're already on the requested floor
        if (floor == CurrentFloor) return;

        // Add to the appropriate queue
        if (floor > CurrentFloor) UpQueue.Add(floor);
        else DownQueue.Add(floor);

        UpQueue.Sort();
        DownQueue.Sort((a, b) => b.CompareTo(a));
    }

    // This is the main engine: it runs until all requests are complete
    public void Run()
    {
        // Keep going while there are any requests left
        while (UpQueue.Any() || DownQueue.Any())
        {
            // If not moving, pick a direction
            if (CurrentDirection == Direction.Idle)
            {
                if (UpQueue.Any()) CurrentDirection = Direction.Up;
                else if (DownQueue.Any()) CurrentDirection = Direction.Down;
                else break; // Nothing to do
            }

            // Handle going up
            if (CurrentDirection == Direction.Up)
            {
                // Get all stops that are above or at our current floor
                var stops = UpQueue.Where(f => f >= CurrentFloor).ToList();

                foreach (var stop in stops)
                {
                    MoveTo(stop);      // Travel to that floor
                    OpenDoor();        // Let people in/out
                    UpQueue.Remove(stop); // Remove stop from the queue
                }

                // Done going up, now switch direction if needed
                CurrentDirection = DownQueue.Any() ? Direction.Down : Direction.Idle;
            }
            // Handle going down
            else if (CurrentDirection == Direction.Down)
            {
                // Get all stops that are below or at our current floor
                var stops = DownQueue.Where(f => f <= CurrentFloor).ToList();

                foreach (var stop in stops)
                {
                    MoveTo(stop);
                    OpenDoor();
                    DownQueue.Remove(stop);
                }

                // Done going down, now switch direction if needed
                CurrentDirection = UpQueue.Any() ? Direction.Up : Direction.Idle;
            }
        }

        // Done with all requests
        Console.WriteLine($"\nElevator is now idle at floor {CurrentFloor}");
    }

    // Actually change floors (simulate movement)
    private void MoveTo(int targetFloor)
    {
        Console.WriteLine($"Moving from Floor {CurrentFloor} to Floor {targetFloor}");
        CurrentFloor = targetFloor;
    }

    // Simulate the door opening
    private void OpenDoor()
    {
        Console.WriteLine($"Opening doors at Floor {CurrentFloor}");
    }

    // Reset everything to start fresh from Ground Floor
    public void ResetToGround()
    {
        CurrentFloor = 0;
        CurrentDirection = Direction.Idle;
        UpQueue.Clear();
        DownQueue.Clear();
    }
}
