namespace indy_shared_rs_dotnet
{
    public static class ErrorCodeConverter
    {
        public static string ToErrorCodeString(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.Success => "Success",
                ErrorCode.Input => "Input",
                ErrorCode.IOError => "IOError",
                ErrorCode.InvalidState => "InvalidState",
                ErrorCode.Unexpected => "Unexpected",
                ErrorCode.CredentialRevoked => "CredentialRevoked",
                ErrorCode.InvalidUserRevocId => "InvalidUserRevocId",
                ErrorCode.ProofRejected => "ProofRejected",
                ErrorCode.RevocationRegistryFull => "RevocationRegistryFull",
                _ => "Unknown error code"
            };
        }
    }
    public enum ErrorCode
    {
        Success = 0,
        Input = 1,
        IOError = 2,
        InvalidState = 3,
        Unexpected = 4,
        CredentialRevoked = 5,
        InvalidUserRevocId = 6,
        ProofRejected = 7,
        RevocationRegistryFull = 8,
    }
}