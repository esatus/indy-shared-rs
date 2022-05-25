using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class AttributeInfo
    {
        public string Name { get; set; }
        public List<string> Names { get; set; }
        //TODO: Implement Query and AbstractQuery
        //public Query Restrictions { get; set; }
        public NonRevokedInterval NonRevoked { get; set; }
    }
}
