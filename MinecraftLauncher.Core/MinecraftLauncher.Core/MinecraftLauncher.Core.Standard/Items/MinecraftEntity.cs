using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinecraftLauncher.Core.Standard.Items
{
    public class MinecraftEntity
    {
        [JsonProperty("arguments")]
        public JObject Arguments { get; private set; }

        [JsonProperty("assetIndex")]
        public JObject AssetIndex { get; private set; }

        [JsonProperty("assets")]
        public string Assets { get; private set; }

        [JsonProperty("downloads")]
        public JObject Downloads { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("inheritsFrom")]
        public string InheritsFrom { get; set; }

        [JsonProperty("libraries")]
        public JArray Libraries { get; private set; }

        [JsonProperty("logging")]
        public JObject Logging { get; private set; }

        [JsonProperty("mainClass")]
        public string MainClass { get; private set; }

        [JsonProperty("minecraftArguments")]
        public string MinecraftArguments { get; private set; }

        [JsonProperty("time")]
        public string Time { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }
    }
}
