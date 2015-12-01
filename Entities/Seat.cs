using Newtonsoft.Json;

namespace RussianRailwaysUtility.Entities {
    public enum SeatType {
        ANY,
        СВ,
        ЛЮКС,
        КУПЕ,
        ПЛАЦКАРТНЫЙ,
        СИДЯЧИЙ
    }

    public class Seat {
        [JsonProperty("freeSeats")] public int FreeSeatsCount { get; set; }
        [JsonProperty("typeLoc")] public SeatType Type { get; set; }
        [JsonProperty("tariff")] public decimal Price { get; set; }
    }
}
