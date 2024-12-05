using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public required string GameName { get; set; }
        public int Genre { get; set; }      // DROPDOWN -->  action=1, adventure=2, puzzle=3, RPG=4, strategy=5 etc.
        public int Platform { get; set; }   // DROPDOWN -->  pc=1, console=2, mobile=3 etc.
        public int YearOfReleased { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Story { get; set; }
        public string Objective { get; set; }
        public float Rating { get; set; }
        public string RequiredSpecs { get; set; }
        public bool GameActiveStatus { get; set; } = true;
    }
}