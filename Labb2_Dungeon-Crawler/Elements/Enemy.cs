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

    public (int, string, int, string) Combat()
    {
        (int, string) attack = Attack();
        (int, string) defense = Defend();

        return (attack.Item1, attack.Item2, defense.Item1, defense.Item2);
    }

    public (int, string) Attack()
    {
        Dice enemyDamage = new Dice(DamageDices, dmgDiceSides, dmgDiceModifier);

        int eDmg = enemyDamage.Throw();

        string eDmgDice = enemyDamage.ToString();

        return (eDmg, eDmgDice);
    }

    public (int, string) Defend()
    {
        Dice enemyDefend = new Dice(DefenseDice, defDiceSides, defDiceModifier);

        int eDef = enemyDefend.Throw();

        string eDefDice = enemyDefend.ToString();

        return (eDef, eDefDice);
    }
    public void Die(List<LevelElements> elements)
    {
        this.objectTile = ' ';
        this.Draw();
        elements.Remove(this);
        if (this is Boss)
        {
            Boss.YouWin();
        }

    }

    abstract public void Update(List<LevelElements> elements);
}
