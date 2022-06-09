using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class SignatureCorrectnessProofValue
    {
        [JsonProperty("se")]
        public string se { get; set; }

        [JsonProperty("c")]
        public string c { get; set; }
    }
}
