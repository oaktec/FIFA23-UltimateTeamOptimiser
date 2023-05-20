using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using DBGrabber;

public static class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string apiKey = Environment.GetEnvironmentVariable("FUTDB_API_KEY");
    private const string apiEndpoint = "https://futdb.app/api/players?page=";
    private static readonly List<Player> allPlayers = new List<Player>();
    private static string RemoveAccents(string text)
    {
        StringBuilder sbReturn = new StringBuilder();
        var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }
        return sbReturn.ToString();
    }

    public static async Task Main(string[] args)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("API Key is missing. Please set the FUTDB_API_KEY environment variable.");
            return;
        }

        client.DefaultRequestHeaders.Add("X-AUTH-TOKEN", apiKey);

        var initialResponse = await FetchPlayers(1);
        if (initialResponse != null)
        {
            await ProcessPlayers(initialResponse.Items);

            for (int i = 2; i <= initialResponse.Pagination.PageTotal; i++)
            {
                Console.WriteLine($"Current Progress: {i}/{initialResponse.Pagination.PageTotal}");
                var players = await FetchPlayers(i);
                if (players != null)
                {
                    await ProcessPlayers(players.Items);
                }
            }

            await StorePlayers();
        }
    }

    private static async Task<ApiResponse?> FetchPlayers(int pageNumber)
    {
        try
        {
            var response = await client.GetAsync(apiEndpoint + pageNumber);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);
            return apiResponse;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch players at page {pageNumber}: {ex.Message}");
            return null;
        }
    }

    private static Task ProcessPlayers(Player[] players)
    {
        foreach (Player player in players)
        {
            player.Name = RemoveAccents(player.Name);
        }

        allPlayers.AddRange(players);
        return Task.CompletedTask;
    }

    private static async Task StorePlayers()
    {
        var json = JsonConvert.SerializeObject(allPlayers);
        await File.WriteAllTextAsync(@"..\..\..\..\players.json", json);
    }
}