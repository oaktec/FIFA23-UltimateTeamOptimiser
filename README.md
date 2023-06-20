# Fifa23 Ultimate Team Optimiser

FUT-Opti is a C# console application designed to optimise your player lineup for the FIFA 23 Ultimate Team mode, focusing on a 4-2-2-2 formation. It prioritises the higest possible team chemistry, followed by the highest team rating. You can specify the players you own, and the application will use a genetic algorithm to optimise the team composition.

## Getting Started

To run this solution, you must first obtain an API key from the [FUTDB](https://futdb.app/) website. This key must be stored in an environment variable named "FUTDB_API_KEY". 

You can then run the DBGrabber console application to fetch and process player data from the FUTDB API. This data will be stored in a local file "players.json".

Finally, you can run the FUTOpti console application to optimise your team. You will be prompted to enter the names of the players you own, and the application will output the optimised team composition.

## Project Structure

### FUTOpti

FUTOpti is the main application that uses the data from DBGrabber and runs the team optimisation logic. It includes several components:

- 'Optimiser.cs' - Controls the program's core functionality. Contains the logic for the interface, and manages process of optimising the team.
- 'GSExt.cs' - Contains custom classes that extend GeneticSharp library classes.
- 'Player.cs' - Represents a player in the game with various attributes and contains methods to calculate a player's chemistry.
- 'PlayerDB.cs' - An internal static class that stores a list of Player objects deserialised from the "players.json" file.
- 'PlayerManager.cs' - A class that stores which players are owned and manages the options for each position in the team.

### DBGrabber

DBGrabber is a console application that fetches, processes, and stores player data from the "FUTDB" API. The process involves:

- Retrieving the API key from an environment variable
- Using an HttpClient to request player data
- Processing this data, including removal of accents from player names
- Serialising the processed data into a JSON string
- Saving the processed data in a local file "players.json"

## Future Plans

No plans to continue development at this time.

However, if I were to continue development, I would likely transfer it to a web application. This would allow a much improved interface, users could create an account and save their owned players and teams. I would also like to add more formation options, and improve the optimisation algorithm to allow for more customisation (e.g. best team consisting of only silver players etc.).