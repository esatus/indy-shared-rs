using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class PresentationProof
    {
        [JsonProperty("proofs")]
        public List<SubProof> proofs { get; set; }
        [JsonProperty("aggregated_proof")]
        public AggregatedProof AggregatedProof { get; set; }
    }
}
