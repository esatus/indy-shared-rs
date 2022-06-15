using System;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class SharedRsException : Exception
    {
        public SharedRsException()
        {

        }

        public SharedRsException(string message) : base(message)
        {

        }

        public SharedRsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
