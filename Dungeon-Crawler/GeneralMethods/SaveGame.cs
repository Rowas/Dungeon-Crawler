using Dungeon_Crawler.GeneralMethods;
using MongoDB.Bson;

namespace Dungeon_Crawler.DBModel
{
    internal class SaveGame
    {
        public static void SavingGame(List<LevelElements> elements, string name, int turn)
        {
            LevelData level = new LevelData();
            Console.Clear();
            TextCenter.CenterText("Saving Game.");
            Console.WriteLine();

            using (var db = new SaveGameContext())
            {
                ObjectId id = new ObjectId();

                if (LevelElements.SaveGameName != "0")
                {
                    id = new MongoDB.Bson.ObjectId($"{LevelElements.SaveGameName}");
                }

                var currentSave = db.SaveGames.FirstOrDefault(s => s.Id == id);

                var saveName = LevelElements.SaveGameName;

                if (currentSave != null && saveName == currentSave.Id.ToString())
                {
                    db.SaveGames.Remove(currentSave);
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

                gameState.CurrentTurn = turn;

                var saveGame = new SaveGame { PlayerName = name, gameState = gameState, MapName = Program.levelFile };
                db.SaveGames.Add(saveGame);
                db.SaveChanges();

                LevelElements.SaveGameName = db.SaveGames.FirstOrDefault().Id.ToString();

                Console.WriteLine();
                TextCenter.CenterText("Game saved");
                TextCenter.CenterText("Press any key to continue.");
                Console.ReadKey();
                Console.Clear();
            }

            GameLoop.DrawGameState(elements);
        }
    }
}
