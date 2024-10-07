using System.Numerics;
using System.Xml.Linq;

abstract class Items : LevelElements
{
    public string Name { get; set; }
    public int HealthRestore {  get; set; }
    public bool IsDead { get; set; }

    abstract public void Update(List<LevelElements> elements);

    public void DistanceCheck(List<LevelElements> elements)
    {
        IsVisible = false;
        for (int i = -5; i < 6; i++)
        {
            for (int j = -5; j < 6; j++)
            {
                if (elements.Any(b => b.Position == (Position.Item1 + i, Position.Item2 + j)) == true)
                {
                    foreach (var element in elements)
                    {
                        if (element.Position == (Position.Item1 + i, Position.Item2 + j))
                        {
                            if (element is Player)
                            {
                                IsVisible = true;
                            }
                        }
                    }
                }
            }
        }
    }
    public void Die(List<LevelElements> elements)
    {
        elements.Remove(this);
    }
}
