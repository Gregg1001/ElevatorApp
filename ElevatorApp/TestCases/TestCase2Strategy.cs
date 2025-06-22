/// <summary>
/// Strategy for Test Case 2: Level 6 and 4 to Level 1.
/// </summary>
public class TestCase2Strategy : ITestCaseStrategy
{
    public void Execute()
    {
        Console.WriteLine("Test Case 2: Passenger summons lift on level 6 to go down. Passenger on level 4 summons the lift to go down. They both choose L1.");
        var elevator = Elevator.Instance;

        var passengers = new List<Passenger>
        {
            new Passenger { StartFloor = 6, Direction = Direction.Down, Destination = 1 },
            new Passenger { StartFloor = 4, Direction = Direction.Down, Destination = 1 }
        };

        foreach (var p in passengers)
            elevator.AddRequest(p.GetCallRequest());

        elevator.Run();

        foreach (var p in passengers)
            elevator.AddInternalDestination(p.Destination);

        elevator.Run();

    }
}