class Wall : LevelElements
{
    public Wall(int x, int y)
    {
        Position = (x, y);
        objectTile = '#';
        objectColor = ConsoleColor.Black;
    }
}
