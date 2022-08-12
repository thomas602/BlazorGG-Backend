namespace BlazorGG_Backend.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int Elo { get; set; }
    }
}
