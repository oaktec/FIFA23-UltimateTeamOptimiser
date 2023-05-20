using GeneticSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FUT_Opti
{
    internal class Optimiser
    {
        private const int PlayerCount = 11;
        private const int FitnessMultiplier = 33;
        private const int PopulationMinSize = 10000;
        private const int PopulationMaxSize = 20000;
        private const float CrossoverProbability = 0.5f;
        private const float MutationProbability = 0.5f;
        private const int StagnationGenerations = 1000;

        private static readonly string[] PlayerPositions = new string[]
        {
            "GK", "LB", "RB", "CB", "CB", "CDM", "CDM", "CAM", "CAM", "ST", "ST"
        };

        private PlayerManager playerManager;
        private List<Player>[] playerListsByFormationIndex;

        public Optimiser()
        {
            PlayerDB.Init();
            playerManager = new PlayerManager();
        }

        private void SetupPlayersAndChoices()
        {
            playerListsByFormationIndex = new List<Player>[PlayerCount];
            for (int i = 0; i < PlayerCount; i++)
            {
                playerListsByFormationIndex[i] = playerManager.GetPlayersByPosition(PlayerPositions[i]);
                OptimisationChromosome.Choices[i] = playerListsByFormationIndex[i].Count;
            }
        }

        private GeneticAlgorithm SetupGeneticAlgorithm()
        {
            var chromosome = new OptimisationChromosome();
            var population = new Population(PopulationMinSize, PopulationMaxSize, chromosome);
            var fitness = CreateFitnessFunction();
            var selection = new WheelPlusNewSelection();
            var crossover = new UniformCrossover(CrossoverProbability);
            var mutation = new UniformMutation(true);
            var termination = new FitnessStagnationTermination(StagnationGenerations);
            var reinsertion = new EliteReinsertion();

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                MutationProbability = MutationProbability,
                Termination = termination,
                Reinsertion = reinsertion
            };

            return ga;
        }

        private FuncFitness CreateFitnessFunction()
        {
            return new FuncFitness((c) =>
            {
                var fc = c as OptimisationChromosome;
                if (fc == null)
                {
                    throw new InvalidCastException($"Failed to cast {nameof(c)} to {nameof(OptimisationChromosome)}");
                }

                var players = GetPlayersFromChromosome(fc);

                if (IsDuplicatePlayer(players))
                {
                    return 0;
                }

                return CalculateFitness(players);
            });
        }

        private List<Player> GetPlayersFromChromosome(OptimisationChromosome fc)
        {
            var players = new List<Player>();
            for (int i = 0; i < PlayerCount; i++)
            {
                players.Add(playerListsByFormationIndex[i][fc.GetSlot(i)]);
            }
            return players;
        }

        private bool IsDuplicatePlayer(List<Player> players)
        {
            return players.GroupBy(x => x.Id).Any(grp => grp.Count() > 1);
        }

        private int CalculateFitness(List<Player> players)
        {
            int fitness = 0;

            for (int i = 0; i < PlayerCount; i++)
            {
                fitness += players[i].Rating;
                fitness += players[i].GetChem(players.Count(x => x.Club == players[i].Club),
                    players.Count(x => x.League == players[i].League),
                    players.Count(x => x.Nation == players[i].Nation)) * FitnessMultiplier;
            }

            return fitness;
        }

        public void Run()
        {
            while (PresentMenu())
            {
                continue;
            }
        }

        private bool PresentMenu()
        {
            Console.WriteLine("Welcome to FUT-Opti!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Optimise a team");
            Console.WriteLine("2. Add Owned Players");
            Console.WriteLine("3. Exit");
            Console.Write("Selection: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    OptimiseTeam();
                    return false;
                case "2":
                    while (AddPlayers())
                        continue;
                    return true;
                case "3":
                    return false;
                default:
                    Console.WriteLine("Invalid selection!");
                    return true;
            }
        }

        private void OptimiseTeam()
        {
            SetupPlayersAndChoices();

            var ga = SetupGeneticAlgorithm();

            var latestFitness = 0.0;

            ga.GenerationRan += (sender, e) =>
            {
                var bestChromosome = ga.BestChromosome as OptimisationChromosome;
                var bestFitness = bestChromosome?.Fitness.Value;

                if (bestFitness != latestFitness)
                {
                    latestFitness = bestFitness.GetValueOrDefault();

                    DisplayBestPlayers(bestChromosome);

                    Console.WriteLine($"--------------------- Best fitness: {bestFitness}");
                    Console.WriteLine($"after {ga.GenerationsNumber} generations");
                }
            };

            ga.Start();

            Console.WriteLine("GA Finished.");
            Console.ReadLine();
        }

        private void DisplayBestPlayers(OptimisationChromosome bestChromosome)
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                var player = playerListsByFormationIndex[i][bestChromosome.GetSlot(i)];
                Console.WriteLine($"{PlayerPositions[i]} - {player.Name} {player.Rating}");
            }
        }

        private bool AddPlayers()
        {
            // Get name, rating and positions of player
            Console.WriteLine("Enter name, rating and positions of player separated by commas. Enter Q to exit");
            Console.WriteLine("Note - all accented letters in player names have been replaced by standard letters");
            Console.WriteLine("Example: Messi, 99, ST RW CF");
            string input = Console.ReadLine();

            if (input == "Q")
            {
                playerManager.SaveOwnedPlayers();
                return false;
            }

            string[] inputSplit = input.Split(',');

            if (inputSplit.Length != 3)
            {
                Console.WriteLine("Invalid input! Please provide name, rating, and positions of the player.");
                return true;
            }

            string name = inputSplit[0].Trim();

            if (!int.TryParse(inputSplit[1].Trim(), out int rating))
            {
                Console.WriteLine("Invalid rating! Please provide a valid number.");
                return true;
            }

            try
            {
                string[] positions = inputSplit[2].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                ValidatePositions(positions);

                playerManager.AddPlayer(name, rating, positions);
            }
            catch (Exception e)
            {
                Console.WriteLine($"!!! - Invalid Input - !!! {e}");
                return true;
            }

            return true;
        }

        private static void ValidatePositions(string[] positions)
        {
            foreach (string position in positions)
            {
                if (!PlayerPositions.Contains(position))
                {
                    throw new Exception($"Invalid position: {position}");
                }
            }
        }
    }
}
