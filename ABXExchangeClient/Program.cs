using System.Text.Json;
using ABXExchangeClient.Models;
using ABXExchangeClient.Services;

namespace ABXExchangeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ExchangeService exchangeService = new ExchangeService();
                List<StockPacket> packets = exchangeService.FetchPackets();

                if (packets.Count > 0)
                {
                    string jsonOutput = JsonSerializer.Serialize(packets, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    string outputDirectory = "Output";
                    string outputPath = Path.Combine(outputDirectory, "stock_data.json");

                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    File.WriteAllText(outputPath, jsonOutput);

                    Console.WriteLine("Stock data has been saved to " + outputPath);
                }
                else
                {
                    Console.WriteLine("No packets were received from the server.");
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Directory not found: " + ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("I/O error occurred: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access to the path is denied: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
