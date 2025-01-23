using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;
using MongoDB.Bson.Serialization.Attributes;

class Player : LevelElements
{
    [BsonElement("name")]
    public string Name { get; set; } = "Adventurer";

    public int maxHealth = 100;
    public int CurrentHealth { get; set; } = 100;
    public static double CollectedPointMods { get; set; }
    public static double FinalScore { get; set; }
    public bool? SwordAquired { get; set; } = false;
    public bool? ArmorAquired { get; set; } = false;
    public int DmgDices { get; set; } = 2;
    public int DmgDiceSides { get; set; } = 6;
    public int DmgDiceModifier { get; set; } = 1;

    public int DefDices { get; set; } = 1;
    public int DefDiceSides { get; set; } = 8;
    public int DefDiceModifier { get; set; } = 0;

    public bool IsDead = false;

    public bool isVisible = true;

    public Player(int x, int y, string name)
    {
        Name = name;
        IsVisible = true;
        Position = (x, y);
        ObjectTile = '@';
        ObjectColor = ConsoleColor.Yellow;
        this.Draw();
    }
    public Player(int x, int y)
    {
        IsVisible = true;
        Position = (x, y);
        ObjectTile = '@';
        ObjectColor = ConsoleColor.Yellow;
        this.Draw();
    }

    public Player()
    {

    }

    public async Task Movement(ConsoleKeyInfo checkKey, List<LevelElements> elements, string levelFile)
    {
        switch (checkKey.Key)
        {
            case ConsoleKey.RightArrow:
                TakeStep(1, 'H', elements);
                GameLoop.TurnCounter++;
                break;
            case ConsoleKey.LeftArrow:
                TakeStep(-1, 'H', elements);
                GameLoop.TurnCounter++;
                break;
            case ConsoleKey.UpArrow:
                TakeStep(-1, 'V', elements);
                GameLoop.TurnCounter++;
                break;
            case ConsoleKey.DownArrow:
                TakeStep(1, 'V', elements);
                GameLoop.TurnCounter++;
                break;
            case ConsoleKey.Escape:
                Console.SetCursorPosition(0, 21);
                break;
            case ConsoleKey.S:
                SaveGame saving = new();
                Console.Clear();
                TextCenter.CenterText("Saving Game.");
                saving.SavingGame(elements, Name, GameLoop.TurnCounter, levelFile);
                Console.WriteLine();
                TextCenter.CenterText("Game saved");
                TextCenter.CenterText("Press any key to continue.");
                Console.ReadKey();
                Console.Clear();
                GameLoop.DrawGameState(elements);
                break;
            case ConsoleKey.L:
                GameLoop.GameLog(elements);
                break;
            default:
                GameLoop.TurnCounter++;
                Draw();
                break;
        }
        Console.ResetColor();
    }

    public void Exploration(List<LevelElements> elements)
    {
        Player player = new()
        {
            Position = this.Position
        };
        if (elements.Any(b => double.Hypot((b.Position.Item1 - player.Position.Item1), (b.Position.Item2 - player.Position.Item2)) <= 5) == true)
        {
            foreach (var element in from LevelElements element in elements
                                    where double.Hypot((element.Position.Item1 - player.Position.Item1), (element.Position.Item2 - player.Position.Item2)) <= 5
                                    select element)
            {
                switch (element)
                {
                    case Wall:
                        Wall wall = (Wall)element;
                        if (wall.IsVisible == false)
                        {
                            wall.IsVisible = true;
                            wall.DrawWall();
                        }
                        break;

                    case Enemy:
                        Enemy enemy = (Enemy)element;
                        if (enemy.IsVisible == false)
                        {
                            enemy.IsVisible = true;
                            enemy.Draw();
                        }
                        break;

                    case Equipment:
                        Equipment equipment = (Equipment)element;
                        if (equipment.IsVisible == false)
                        {
                            equipment.IsVisible = true;
                            equipment.Draw();
                        }
                        break;

                    case Items:
                        Items item = (Items)element;
                        if (item.IsVisible == false)
                        {
                            item.IsVisible = true;
                            item.Draw();
                        }
                        break;
                }
            }
        }
    }

    public void TakeStep(int d, char direction, List<LevelElements> elements)
    {
        if (direction == 'H')
        {

            d = TileCheck(d, direction, elements);
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Position = (Position.Item1 + d, Position.Item2);
            DrawPlayer();

        }
        else
        {
            d = TileCheck(d, direction, elements);
            Console.SetCursorPosition(Position.Item1, Position.Item2);
            Position = (Position.Item1, Position.Item2 + d);
            DrawPlayer();
        }
    }
    public int TileCheck(int d, char direction, List<LevelElements> elements)
    {
        CombatMethods combat = new();
        int x = 0;
        int y = 0;
        switch (direction)
        {
            case 'H':
                x = d; break;
            case 'V':
                y = d; break;
        }

        if (elements.Any(b => b.Position == (Position.Item1 + x, Position.Item2 + y)) == true)
        {
            foreach (var element in from element in elements.ToList()
                                    where element.Position == (Position.Item1 + x, Position.Item2 + y)
                                    select element)
            {
                switch (element)
                {
                    case Rat:
                        combat.Encounter(this, (Rat)element, 'P', elements);
                        break;
                    case Snake:
                        combat.Encounter(this, (Snake)element, 'P', elements);
                        break;
                    case Boss:
                        combat.Encounter(this, (Boss)element, 'P', elements);
                        break;
                    case Guard:
                        combat.Encounter(this, (Guard)element, 'P', elements);
                        break;
                    case Grue:
                        combat.Encounter(this, (Grue)element, 'P', elements);
                        break;
                    case Equipment:
                        combat.EquipmentPickup(this, (Equipment)element, elements);
                        return d;
                    case Items:
                        combat.ItemPickup(this, (Items)element, elements);
                        return d;
                }
            }

            return 0;
        }
        else
        {
            return d;
        }
    }

    public (int, string, int, string) Combat()
    {
        (int, string) attack = Attack();
        (int, string) defense = Defend();

        return (attack.Item1, attack.Item2, defense.Item1, defense.Item2);
    }

    public (int, string) Attack()
    {
        Dice playerDamage = new(DmgDices, DmgDiceSides, DmgDiceModifier);
        int pDmg = playerDamage.Throw();
        string pDmgDice = playerDamage.ToString();

        return (pDmg, pDmgDice);
    }

    public (int, string) Defend()
    {
        Dice playerDefense = new(DefDices, DefDiceSides, DefDiceModifier);
        int pDef = playerDefense.Throw();
        string pDefDice = playerDefense.ToString();

        return (pDef, pDefDice);
    }

    public void GameOver()
    {
        this.ObjectTile = ' ';
        Draw();
        Console.Clear();
        Console.SetCursorPosition(0, 12);
        TextCenter.CenterText("It's a sad thing that your adventures have ended here!!");
        TextCenter.CenterText(Name + " has died!!");
        TextCenter.CenterText("Your score was: " + Math.Round(ScoreCalc()));
        SaveScore();
        Console.ReadKey();
        GameLoop.IsPlayerDead = true;
    }

    public double ScoreCalc()
    {
        double score;
        double turnMod;
        double turnCounter = GameLoop.TurnCounter;
        turnMod = turnCounter / 250;
        score = (CollectedPointMods * 100) / turnMod;
        if (score <= 0) { score = 0; }
        FinalScore = score;
        return score;
    }

    public void SaveScore()
    {
        using (var db = new SaveGameContext())
        {
            var id = new MongoDB.Bson.ObjectId($"{LevelElements.SaveGameName}");
            var logId = new MongoDB.Bson.ObjectId($"{LevelElements.CombatLogName}");

            var deadSave = db.SaveGames.FirstOrDefault(s => s.Id == id);
            var deadLog = db.CombatLogs.FirstOrDefault(s => s.Id == logId);

            db.SaveGames.Remove(deadSave);
            db.CombatLogs.Remove(deadLog);

            db.Highscores.Add(new Highscore
            {
                PlayerName = Name,
                MapName = GameLoop.MapName,
                Score = ScoreCalc()
            });
            db.SaveChanges();
        }
    }
}
