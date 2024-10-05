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

    public void Load(string filename)
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
                            Elements.Add(new Wall(x + 2, y + 2));
                            break;
                        case '@':
                            Elements.Add(new Player(x + 2, y + 2));
                            break;
                        case 'r':
                            Elements.Add(new Rat(x + 2, y + 2));
                            break;
                        case 's':
                            Elements.Add(new Snake(x + 2, y + 2));
                            break;
                        case 'B':
                            Elements.Add(new Boss(x + 2, y + 2));
                            break;
                        case 'G':
                            Elements.Add(new Guard(x + 2, y + 2));
                            break;
                        case 'W':
                            Elements.Add(new Sword(x + 2, y + 2));
                            break;
                        case 'A':
                            Elements.Add(new Armor(x + 2, y + 2));
                            break;
                    }
                }
                y++;
            }
        }
    }
}
