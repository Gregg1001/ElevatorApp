// ================================================================
// Passenger.cs (POCO - Generates Request from passenger input)
// ================================================================

/// <summary>
/// Represents an external elevator request.
/// Implements IRequest interface in Strategy Pattern.
/// </summary>
public class Request : IRequest
{
    public int Floor { get; set; }
    public Direction Direction { get; set; }
}