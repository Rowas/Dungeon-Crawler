﻿using MongoDB.Bson.Serialization.Attributes;

abstract class Enemy : LevelElements
{
    public string Name { get; set; }

    [BsonIgnore]
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int DmgDice { get; set; }
    public int dmgDiceSides { get; set; }
    public int dmgDiceModifier { get; set; }
    public int DefDice { get; set; }
    public int defDiceSides { get; set; }
    public int defDiceModifier { get; set; }

    [BsonIgnore]
    public bool IsDead { get; set; }

    public (int, string, int, string) Combat()
    {
        (int, string) attack = Attack();
        (int, string) defense = Defend();

        return (attack.Item1, attack.Item2, defense.Item1, defense.Item2);
    }

    public (int, string) Attack()
    {
        Dice enemyDamage = new Dice(DmgDice, dmgDiceSides, dmgDiceModifier);

        int eDmg = enemyDamage.Throw();

        string eDmgDice = enemyDamage.ToString();

        return (eDmg, eDmgDice);
    }

    public (int, string) Defend()
    {
        Dice enemyDefend = new Dice(DefDice, defDiceSides, defDiceModifier);

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
        if (this is Grue)
        {
            GrueSpawned = false;
        }

    }

    abstract public void Update(List<LevelElements> elements);
}
