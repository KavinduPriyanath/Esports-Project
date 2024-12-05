using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Game;
using backend.Models;

namespace backend.Interfaces
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        Task<Game> CreateAsync(Game Game);
        Task<Game> UpdateAsync(int id, UpdateGameDto updateGameDto);
        Task<Game> DeleteAsync(int id);
    }
}