using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class RevocationRegistryDefinitionPrivateValue
    {
        [JsonProperty("gamma")]
        public string Gamma { get; set; }
    }
}
