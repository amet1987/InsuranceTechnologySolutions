namespace Shared.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    public ValidationException() : base("Invalid data - see errors") {}
    public ValidationException(string message) : base(message) {}

    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
}
