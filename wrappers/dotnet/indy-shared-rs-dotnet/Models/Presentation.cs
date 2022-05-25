using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class Presentation
    {
        public Proof Proof { get; set; }
        public RequestedProof RequestedProof { get; set; }
        public List<Identifier> Identifiers { get; set; }
    }
}
