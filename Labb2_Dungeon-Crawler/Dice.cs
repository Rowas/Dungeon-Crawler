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
        string sign = "";
        if (modifier < 0)
            sign = "";
        else if (modifier >= 0)
            sign = "+";

        return $"{dices}D{sides}{sign}{modifier}";
    }
}
