namespace Mocale.Exceptions;

public class NotInitializedException : Exception
{
    public NotInitializedException()
        : base("Mocale must be initialized before localizations can be provided")
    {
    }
}
