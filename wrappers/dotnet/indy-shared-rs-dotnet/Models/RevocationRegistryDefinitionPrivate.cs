using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class RevocationRegistryDefinitionPrivate
    {
        public uint Handle { get; set; }

        [JsonProperty("value")]
        public RevocationRegistryDefinitionPrivateValue Value { get; set; }
    }
}
