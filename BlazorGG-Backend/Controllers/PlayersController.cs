using BlazorGG_Backend.Controllers.Dto;
using BlazorGG_Backend.Data;
using BlazorGG_Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorGG_Backend.Services;

namespace BlazorGG_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPlayerService _playerService;
        public PlayersController(DataContext context, IPlayerService playerService)
        {
            _context = context;
            _playerService = playerService;
        }

        /// <summary>
        /// Obtiene un listado de todos los jugadores.
        /// </summary>
        /// <returns>Un objeto de tipo <see cref="List{Player}"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Player>>> GetPlayers()
        {
            var players = await _context.Players.OrderByDescending(p => p.Elo).ToListAsync();
            return Ok(players);
        }

        /// <summary>
        /// Obtiene un jugador a partir de su Id.
        /// </summary>
        /// <param name="id">Número entero que identifica al jugador.</param>
        /// <returns>Un objeto de tipo <see cref="Player"/>.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayerById([FromRoute] int id)
        {
            var player = await _context.Players.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (player == null)
                return NotFound($"No se encontró el jugador con Id {id}");
            return Ok(player);
        }

        /// <summary>
        /// Crea un jugador y lo agrega a la lista.
        /// </summary>
        /// <param name="playerDto">Objeto que contiene los datos del jugador</param>
        /// <returns>Un objeto de tipo <see cref="List{Player}"/>.</returns>
        [HttpPost]
        public async Task<ActionResult<List<Player>>> CreatePlayer([FromBody] CreatePlayerDto playerDto)
        {
            const string profileIconBaseUrl = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/profile-icons/";
            var playerBase = await _playerService.GetSummonerByAccountName(playerDto.AccountName);
            if (playerBase is null)
                return BadRequest("No se encontró ningún invocador con ese nombre.");

            var playerRank = await _playerService.GetRankBySummonerId(playerBase.SummonerId);
            if (playerRank is null)
                return UnprocessableEntity("El jugador no completó las placements");
            
            var player = CreatePlayer(playerDto, playerRank, playerBase, profileIconBaseUrl);
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok(player.Id);
        }

        private static Player CreatePlayer(CreatePlayerDto playerDto, PlayerRankDto playerRank, PlayerBaseDto playerBase, string profileIconBaseUrl)
        {
            return new Player
            {
                AccountName = playerDto.AccountName,
                UserName = playerDto.UserName,
                Elo = CalculateElo(playerRank),
                Rank = CalculateRank(playerRank),
                SummonerId = playerBase.SummonerId,
                Wins = playerRank.Wins,
                Losses = playerRank.Losses,
                LeaguePoints = playerRank.LeaguePoints,
                HotStreak = playerRank.HotStreak,
                TotalGames = playerRank.Wins + playerRank.Losses,
                Winrate = playerRank.Wins / (decimal)(playerRank.Wins + playerRank.Losses) * 100,
                ProfileIconUrl = $"{profileIconBaseUrl}{playerBase.ProfileIconId}.jpg"
            };
        }

        private static string CalculateRank(PlayerRankDto? soloQRank)
        {
            var rank = "Unranked";
            if (soloQRank != null)
                rank = $"{soloQRank.Tier} {soloQRank.Rank}";
            return rank;
        }

        private static int CalculateElo(PlayerRankDto? soloQRank)
        {
            var elo = 0;
            if (soloQRank == null) return elo;
            
            elo = soloQRank.Tier switch
            {
                "IRON" => 100,
                "BRONZE" => 200,
                "SILVER" => 300,
                "GOLD" => 400,
                "PLATINUM" => 500,
                "DIAMOND" => 600,
                "MASTER" => 700,
                "GRANDMASTER" => 700,
                "CHALLENGER" => 700,
                _ => 0,
            };
            elo += soloQRank.LeaguePoints;
            return elo;
        }

        /// <summary>
        /// Actualiza un jugador.
        /// </summary>
        /// <param name="updatedPlayer">Objeto que contiene los datos actualizados del jugador.</param>
        /// <param name="id">Número entero que identifica al jugador.</param>
        /// <returns>Un objeto de tipo <see cref="Player"/>.</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<List<Player>>> UpdatePlayer([FromBody] Player updatedPlayer, [FromRoute] int id)
        {
            var player = await _context.Players.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (player == null)
                return NotFound($"No se encontró el jugador con Id {id}");
            player = updatedPlayer;
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
            return Ok(player);
        }

        /// <summary>
        /// Borra un jugador.
        /// </summary>
        /// <param name="id">Número entero que identifica al jugador.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<List<Player>>> DeletePlayer([FromRoute] int id)
        {
            var player = await _context.Players.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (player == null)
                return NotFound($"No se encontró el jugador con Id {id}");
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return Ok($"Se elminó exitosamente el jugador con Id {id}");
        }

        [HttpPost("update-players")]
        public async Task UpdatePlayers()
        {
            var tasks = _context.Players.ToList().Select(p => UpdatePlayerRankAsync(p));
            await Task.WhenAll(tasks);
            await _context.SaveChangesAsync();
        }

        private async Task UpdatePlayerRankAsync(Player player)
        {
            var rank = await _playerService.GetRankBySummonerId(player.SummonerId);
            player = UpdatePlayerRank(player, rank);
            _context.Players.Update(player);
        }

        private static Player UpdatePlayerRank(Player player, PlayerRankDto? playerRankDto)
        {
            player.Elo = CalculateElo(playerRankDto);
            player.Rank = CalculateRank(playerRankDto);
            player.Wins = playerRankDto.Wins;
            player.Losses = playerRankDto.Losses;
            player.LeaguePoints = playerRankDto.LeaguePoints;
            player.HotStreak = playerRankDto.HotStreak;
            player.TotalGames = playerRankDto.Wins + playerRankDto.Losses;
            player.Winrate = playerRankDto.Wins / (decimal)(playerRankDto.Wins + playerRankDto.Losses) * 100;

            return player;
        }
    }
}
