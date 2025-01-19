using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;
using MongoDB.Bson.Serialization.Attributes;

class Player : LevelElements
{
    [BsonElement("name")]
    public string Name { get; set; } = "Adventurer";

    public int maxHealth = 100;
    public int currentHealth { get; set; } = 100;
    public static double CollectedPointMods { get; set; }
    public static double FinalScore { get; set; }
    public bool? swordAquired { get; set; } = false;
    public bool? armorAquired { get; set; } = false;
    public int dmgDices { get; set; } = 2;
    public int dmgDiceSides { get; set; } = 6;
    public int dmgDiceModifier { get; set; } = 1;

    public int defDices { get; set; } = 1;
    public int defDiceSides { get; set; } = 8;
    public int defDiceModifier { get; set; } = 0;

    public bool IsDead = false;

    public bool isVisible = true;

    public Player(int x, int y, string name)
    {
        Name = name;
        IsVisible = true;
        Position = (x, y);
        objectTile = '@';
        objectColor = ConsoleColor.Yellow;
        this.Draw();
    }
    public Player(int x, int y)
    {
        IsVisible = true;
        Position = (x, y);
        objectTile = '@';
        objectColor = ConsoleColor.Yellow;
        this.Draw();
    }

    public Player()
    {

    }

    public void Movement(ConsoleKeyInfo checkKey, List<LevelElements> elements)
    {
        switch (checkKey.Key)
        {
            case ConsoleKey.RightArrow:
                TakeStep(1, 'H', elements);
                GameLoop.turnCounter++;
                break;
            case ConsoleKey.LeftArrow:
                TakeStep(-1, 'H', elements);
                GameLoop.turnCounter++;
                break;
            case ConsoleKey.UpArrow:
                TakeStep(-1, 'V', elements);
                GameLoop.turnCounter++;
                break;
            case ConsoleKey.DownArrow:
                TakeStep(1, 'V', elements);
                GameLoop.turnCounter++;
                break;
            case ConsoleKey.Escape:
                Console.SetCursorPosition(0, 21);
                Environment.Exit(0);
                break;
            case ConsoleKey.S:
                GameLoop.GameSave(elements, Name, GameLoop.turnCounter);
                break;
            case ConsoleKey.L:
                GameLoop.GameLog(elements);
                break;
            default:
                GameLoop.turnCounter++;
                Draw();
                break;
        }
        Console.ResetColor();
    }

    public void Exploration(List<LevelElements> elements)
    {
        Player player = new Player();
        player.Position = this.Position;
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
                        GameLoop.Encounter(this, (Rat)element, 'P', elements);
                        break;
                    case Snake:
                        GameLoop.Encounter(this, (Snake)element, 'P', elements);
                        break;
                    case Boss:
                        GameLoop.Encounter(this, (Boss)element, 'P', elements);
                        break;
                    case Guard:
                        GameLoop.Encounter(this, (Guard)element, 'P', elements);
                        break;
                    case Grue:
                        GameLoop.Encounter(this, (Grue)element, 'P', elements);
                        break;
                    case Equipment:
                        GameLoop.EquipmentPickup(this, (Equipment)element, elements);
                        return d;
                    case Items:
                        GameLoop.ItemPickup(this, (Items)element, elements);
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
        Dice playerDamage = new Dice(dmgDices, dmgDiceSides, dmgDiceModifier);
        int pDmg = playerDamage.Throw();
        string pDmgDice = playerDamage.ToString();

        return (pDmg, pDmgDice);
    }

    public (int, string) Defend()
    {
        Dice playerDefense = new Dice(defDices, defDiceSides, defDiceModifier);
        int pDef = playerDefense.Throw();
        string pDefDice = playerDefense.ToString();

        return (pDef, pDefDice);
    }

    public void GameOver()
    {
        this.objectTile = ' ';
        Draw();
        Console.Clear();
        Console.SetCursorPosition(0, 12);
        TextCenter.CenterText("It's a sad thing that your adventures have ended here!!");
        TextCenter.CenterText(Name + " has died!!");
        TextCenter.CenterText("Your score was: " + Math.Round(ScoreCalc()));
        SaveScore();
        Console.ReadKey();
        Environment.Exit(0);
    }

    public double ScoreCalc()
    {
        double score;
        double turnMod;
        double collectedPoints;
        double turns = GameLoop.turnCounter;
        turnMod = turns / 250;
        collectedPoints = CollectedPointMods * 100;
        score = collectedPoints / turnMod;
        if (score <= 0) { score = 0; }
        FinalScore = score;
        return score;
    }

    public void SaveScore()
    {
        using (var db = new SaveGameContext())
        {
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
