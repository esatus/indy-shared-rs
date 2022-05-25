namespace indy_shared_rs_dotnet.models
{
    public class DidValue
    {
        private readonly string _did;
        private readonly string _method;

        public DidValue(string did, string method)
        {
            _did = did;
            _method = method;
        }

        public ShortDidValue ToShort()
        {
            return new ShortDidValue("");
        }

        public bool IsAbbrevation()
        {
            if (_method.StartsWith("sov"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ShortDidValue
    {
        private string _value;
        public ShortDidValue(string value)
        {
            _value = value;
        }
    }
}
