using Dungeon_Crawler.GeneralMethods;

namespace Dungeon_Crawler.DBModel
{
    internal class GameLoad
    {
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
                TextCenter.CenterText("Database corrupted, save game does not exist or save game is from an incompatible version.");
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
            for (int i = 0; i < logMessage.Key.Count; i++)
            {
                GameLoop.combatLog.Add(logMessage.Key[i], logMessage.Message[i]);
                GameLoop.logPosition++;
            }
        }
    }
}
