abstract class Equipment : LevelElements
{
    public string Name { get; set; }
    public int DamageDices { get; set; }
    public int DmgDiceSides { get; set; }
    public int DmgDiceModifier { get; set; }
    public int DefenseDice { get; set; }
    public int DefDiceSides { get; set; }
    public int DefDiceModifier { get; set; }
    public bool IsDead { get; set; }

    abstract public void Update(List<LevelElements> elements);

    public void Die(List<LevelElements> elements)
    {
        this.ObjectTile = ' ';
        this.Draw();
        elements.Remove(this);
    }
}
