using BlazorGG_Backend.Controllers.Dto;

namespace BlazorGG_Backend.Services;

public interface IPlayerService
{
    Task<PlayerBaseDto?> GetSummonerByAccountName(string accountName);
    Task<PlayerRankDto?> GetRankBySummonerId(string summonerId);
}