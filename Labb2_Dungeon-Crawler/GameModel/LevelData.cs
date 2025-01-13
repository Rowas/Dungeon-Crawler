class LevelData
{
    private List<LevelElements> _elements = new List<LevelElements>();

    public List<LevelElements> Elements
    {
        get
        {
            return _elements;
        }
    }

    public void Load(string filename, string playerName)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                int y = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    for (int x = 0; x < line.Length; x++)
                    {
                        switch (line[x])
                        {
                            case '#':
                                Elements.Add(new Wall(x + 59, y + 2));
                                break;
                            case '@':
                                Elements.Add(new Player(x + 59, y + 2, playerName));
                                break;
                            case 'r':
                                Elements.Add(new Rat(x + 59, y + 2));
                                break;
                            case 's':
                                Elements.Add(new Snake(x + 59, y + 2));
                                break;
                            case 'B':
                                Elements.Add(new Boss(x + 59, y + 2));
                                break;
                            case 'G':
                                Elements.Add(new Guard(x + 59, y + 2));
                                break;
                            case 'W':
                                Elements.Add(new Sword(x + 59, y + 2));
                                break;
                            case 'A':
                                Elements.Add(new Armor(x + 59, y + 2));
                                break;
                            case 'F':
                                Elements.Add(new Food(x + 59, y + 2));
                                break;
                            case 'P':
                                Elements.Add(new Potion(x + 59, y + 2));
                                break;
                            case 'E':
                                Elements.Add(new Grue(x + 59, y + 2));
                                break;
                        }
                    }
                    y++;
                }
            }
        }
        catch (Exception ArgumentException)
        {
            Console.Clear();
            Console.WriteLine("Invalid Custom Map selected.");
            Console.WriteLine("Map does not exist.");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to exit.");
            Console.ReadKey();
        }
    }
}
