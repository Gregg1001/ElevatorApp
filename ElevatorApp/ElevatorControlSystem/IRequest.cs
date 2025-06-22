// ==============================
// IRequest.cs (Strategy Pattern Interface)
// ==============================

/// <summary>
/// Defines a contract for an elevator request.
/// Part of the Strategy Pattern used for request handling.
/// </summary>
public interface IRequest
{
    int Floor { get; set; }
    Direction Direction { get; set; }
}