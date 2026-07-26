using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
    }

    public class ConflictException(string message) : Exception(message)
    {
    }

    public class BusinessRulesException(string message) : Exception(message)
    {
    }
}
