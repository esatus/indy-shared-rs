﻿using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class RevocationRegistryDelta
    {
        public uint Handle { get; set; }

        public string JsonString { get; set; }

        [JsonProperty("ver")]
        public string Ver { get; set; }

        [JsonProperty("value")]
        public RevocationRegistryDeltaValue Value { get; set; }
    }
}