using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.models
{
    public class CredentialDefinition
    {
        public uint Handle { get; set; }
       
        public string CredentialDefinitionId { get; set; }

        public string SchemaId { get; set; }
        public string SignatureType { get; set; }
        public string tag { get; set; }
        public CredentialDefinitionData Value { get; set; }
    }
}
