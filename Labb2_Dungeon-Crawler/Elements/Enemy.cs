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

    public void DistanceCheck(List<LevelElements> elements)
    {
        for (int i = -2; i < 5; i++)
        {
            for (int j = -2; j < 5; j++)
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
                            else if (element is Enemy)
                            {
                                IsVisible = false;
                            }
                        }
                    }
                }
            }
        }
    }

    public int Attack()
    {
        Dice enemyDamage = new Dice(DamageDices, dmgDiceSides, dmgDiceModifier);
        
        int eDmg = enemyDamage.Throw();

        return eDmg;
    }

    public int Defend()
    {
        Dice enemyDefend = new Dice(DamageDices, defDiceSides, defDiceModifier);

        int eDef = enemyDefend.Throw();

        return eDef;
    }
    public void Die(List<LevelElements> elements)
    {
        elements.Remove(this);
    }

    abstract public void Update(List<LevelElements> elements);
}
