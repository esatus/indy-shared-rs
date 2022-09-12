using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class SharedRsException : Exception
    {
        public SharedRsException(string message) : base(message)
        {

        }

        public static SharedRsException FromSdkError(string message)
        {
            string msg = JsonConvert.DeserializeObject<Dictionary<string, string>>(message)["message"];
            string errorCode = JsonConvert.DeserializeObject<Dictionary<string, string>>(message)["code"];
            string extra = JsonConvert.DeserializeObject<Dictionary<string, string>>(message).ContainsKey("extra") ? JsonConvert.DeserializeObject<Dictionary<string, string>>(message)["extra"] : null;
            if (int.TryParse(errorCode, out int errCodeInt))
            {
                return new SharedRsException(
                    $"'{((ErrorCode)errCodeInt).ToErrorCodeString()}' error occured with ErrorCode '{errorCode}' and extra: '{extra}': {msg}.");
            }
            else
            {
                return new SharedRsException("An unknown error code was received.");
            }
        }
    }
}