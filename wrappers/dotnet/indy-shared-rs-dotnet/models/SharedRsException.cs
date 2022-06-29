using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public class SharedRsException : Exception
    {
        public SharedRsException(string message) : base(message)
        {

        }

        public static SharedRsException FromSdkError(string message)
        {
            string msg = JsonConvert.DeserializeObject<Dictionary<string, string>>(message)["message"];
            string errCode = JsonConvert.DeserializeObject<Dictionary<string, string>>(message)["code"];
            if (int.TryParse(errCode, out int errCodeInt))
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