abstract class Items : LevelElements
{
    public string Name { get; set; }
    public int HealthRestore { get; set; }
    public bool IsDead { get; set; }

    abstract public void Update(List<LevelElements> elements);

    public void Die(List<LevelElements> elements)
    {
        this.objectTile = ' ';
        this.Draw();
        elements.Remove(this);
    }
}
