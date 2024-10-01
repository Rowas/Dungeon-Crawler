abstract class Enemy : LevelElements
{
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int DamageDices { get; set; }
    public int dmgDiceSides { get; set; }
    public int dmgDiceModifier { get; set; }
    public int DefenseDice { get; set; }
    public int defDiceSides { get; set; }
    public int defDiceModifier { get; set; }
    public bool IsDead { get; set; }

    public int Attack(int DamageDices, int DiceSides, int DiceModifier)
    {
        Dice damage = new Dice(DamageDices, DiceSides, DiceModifier);
        return damage.Throw();
    }

    public void Defend()
    {
        // Defend
    }
    public void Die()
    {
        // Die
    }

    abstract public void Update(List<LevelElements> elements);

}
