using Dungeon_Crawler.GeneralMethods;

namespace Dungeon_Crawler.DBModel
{
    internal class GameLoad
    {

        public static string SelectLoadGame()
        {
            string selectedGame = string.Empty;
            var possibleSelections = new Dictionary<string, string>();
            var possibleGames = new Dictionary<string, string>();
            using (var db = new SaveGameContext())
            {
                List<string> savedGames = new();

                var gameList = db.SaveGames.OrderByDescending(sg => sg.SaveDate).Distinct();

                int i = 1;
                foreach (var game in gameList)
                {
                    TextCenter.CenterText($"{i}: {game.PlayerName} | {game.MapName} | {game.SaveDate} ");
                    possibleSelections.Add(i.ToString(), game.Id.ToString());
                    i++;
                }
            }
            var currentLine = Console.GetCursorPosition().Top;
            Console.SetCursorPosition(Console.WindowWidth / 2, currentLine);
            var PlayerID = Console.ReadLine();

            Console.WriteLine();

            if (PlayerID == "")
            {
                TextCenter.CenterText("No savegame selected.");
                TextCenter.CenterText("Press any key to return to main menu.");
                Console.ReadKey();
                return null;
            }

            selectedGame = possibleSelections[PlayerID];


            return selectedGame;
        }
        async public void LoadGame(List<LevelElements> elements, string selectedGame)
        {
            try
            {
                using (var db = new SaveGameContext())
                {
                    var loadId = new MongoDB.Bson.ObjectId($"{selectedGame}");
                    var saveGame = db.SaveGames.Find(loadId);

                    var logId = new MongoDB.Bson.ObjectId(saveGame.CombatLogs);
                    var logMessages = db.CombatLogs.Find(logId);


                    LevelElements.SaveGameName = saveGame.Id.ToString();
                    LevelElements.CombatLogName = saveGame.CombatLogs.ToString();
                    GameLoop.MapName = saveGame.MapName.ToString();
                    await LoadGameState(saveGame.GameState, elements);
                    await LoadCombatLog(logMessages.SavedCombatLog);
                    Console.Clear();
                    TextCenter.CenterText("Save game loaded.");
                    Console.WriteLine();
                    TextCenter.CenterText("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    GameLoop.TurnCounter = saveGame.GameState.CurrentTurn;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                TextCenter.CenterText("Unable to load save game.");
                TextCenter.CenterText("Database corrupted, save game does not exist or save game is from a different version.");
                Console.WriteLine();
                TextCenter.CenterText("Press any key to return to the Main Menu.");
                Console.ReadKey();
            }
        }

        async public Task LoadGameState(GameState gameState, List<LevelElements> elements)
        {

            elements.Add(gameState.Player);
            elements.AddRange(gameState.Walls);
            elements.AddRange(gameState.Bosses);
            elements.AddRange(gameState.Guards);
            elements.AddRange(gameState.Rats);
            elements.AddRange(gameState.Snakes);
            elements.AddRange(gameState.Armors);
            elements.AddRange(gameState.Swords);
            elements.AddRange(gameState.Foods);
            elements.AddRange(gameState.Potions);
            elements.AddRange(gameState.Grues);
        }

        async public Task LoadCombatLog(LogMessage logMessage)
        {
            GameLoop loop = new();
            for (int i = 0; i < logMessage.Key.Count; i++)
            {
                loop.combatLog.Add(logMessage.Key[i], logMessage.Message[i]);
                loop.LogPosition++;
            }
        }
    }
}
