namespace NetCafe.Server.Exceptions;

public class DataInsertionFailedException : Exception
{
    public DataInsertionFailedException(string message) : base(message)
    {
    }
}
