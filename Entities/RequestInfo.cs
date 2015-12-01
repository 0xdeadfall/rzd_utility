using System.Collections.Generic;
using Newtonsoft.Json;

namespace RussianRailwaysUtility.Entities {
    public class RequestInfo {
        [JsonProperty("from")] public string From { get; set; }
        [JsonProperty("where")] public string Where { get; set; }
        [JsonProperty("date")] public string Date { get; set; }
        [JsonProperty("list")] public IList<Train> Trains { get; set; }
    }
}
