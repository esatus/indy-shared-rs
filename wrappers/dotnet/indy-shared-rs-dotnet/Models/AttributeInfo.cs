﻿using indy_shared_rs_dotnet.models;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class AttributeInfo
    {
        public string Name { get; set; }

        public string Value { get; set; }
        public List<string> Names { get; set; }
        public Query Restrictions { get; set; }
        public NonRevokedInterval NonRevoked { get; set; }

        public AttributeInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
