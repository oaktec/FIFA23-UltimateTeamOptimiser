using Newtonsoft.Json;

namespace FUT_Opti
{
    internal static class PlayerDB
    {
        private static List<Player> players;

        public static void Init()
        {
            var json = File.ReadAllText(@"..\..\..\..\players.json");
            players = JsonConvert.DeserializeObject<List<Player>>(json);
        }

        public static Player GrabPlayer(string name, int overall)
        {
            bool force = false;
            if (name.EndsWith("-f"))
            {
                force = true;
                name = name.Substring(0, name.Length - 2);
            }
            List<Player> results = players.FindAll(x => x.Name.Contains(name) && x.Rating == overall);
            if (results.Count > 1 && !force)
            {
                Console.WriteLine("*** Too many matches for '" + name + "' with rating of " + overall + " - Append name with -f to force first ***");
                Console.WriteLine("*** Please be more specific ***");
                return null;
            }
            if (results.Count == 0)
            {
                Console.WriteLine("*** No player found ***");
                return null;
            }
            return results[0];
        }

        public static Player GrabPlayer(int id)
        {
            return players.First(x => x.Id == id);
        }
    }
}
