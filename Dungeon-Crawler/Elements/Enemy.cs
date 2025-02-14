﻿using MongoDB.Bson.Serialization.Attributes;

abstract class Enemy : LevelElements
{
    public string Name { get; set; }

    [BsonIgnore]
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int DmgDice { get; set; }
    public int DmgDiceSides { get; set; }
    public int DmgDiceModifier { get; set; }
    public int DefDice { get; set; }
    public int DefDiceSides { get; set; }
    public int DefDiceModifier { get; set; }

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
        Dice enemyDamage = new(DmgDice, DmgDiceSides, DmgDiceModifier);

        int eDmg = enemyDamage.Throw();

        string eDmgDice = enemyDamage.ToString();

        return (eDmg, eDmgDice);
    }

    public (int, string) Defend()
    {
        Dice enemyDefend = new(DefDice, DefDiceSides, DefDiceModifier);

        int eDef = enemyDefend.Throw();

        string eDefDice = enemyDefend.ToString();

        return (eDef, eDefDice);
    }
    public void Die(List<LevelElements> elements)
    {
        this.ObjectTile = ' ';
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
