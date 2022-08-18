using System.Text.Json.Serialization;

namespace BlazorGG_Backend.Controllers.Dto
{
    public class PlayerRankDto
    {
        [JsonPropertyName("leagueId")] public string LeagueId { get; set; } = string.Empty;
        [JsonPropertyName("queueType")] public string QueueType { get; set; } = string.Empty;
        [JsonPropertyName("tier")] public string Tier { get; set; } = string.Empty;
        [JsonPropertyName("rank")] public string Rank { get; set; } = string.Empty;

        [JsonPropertyName("leaguePoints")] public int LeaguePoints { get; set; } = 0;
        [JsonPropertyName("wins")] public int Wins { get; set; } = 0;
        [JsonPropertyName("losses")] public int Losses { get; set; } = 0;
        [JsonPropertyName("hotStreak")] public bool HotStreak { get; set; } = false;
        [JsonPropertyName("freshBlood")] public bool FreshBlood { get; set; } = false;

        [JsonPropertyName("inactive")] public bool Inactive { get; set; } = true;
    }
}