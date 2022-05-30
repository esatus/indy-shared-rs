using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class RichSchema
    {
        public string Id { get; set; }
        public RSContent RsContent { get; set; }
        [JsonProperty("rs_name")]
        public string RsName { get; set; }
        [JsonProperty("rs_version")]
        public string RsVersion { get; set; }
        [JsonProperty("rs_type")]
        public string RsType { get; set; }
        public string Ver { get; set; }
    }
}
