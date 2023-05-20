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

        public int GetChem(int numClubs, int numLeagues, int numNations)
        {
            return Math.Min(clubRet[numClubs] + leagueRet[numLeagues] + nationRet[numNations], 3);
        }
    }
}