using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TemplateProductName.Domain.Services
{
    public class HashedPassword
    {
        [JsonIgnore]
        public string Hash { get; protected set; }
        [JsonIgnore]
        public string Salt { get; protected set; }

        public HashedPassword() { }
        public HashedPassword(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }
    }
}
