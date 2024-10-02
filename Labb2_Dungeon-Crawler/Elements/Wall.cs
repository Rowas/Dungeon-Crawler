using System.Runtime.InteropServices;

class Wall : LevelElements
{
    public Wall(int x, int y)
    {
        //isVisible = false;
        Position = (x, y);
        objectTile = '#';
        objectColor = ConsoleColor.Black;
    }
}
