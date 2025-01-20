using Dungeon_Crawler.GeneralMethods;

namespace Dungeon_Crawler.DBModel
{
    internal class GameLoad
    {

        public static string SelectLoadGame()
        {
            string selectedGame = "";
            var possibleSelections = new Dictionary<string, string>();
            var possibleGames = new Dictionary<string, string>();
            using (var db = new SaveGameContext())
            {
                ClearConsole.ConsoleClear();
                TextCenter.CenterText("Please select the character to load:");
                TextCenter.CenterText("(Escape to go back (not implemented yet))");
                Console.WriteLine();
                List<string> savedGames = new List<string>();

                var gameList = db.SaveGames.OrderByDescending(sg => sg.SaveDate).Distinct();

                int i = 1;
                foreach (var game in gameList)
                {
                    TextCenter.CenterText($"{i}: {game.PlayerName} | {game.MapName} | {game.SaveDate} ");
                    possibleSelections.Add(i.ToString(), game.PlayerName);
                    possibleGames.Add(i.ToString(), game.SaveDate.ToString());
                    i++;
                }
            }
            var currentLine = Console.GetCursorPosition().Top;
            Console.SetCursorPosition(Console.WindowWidth / 2, currentLine);
            var PlayerID = Console.ReadLine();

            selectedGame = possibleSelections[PlayerID];


            return selectedGame;
        }
        public static void LoadGame(List<LevelElements> elements, string selectedGame)
        {
            GameLoad loading = new GameLoad();
            try
            {
                using (var db = new SaveGameContext())
                {
                    var saveGame = db.SaveGames.OrderByDescending(s => s.SaveDate).Where(id => id.PlayerName == selectedGame).FirstOrDefault();
                    var logMessages = db.CombatLogs.OrderByDescending(l => l.SaveDate).Where(id => id.PlayerName == selectedGame).FirstOrDefault();
                    if (saveGame != null)
                    {
                        LevelElements.SaveGameName = saveGame.Id.ToString();
                        LevelElements.CombatLogName = saveGame.CombatLogs.ToString();
                        Program.levelFile = saveGame.MapName.ToString();
                        loading.LoadGameState(saveGame.GameState, elements);
                        loading.LoadCombatLog(logMessages.SavedCombatLog);
                        Console.Clear();
                        TextCenter.CenterText("Save game loaded.");
                        Console.WriteLine();
                        TextCenter.CenterText("Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        GameLoop.turnCounter = saveGame.GameState.CurrentTurn;
                    }
                    else
                    {
                        Console.Clear();
                        TextCenter.CenterText("No save game found.");
                        Console.WriteLine();
                        TextCenter.CenterText("Press any key to exit.");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                TextCenter.CenterText("Unable to load save game.");
                TextCenter.CenterText("Database corrupted, save game does not exist or save game is from a different version.");
                Console.WriteLine();
                TextCenter.CenterText("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public void LoadGameState(GameState gameState, List<LevelElements> elements)
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

        public void LoadCombatLog(LogMessage logMessage)
        {
            for (int i = 0; i < logMessage.Key.Count; i++)
            {
                GameLoop.combatLog.Add(logMessage.Key[i], logMessage.Message[i]);
                GameLoop.logPosition++;
            }
        }
    }
}
