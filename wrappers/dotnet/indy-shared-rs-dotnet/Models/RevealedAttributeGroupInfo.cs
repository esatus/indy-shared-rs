using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class RevealedAttributeGroupInfo
    {
        public uint SubProofIndex { get; set; }
        public Dictionary<string, AttributeValue> Values { get; set; }
    }
}
