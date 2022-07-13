using Newtonsoft.Json;

namespace indy_shared_rs_dotnet
{
    public class Settings
    {
        public static JsonSerializerSettings JsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }
}