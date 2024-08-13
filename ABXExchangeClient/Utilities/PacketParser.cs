using System.Text;
using ABXExchangeClient.Models;

namespace ABXExchangeClient.Utilities
{
    public static class PacketParser
    {
        public static StockPacket Parse(byte[] buffer)
        {
            if (buffer.Length != 17)
                throw new ArgumentException("Invalid packet size.");

            string symbol = Encoding.ASCII.GetString(buffer, 0, 4).Trim();
            char buySellIndicator = (char)buffer[4];
            int quantity = BitConverter.ToInt32(buffer, 5);
            int price = BitConverter.ToInt32(buffer, 9);
            int packetSequence = BitConverter.ToInt32(buffer, 13);

            if (BitConverter.IsLittleEndian)
            {
                quantity = ReverseBytes(quantity);
                price = ReverseBytes(price);
                packetSequence = ReverseBytes(packetSequence);
            }

            return new StockPacket
            {
                Symbol = symbol,
                BuySellIndicator = buySellIndicator,
                Quantity = quantity,
                Price = price,
                PacketSequence = packetSequence
            };
        }

        private static int ReverseBytes(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
