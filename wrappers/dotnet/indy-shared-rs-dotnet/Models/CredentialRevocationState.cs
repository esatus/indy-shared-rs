using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRevocationState
    {
        public uint Handle { get; set; }
        public string Witness { get; set; }
        public RevocationRegistry RevocationRegistry { get; set; }
        public long Timestamp { get; set; }
    }
}
