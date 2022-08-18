using System.Text.Json.Serialization;

namespace BlazorGG_Backend.Controllers.Dto
{
    public class PlayerBaseDto
    {
        [JsonPropertyName("id")] public string SummonerId { get; set; } = string.Empty;
        [JsonPropertyName("accountId")] public string AccountId { get; set; } = string.Empty;
        [JsonPropertyName("puuid")] public string PuuId { get; set; } = string.Empty;
        [JsonPropertyName("profileIconId")] public int ProfileIconId { get; set; }
        [JsonPropertyName("summonerLevel")] public int SummonerLevel { get; set; }
    }
}