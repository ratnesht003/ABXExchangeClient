using System.Net.Sockets;
using ABXExchangeClient.Models;
using ABXExchangeClient.Utilities;

namespace ABXExchangeClient.Services
{
    public class ExchangeService
    {
        private const string Hostname = "localhost";
        private const int Port = 3000;

        public List<StockPacket> FetchPackets()
        {
            List<StockPacket> packets = new List<StockPacket>();

            try
            {
                using (TcpClient client = new TcpClient(Hostname, Port))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] request = new byte[] { 1, 0 };
                    stream.Write(request, 0, request.Length);

                    Console.WriteLine("Request sent. Waiting for response...");
                    while (true)
                    {
                        byte[] buffer = new byte[17];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);

                        if (bytesRead > 0)
                        {
                            Console.WriteLine($"Received {bytesRead} bytes of data.");
                            StockPacket packet = PacketParser.Parse(buffer);
                            packets.Add(packet);
                        }
                        else
                        {
                            Console.WriteLine("No more data received from the server.");
                            break;
                        }
                    }

                    Console.WriteLine("Finished receiving data.");

                    int expectedSequence = 1;
                    foreach (var packet in packets)
                    {
                        while (expectedSequence < packet.PacketSequence)
                        {
                            byte[] resendRequest = new byte[] { 2, (byte)expectedSequence };
                            stream.Write(resendRequest, 0, resendRequest.Length);

                            Console.WriteLine($"Requested missing packet {expectedSequence}.");

                            byte[] buffer = new byte[17];
                            int bytesRead = stream.Read(buffer, 0, buffer.Length);

                            if (bytesRead == buffer.Length)
                            {
                                StockPacket missingPacket = PacketParser.Parse(buffer);
                                packets.Add(missingPacket);
                            }

                            expectedSequence++;
                        }

                        expectedSequence = packet.PacketSequence + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error communicating with the server: " + ex.Message);
            }

            packets.Sort((x, y) => x.PacketSequence.CompareTo(y.PacketSequence));

            return packets;
        }
    }
}
