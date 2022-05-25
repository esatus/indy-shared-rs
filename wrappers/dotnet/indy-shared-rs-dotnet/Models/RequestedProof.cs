using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class RequestedProof
    {
        public Dictionary<string, RevealedAttributeInfo> RevealedAttrs { get; set; }
        public Dictionary<string, RevealedAttributeGroupInfo> RevealedAttrGroups { get; set; }
        public Dictionary<string, string> SelfAttestedAttrs { get; set; }
        public Dictionary<string, SubProofReferent> UnrevealedAttrs { get; set; }
        public Dictionary<string, SubProofReferent> Predicates { get; set; }
    }
}