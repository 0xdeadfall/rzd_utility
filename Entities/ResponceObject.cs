using System.Collections.Generic;
using Newtonsoft.Json;

namespace RussianRailwaysUtility.Entities {
    public class ResponseObjectBase {
        [JsonProperty("result")] public string ResultCode { get; set; }
    }
    public class RidResponseObject : ResponseObjectBase {
        [JsonProperty("rid")] public string RidValue { get; set; }
    }
    public class ResponseObject : ResponseObjectBase {
        [JsonProperty("tp")] public IList<RequestInfo> RequestInfo { get; set; }
    }
}
