using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class SubProof
    {
        [JsonProperty("primary_proof")]
        public PrimaryProof PrimaryProof { get; set; }

        [JsonProperty("non_revoc_proof")]
        public NonRevocProof NonRevocProof { get; set; } = null;
    }
}
