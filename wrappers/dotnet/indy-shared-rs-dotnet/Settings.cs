using Newtonsoft.Json;

namespace indy_shared_rs_dotnet
{
    public class Settings
    {
        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }
}