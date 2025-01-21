class Wall : LevelElements
{
    public Wall(int x, int y)
    {
        IsVisible = false;
        Position = (x, y);
        ObjectTile = '#';
        ObjectColor = ConsoleColor.Black;
        this.DrawWall();
    }

    public Wall()
    {

    }
}
