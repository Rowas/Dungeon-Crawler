namespace Dungeon_Crawler.GeneralMethods
{
    internal class TextCenter
    {
        public static void CenterText(string text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
        }
    }
}
