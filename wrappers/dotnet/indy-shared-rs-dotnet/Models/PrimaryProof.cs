using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class PrimaryProof
    {
        [JsonProperty("eq_proof")]
        public PrimaryEqualProof EqProof { get; set; }

        [JsonProperty("ne_proofs")] 
        public List<PrimaryPredicateInequalityProof> NeProofs { get; set; }
    }
}
