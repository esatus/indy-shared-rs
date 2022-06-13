using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class RevocationRegistryEntry
    {
        public uint Handle { get; set; }
        public long DefEntryIdx { get; set; }
        public uint Entry { get; set; }
        public long Timestamp { get; set; }
    }
}
