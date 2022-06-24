using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        public static SharedRsException FromSdkError(string message)
        {
            string msg = JsonConvert.DeserializeObject<Dictionary<string, string>>(message)["message"];
            string errCode = JsonConvert.DeserializeObject<Dictionary<string, string>>(message)["code"];
            int errCodeInt;
            if (int.TryParse(errCode, out errCodeInt))
            {
                return new SharedRsException(
                    $"'{((ErrorCode)errCodeInt).ToErrorCodeString()}' error occured with ErrorCode '{errCode}' : {msg}.");
            }
            else
            {
                return new SharedRsException("An unknown error code was received.");
            }
        }
    }
}
