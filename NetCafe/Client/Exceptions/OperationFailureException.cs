﻿namespace NetCafe.Client.Exceptions;

public class OperationFailureException : Exception
{
    public OperationFailureException(string message) : base(message)
    {

    }
}
