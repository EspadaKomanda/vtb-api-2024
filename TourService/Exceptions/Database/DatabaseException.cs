using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Exceptions.Database
{
    [System.Serializable]
    public class DatabaseException : System.Exception
    {
        public DatabaseException() { }
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, System.Exception inner) : base(message, inner) { }
    }
}