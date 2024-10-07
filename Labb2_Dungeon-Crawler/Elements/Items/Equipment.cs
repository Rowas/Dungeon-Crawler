using System.Numerics;
using System.Xml.Linq;

abstract class Equipment : LevelElements
{
    public string Name { get; set; }
    public int DamageDices { get; set; }
    public int dmgDiceSides { get; set; }
    public int dmgDiceModifier { get; set; }
    public int DefenseDice { get; set; }
    public int defDiceSides { get; set; }
    public int defDiceModifier { get; set; }
    public bool IsDead { get; set; }

    abstract public void Update(List<LevelElements> elements);

    public void Die(List<LevelElements> elements)
    {
        elements.Remove(this);
    }
}
