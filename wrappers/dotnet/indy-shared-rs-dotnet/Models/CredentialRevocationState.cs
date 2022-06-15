using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRevocationState
    {
        public uint Handle { get; set; }

        [JsonProperty("witness")]
        public Witness Witness { get; set; }

        [JsonProperty("rev_reg")]
        public RevocationRegistryValue CryptoRevocationRegistry { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
