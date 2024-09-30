class Snake : Enemy
{
    public Snake(int x, int y)
    {
        Position = (x, y);
        objectTile = 'S';
        objectColor = ConsoleColor.Green;
        Name = "Snake";
        Health = 20;
        //DamageDice = 2D4;
        //DefenseDice = 2D6-1;
        IsDead = false;
    }

    public override void Update(List<LevelElements> elements)
    {
        //Move();
        //Attack();
        //Defend();
        //Die();
    }
}
