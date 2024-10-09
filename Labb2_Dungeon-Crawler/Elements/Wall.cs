class Wall : LevelElements
{
    public Wall(int x, int y)
    {
        IsVisible = false;
        Position = (x, y);
        objectTile = '#';
        objectColor = ConsoleColor.Black;
        this.DrawWall();
    }
}
