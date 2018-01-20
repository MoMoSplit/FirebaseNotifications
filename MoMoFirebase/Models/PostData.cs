using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoMoFirebase.Models
{
    public class PostData
    {
        [JsonProperty(PropertyName = "registration_ids")]
        public List<string> FcmTokens { get; set; }

        [JsonProperty(PropertyName = "data")]
        public MessageData MessageData { get; set; }
    }
}