using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class Proof
    {
        public List<sbyte> RootHash { get; set; }
        public Lemma Lemma { get; set; }
        public List<sbyte> Value { get; set; }
    }
}
