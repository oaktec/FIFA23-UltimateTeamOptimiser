using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGrabber
{
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Nation { get; set; }

        public int League { get; set; }

        public int Club { get; set; }

        public int Rarity { get; set; }

        public string Position { get; set; }

        public int Rating { get; set; }
    }

    public class ApiResponse
    {
        public Pagination Pagination { get; set; }
        public Player[] Items { get; set; }
    }

    public class Pagination
    {
        public int PageTotal { get; set; }
    }
}
