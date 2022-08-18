using System.Text.Json;
using BlazorGG_Backend.Controllers.Dto;

namespace BlazorGG_Backend.Services;

public class PlayerService : IPlayerService
{
    private const string RiotBaseUrl = "https://la2.api.riotgames.com/lol";
    private const string RiotApiKey = "RGAPI-57b9cc6b-b264-4945-8c66-de89e0148f6b";
    private static readonly HttpClient Client = new();

    public async Task<PlayerBaseDto?> GetSummonerByAccountName(string accountName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{RiotBaseUrl}/summoner/v4/summoners/by-name/{accountName}?api_key={RiotApiKey}");
        var response = await Client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseStream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<PlayerBaseDto>(responseStream);
    }

    public async Task<PlayerRankDto?> GetRankBySummonerId(string summonerId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{RiotBaseUrl}/league/v4/entries/by-summoner/{summonerId}?api_key={RiotApiKey}");
        var response = await Client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var responseStream = await response.Content.ReadAsStreamAsync();
        var playerRanks = await JsonSerializer.DeserializeAsync<List<PlayerRankDto>>(responseStream);
        return playerRanks?.Find(p => p.QueueType == "RANKED_SOLO_5x5");
    }
}