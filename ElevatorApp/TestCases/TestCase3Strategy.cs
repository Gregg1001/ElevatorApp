/// <summary>
/// Strategy for Test Case 3: L2 up to L6, L4 down to G.
/// </summary>
public class TestCase3Strategy : ITestCaseStrategy
{
    public void Execute()
    {
        Console.WriteLine("Test Case 3: Passenger 1 summons lift to go up from L2. Passenger 2 summons lift to go down from L4. Passenger 1 chooses to go to L6. Passenger 2 chooses to go to Ground Floor");
        var elevator = Elevator.Instance;

        var passengers = new List<Passenger>
        {
            new Passenger { StartFloor = 2, Direction = Direction.Up, Destination = 6 },
            new Passenger { StartFloor = 4, Direction = Direction.Down, Destination = 0 }
        };

        foreach (var p in passengers)
            elevator.AddRequest(p.GetCallRequest());

        elevator.Run();

        foreach (var p in passengers)
            elevator.AddInternalDestination(p.Destination);

        elevator.Run();

    }
}