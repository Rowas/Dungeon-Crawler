using MongoDB.Bson;

namespace Dungeon_Crawler.DBModel
{
    internal class SaveGame
    {
        public async Task SavingGame(List<LevelElements> elements, string name, int turn)
        {
            try
            {
                LevelData level = new LevelData();

                using (var db = new SaveGameContext())
                {
                    ObjectId id = new ObjectId();
                    ObjectId logId = new ObjectId();

                    if (LevelElements.SaveGameName != "0")
                    {
                        id = new MongoDB.Bson.ObjectId($"{LevelElements.SaveGameName}");
                        logId = new MongoDB.Bson.ObjectId($"{LevelElements.CombatLogName}");
                    }

                    var currentSave = db.SaveGames.FirstOrDefault(s => s.Id == id);
                    var currentLog = db.CombatLogs.FirstOrDefault(s => s.Id == logId);

                    var saveName = LevelElements.SaveGameName;


                    if (currentSave != null && saveName == currentSave.Id.ToString())
                    {
                        db.SaveGames.Remove(currentSave);
                        db.CombatLogs.Remove(currentLog);
                    }

                    var gameState = new GameState();

                    foreach (var element in elements)
                    {
                        element.xPos = element.Position.Item1;
                        element.yPos = element.Position.Item2;

                        switch (element)
                        {
                            case Wall wall:
                                gameState.Walls.Add(wall);
                                break;
                            case Boss boss:
                                gameState.Bosses.Add(boss);
                                break;
                            case Grue grue:
                                gameState.Grues.Add(grue);
                                break;
                            case Guard guard:
                                gameState.Guards.Add(guard);
                                break;
                            case Rat rat:
                                gameState.Rats.Add(rat);
                                break;
                            case Snake snake:
                                gameState.Snakes.Add(snake);
                                break;
                            case Armor armor:
                                gameState.Armors.Add(armor);
                                break;
                            case Sword sword:
                                gameState.Swords.Add(sword);
                                break;
                            case Food food:
                                gameState.Foods.Add(food);
                                break;
                            case Potion potion:
                                gameState.Potions.Add(potion);
                                break;
                            case Player player:
                                gameState.Player = player;
                                break;
                        }
                    }

                    var logMsg = new LogMessage();

                    foreach (var log in GameLoop.combatLog)
                    {
                        logMsg.Message.Add(log.Value);
                        logMsg.Key.Add(log.Key);
                    }

                    var savedLog = new CombatLog { Id = logId, PlayerName = name, MapName = Program.levelFile, SavedCombatLog = logMsg };

                    gameState.CurrentTurn = turn;

                    db.CombatLogs.Update(savedLog);
                    await db.SaveChangesAsync();

                    var saveGame = new GameSave { PlayerName = name, GameState = gameState, MapName = Program.levelFile, CombatLogs = savedLog.Id.ToString() };

                    db.SaveGames.Update(saveGame);
                    await db.SaveChangesAsync();

                    LevelElements.SaveGameName = db.SaveGames.OrderBy(t => t.SaveDate).FirstOrDefault(s => s.PlayerName == name).Id.ToString();
                    LevelElements.CombatLogName = db.CombatLogs.OrderBy(t => t.SaveDate).FirstOrDefault(s => s.PlayerName == name).Id.ToString();

                    return;
                }
            }

            catch (Exception ex)
            {
                return;
            }
        }
    }
}
