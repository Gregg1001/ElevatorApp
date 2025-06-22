// ========================================
// Passenger.cs (POCO + Strategy Provider)
// ========================================

/// <summary>
/// Represents a passenger who generates a request.
/// This class acts as a provider of IRequest for the Elevator.
/// </summary>
public class Passenger
{
    public int StartFloor { get; set; }
    public Direction Direction { get; set; }
    public int Destination { get; set; }

    /// <summary>
    /// Returns the elevator request to be submitted.
    /// </summary>
    public IRequest GetCallRequest() => new Request { Floor = StartFloor, Direction = Direction };
}
