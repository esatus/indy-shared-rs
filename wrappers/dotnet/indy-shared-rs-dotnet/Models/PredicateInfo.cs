﻿using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class PredicateInfo
    {
        public string Name { get; set; }
        [JsonProperty("p_type")]
        public PredicateTypes PredicateType { get; set; }
        [JsonProperty("p_value")]
        public int PredicateValue { get; set; }
        //TODO: Implement Query and AbstractQuery
        //public Query Restrictions { get; set; }
        public NonRevokedInterval NonRevoked { get; set; }
    }
}