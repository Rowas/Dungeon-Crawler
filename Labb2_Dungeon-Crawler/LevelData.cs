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
            int z = 0;
            while ((line = reader.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case '#':
                            Elements.Add(new Wall(x, y));
                            z++;
                            break;
                        case '@':
                            Elements.Add(new Player(x, y));
                            z++;
                            break;
                        case 'r':
                            Elements.Add(new Rat(x, y, z));
                            z++;
                            break;
                        case 's':
                            Elements.Add(new Snake(x, y));
                            z++;
                            break;
                    }
                }
                y++;
            }
        }
    }
}
