using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Dice
{
    int dices;
    int sides;
    int modifier;

    public Dice(int dices, int sides, int modifier)
    {
        this.dices = dices;
        this.sides = sides;
        this.modifier = modifier;
    }

    public int Throw()
    {
        Random rnd = new Random();
        int result = rnd.Next(dices + modifier, dices * sides + modifier);

        return result;
    }

    public override string ToString()
    {
        return $"{dices}D{sides}+{modifier}";
    }
}
