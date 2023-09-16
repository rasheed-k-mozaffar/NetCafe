namespace NetCafe.Server;

public class RecordDeletionFailedException : Exception
{
    public RecordDeletionFailedException(string message) : base(message)
    {

    }
}
