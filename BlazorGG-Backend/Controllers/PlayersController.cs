using BlazorGG_Backend.Controllers.Dto;
using BlazorGG_Backend.Data;
using BlazorGG_Backend.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorGG_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly DataContext _context;

        public PlayersController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene un listado de todos los jugadores.
        /// </summary>
        /// <returns>Un objeto de tipo <see cref="List{Player}"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Player>>> GetPlayers()
        {
            var players = await _context.Players.ToListAsync();
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
            Player? player = await _context.Players.Where(p => p.Id == id).FirstOrDefaultAsync();
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
            Player player = new()
            {
                AccountName = playerDto.AccountName,
                UserName = playerDto.UserName,
                Elo = playerDto.Elo,
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok(player.Id);
        }

        /// <summary>
        /// Actualiza un jugador.
        /// </summary>
        /// <param name="updatedPlayer">Objeto que contiene los datos actualizados del jugador.</param>
        /// <param name="id">Número entero que identifica al jugador.</param>
        /// <returns>Un objeto de tipo <see cref="Player"/>.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Player>>> UpdatePlayer([FromBody] Player updatedPlayer, [FromRoute] int id)
        {
            Player? player = await _context.Players.Where(p => p.Id == id).FirstOrDefaultAsync();
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Player>>> DeletePlayer([FromRoute] int id)
        {
            Player? player = await _context.Players.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (player == null)
                return NotFound($"No se encontró el jugador con Id {id}");
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return Ok($"Se elminó exitosamente el jugador con Id {id}");
        }
    }
}
