namespace BlazorGG_Backend.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int Elo { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string SummonerId { get; set; } = string.Empty;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int LeaguePoints { get; set; } = 0;
        public bool HotStreak { get; set; } = false;
        public int TotalGames { get; set; } = 0;
        public decimal Winrate { get; set; } = 0;
        public string ProfileIconUrl { get; set; } = string.Empty;
    }
}
