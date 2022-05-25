using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.models
{
    public class MasterSecret
    {
        public SecretValue Value { get; set; }
        public uint Handle { get; set; } 
    }

    public class SecretValue
    {
        public string Ms { get; set; }
    }
}
