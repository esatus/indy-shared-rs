using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class Lemma
    {
        public List<sbyte> NodeHash { get; set; }
        public List<sbyte> SiblingHash { get; set; }
        public Lemma SubLemma { get; set; }
    }
}
