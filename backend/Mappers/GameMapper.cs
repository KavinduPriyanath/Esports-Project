using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Game;
using backend.Models;

namespace backend.Mappers
{
    public static class GameMapper
    {
        public static GameDto ToGameDto(this Game game)
        {
            return new GameDto
            {
                GameId = game.GameId,
                GameName = game.GameName,
                Genre = game.Genre,
                Platform = game.Platform,
                YearOfReleased = game.YearOfReleased,
                CompanyName = game.CompanyName,
                Description = game.Description,
                Story = game.Story,
                Objective = game.Objective,
                Rating = game.Rating,
                RequiredSpecs = game.RequiredSpecs
            };
        }

        public static Game ToGameFromGameCreateDto(this CreateGameDto createGameDto)
        {
            return new Game
            {
                GameName = createGameDto.GameName,
                Genre = createGameDto.Genre,
                Platform = createGameDto.Platform,
                YearOfReleased = createGameDto.YearOfReleased,
                CompanyName = createGameDto.CompanyName,
                Description = createGameDto.Description,
                Story = createGameDto.Story,
                Objective = createGameDto.Objective,
                Rating = createGameDto.Rating,
                RequiredSpecs = createGameDto.RequiredSpecs
            };
        }
    }
}