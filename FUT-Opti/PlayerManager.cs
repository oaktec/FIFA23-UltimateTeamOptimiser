using Newtonsoft.Json;

namespace FUT_Opti
{
    internal class PlayerManager
    {
        private List<Player> GKs = new List<Player>();
        private List<Player> LBs = new List<Player>();
        private List<Player> RBs = new List<Player>();
        private List<Player> CBs = new List<Player>();
        private List<Player> CDMs = new List<Player>();
        private List<Player> CAMs = new List<Player>();
        private List<Player> STs = new List<Player>();

        public PlayerManager()
        {
            InitOwnedPlayers();
        }

        public void AddPlayer(string name, int rating, string[] positions)
        {
            var player = PlayerDB.GrabPlayer(name, rating);
            if (player == null)
            {
                Console.WriteLine("Player not found^^");
                return;
            }

            foreach (var position in positions)
            {
                switch (position)
                {
                    case "GK":
                        GKs.Add(player);
                        break;
                    case "LB":
                        LBs.Add(player);
                        break;
                    case "RB":
                        RBs.Add(player);
                        break;
                    case "CB":
                        CBs.Add(player);
                        break;
                    case "CDM":
                        CDMs.Add(player);
                        break;
                    case "CAM":
                        CAMs.Add(player);
                        break;
                    case "ST":
                        STs.Add(player);
                        break;
                    default:
                        Console.WriteLine("Invalid position^^");
                        return;
                }
            }

            if (name.EndsWith("-f"))
            {
                Console.WriteLine("Player " + player.Name + " forced!");
            }
            else
            {
                Console.WriteLine("Player " + player.Name + " added!");
            }
        }

        public void SaveOwnedPlayers()
        {
            string json = JsonConvert.SerializeObject(GKs);
            File.WriteAllText(@"..\..\..\..\GKs.json", json);

            json = JsonConvert.SerializeObject(LBs);
            File.WriteAllText(@"..\..\..\..\LBs.json", json);

            json = JsonConvert.SerializeObject(RBs);
            File.WriteAllText(@"..\..\..\..\RBs.json", json);

            json = JsonConvert.SerializeObject(CBs);
            File.WriteAllText(@"..\..\..\..\CBs.json", json);

            json = JsonConvert.SerializeObject(CDMs);
            File.WriteAllText(@"..\..\..\..\CDMs.json", json);

            json = JsonConvert.SerializeObject(CAMs);
            File.WriteAllText(@"..\..\..\..\CAMs.json", json);

            json = JsonConvert.SerializeObject(STs);
            File.WriteAllText(@"..\..\..\..\STs.json", json);
        }

        private void InitOwnedPlayers()
        {
            var json = File.Exists(@"..\..\..\..\GKs.json") ? File.ReadAllText(@"..\..\..\..\GKs.json") : JsonConvert.SerializeObject(new List<Player>());
            GKs = JsonConvert.DeserializeObject<List<Player>>(json);

            json = File.Exists(@"..\..\..\..\LBs.json") ? File.ReadAllText(@"..\..\..\..\LBs.json") : JsonConvert.SerializeObject(new List<Player>());
            LBs = JsonConvert.DeserializeObject<List<Player>>(json);

            json = File.Exists(@"..\..\..\..\RBs.json") ? File.ReadAllText(@"..\..\..\..\RBs.json") : JsonConvert.SerializeObject(new List<Player>());
            RBs = JsonConvert.DeserializeObject<List<Player>>(json);

            json = File.Exists(@"..\..\..\..\CBs.json") ? File.ReadAllText(@"..\..\..\..\CBs.json") : JsonConvert.SerializeObject(new List<Player>());
            CBs = JsonConvert.DeserializeObject<List<Player>>(json);

            json = File.Exists(@"..\..\..\..\CDMs.json") ? File.ReadAllText(@"..\..\..\..\CDMs.json") : JsonConvert.SerializeObject(new List<Player>());
            CDMs = JsonConvert.DeserializeObject<List<Player>>(json);

            json = File.Exists(@"..\..\..\..\CAMs.json") ? File.ReadAllText(@"..\..\..\..\CAMs.json") : JsonConvert.SerializeObject(new List<Player>());
            CAMs = JsonConvert.DeserializeObject<List<Player>>(json);

            json = File.Exists(@"..\..\..\..\STs.json") ? File.ReadAllText(@"..\..\..\..\STs.json") : JsonConvert.SerializeObject(new List<Player>());
            STs = JsonConvert.DeserializeObject<List<Player>>(json);
        }

        public List<Player> GetPlayersByPosition(string position)
        {
            switch (position)
            {
                case "GK":
                    return GKs;
                case "LB":
                    return LBs;
                case "RB":
                    return RBs;
                case "CB":
                    return CBs;
                case "CDM":
                    return CDMs;
                case "CAM":
                    return CAMs;
                case "ST":
                    return STs;
                default:
                    throw new ArgumentException("Invalid position");
            }
        }
    }
}
