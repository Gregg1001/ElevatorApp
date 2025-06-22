/// <summary>
/// Strategy for Test Case 1: Ground floor to Level 5.
/// </summary>
public class TestCase1Strategy : ITestCaseStrategy
{
    public void Execute()
    {
        Console.WriteLine("Test Case 1: Passenger summons lift on the ground floor. Once in, choose to go to level 5.");
        var elevator = Elevator.Instance;

        var passengers = new List<Passenger>
        {
            new Passenger { StartFloor = 0, Direction = Direction.Up, Destination = 5 }
        };

        foreach (var p in passengers)
            elevator.AddRequest(p.GetCallRequest());

        elevator.Run();

        foreach (var p in passengers)
            elevator.AddInternalDestination(p.Destination);

        elevator.Run();

    }
}