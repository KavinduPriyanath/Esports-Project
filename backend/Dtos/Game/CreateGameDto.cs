using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Game
{
    public class CreateGameDto
    {
        public required string GameName { get; set; }
        public int Genre { get; set; }      
        public int Platform { get; set; }   
        public int YearOfReleased { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Story { get; set; }
        public string Objective { get; set; }
        public float Rating { get; set; }
        public string RequiredSpecs { get; set; }
    }
}