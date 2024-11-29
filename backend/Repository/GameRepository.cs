using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Game;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;


namespace backend.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDBContext _context;
        public GameRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<Game> CreateAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> DeleteAsync(int id)
        {
            var gameModel = await _context.Games.FirstOrDefaultAsync(g => g.GameId == id);
            if (gameModel == null)
            {
                return null;
            }
            gameModel.GameActiveStatus = false;
            await _context.SaveChangesAsync();
            return gameModel;
        }

        public async Task<Game?> UpdateAsync(int id, UpdateGameDto gameDto)
        {
            var gameModel = await _context.Games.FirstOrDefaultAsync(g => g.GameId == id);
            if (gameModel == null)
            {
                return null;
            }
            gameModel.GameId = gameDto.GameId;
            gameModel.GameName = gameDto.GameName;
            gameModel.Genre = gameDto.Genre;
            gameModel.Platform = gameDto.Platform;
            gameModel.YearOfReleased = gameDto.YearOfReleased;
            gameModel.CompanyName = gameDto.CompanyName;
            gameModel.Description = gameDto.Description;
            gameModel.Story = gameDto.Story;
            gameModel.Objective = gameDto.Objective;
            gameModel.Rating = gameDto.Rating;
            gameModel.RequiredSpecs = gameDto.RequiredSpecs;
            gameModel.GameActiveStatus = gameDto.GameActiveStatus;
            await _context.SaveChangesAsync();
            return gameModel;
        }
    }
}