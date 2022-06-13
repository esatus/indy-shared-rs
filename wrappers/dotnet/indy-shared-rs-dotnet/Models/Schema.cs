using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.Models
{
    public class Schema
    {
        public uint Handle { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Ver { get; set; }
        public HashSet<string> AttrNames { get; set; }
        public uint SeqNo { get; set; }
    }
}
