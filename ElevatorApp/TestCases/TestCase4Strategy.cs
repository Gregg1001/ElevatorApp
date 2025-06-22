/// <summary>
/// Strategy for Test Case 4: G up to L5, L4 and L10 down to G.
/// </summary>
public class TestCase4Strategy : ITestCaseStrategy
{
    public void Execute()
    {
        Console.WriteLine("Test Case 4: Passenger 1 summons lift to go up from Ground. They choose L5. Passenger 2 summons lift to go down from L4. Passenger 3 summons lift to go down from L10. Passengers 2 and 3 choose to travel to Ground.");
        var elevator = Elevator.Instance;

        var passengers = new List<Passenger>
        {
            new Passenger { StartFloor = 0, Direction = Direction.Up, Destination = 5 },
            new Passenger { StartFloor = 4, Direction = Direction.Down, Destination = 0 },
            new Passenger { StartFloor = 10, Direction = Direction.Down, Destination = 0 }
        };

        foreach (var p in passengers)
            elevator.AddRequest(p.GetCallRequest());

        elevator.Run();

        foreach (var p in passengers)
            elevator.AddInternalDestination(p.Destination);

        elevator.Run();

    }
}
