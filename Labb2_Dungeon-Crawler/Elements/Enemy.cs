using Labb2_thirdTime;

abstract class Enemy : LevelElements
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int DamageDices { get; set; }
    public int DiceSides { get; set; }
    public int DiceModifier { get; set; }
    public int DefenseDices { get; set; }
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
