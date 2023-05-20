using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT_Opti
{
    internal class Player
    {
        static readonly int[] clubRet = { 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
        static readonly int[] nationRet = { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
        static readonly int[] leagueRet = { 0, 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };

        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public int Club { get; set; }
        public int League { get; set; }
        public int Nation { get; set; }
        public string Position { get; set; }

        public int chem;
        public int clubCnt = 0;
        public int nationCnt = 0;
        public int leagueCnt = 0;

        public Player()
        {
            chem = 0;
        }

        internal void CalcChem(Dictionary<int, int> currClubs, Dictionary<int, int> currLeagues, Dictionary<int, int> currNations)
        {
            chem = Math.Min(clubRet[currClubs[Club]] + nationRet[currNations[Nation]] + leagueRet[currLeagues[League]], 3);
        }

        public int GetChem(int numClubs, int numLeagues, int numNations)
        {
            return Math.Min(clubRet[numClubs] + leagueRet[numLeagues] + nationRet[numNations], 3);
        }
    }
}