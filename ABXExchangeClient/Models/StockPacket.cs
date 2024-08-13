using System.Text.Json.Serialization;

namespace ABXExchangeClient.Models
{
    public class StockPacket
    {
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("buySellIndicator")]
        public char BuySellIndicator { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonPropertyName("packetSequence")]
        public int PacketSequence { get; set; }
    }
}
