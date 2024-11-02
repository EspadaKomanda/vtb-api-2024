﻿namespace OpenApiService.KafkaException.ConsumerException;

public class ConsumerRecievedMessageInvalidException : ConsumerException
{
    public ConsumerRecievedMessageInvalidException()
    {
    }

    public ConsumerRecievedMessageInvalidException(string message)
        : base(message)
    {
    }

    public ConsumerRecievedMessageInvalidException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}