using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RussianRailwaysUtility.Entities {
    public class Train {
        [JsonProperty("number")] public string TripNumber { get; set; }
        [JsonProperty("date0")] public string DepDate { get; set; }
        [JsonProperty("time0")] public string DepTime { get; set; }
        [JsonProperty("date1")] public string ArrivalDate { get; set; }
        [JsonProperty("time1")] public string ArrivalTime { get; set; }
        [JsonProperty("station0")] public string StationFrom { get; set; }
        [JsonProperty("station1")] public string StationTo { get; set; }
        [JsonProperty("timeInWay")] public TimeSpan TimeInWay { get; set; }
        [JsonProperty("carrier")] public string Carrier { get; set; }
        [JsonProperty("cars")] public IList<Seat> FoundSeats { get; set; }
    }
}
