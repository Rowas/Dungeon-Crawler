using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;

namespace Dungeon_Crawler.MainMenu
{
    internal class MainMenuLoops
    {
        ConsoleKeyInfo checkKey;
        MainMenuUI UI = new();
        public void MainMenu(bool sg)
        {
            Console.CursorVisible = false;
            int x = 1;

            do
            {
                string? menuSelect = string.Empty;

                UI.UIMainMenu(x, sg);

                Console.CursorVisible = false;

                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(16);
                }
                checkKey = Console.ReadKey(true);

                if (checkKey.Key == ConsoleKey.DownArrow) x++;
                if (x >= 4) x = 4;
                if (checkKey.Key == ConsoleKey.UpArrow) x--;
                if (x <= 1) x = 1;
                if (checkKey.Key == ConsoleKey.Enter) menuSelect = x.ToString();

                Console.CursorVisible = true;

                if (sg == false)
                {
                    if (x >= 2) x = 2;

                    switch (menuSelect)
                    {
                        case "1":
                            PickName(sg);
                            break;
                        case "2":
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    switch (menuSelect)
                    {
                        case "1":
                            PickName(sg);
                            break;
                        case "2":
                            LoadGameSelect();
                            break;
                        case "3":
                            ViewHighScore();
                            break;
                        case "4":
                            Environment.Exit(0);
                            break;
                    }
                }
            } while (checkKey.Key != ConsoleKey.Escape);
        }

        public void PickName(bool sg)
        {
            Console.CursorVisible = true;
            UI.UIPickName();
            Console.SetCursorPosition(Console.WindowWidth / 2, 2);
            string? playerName = Console.ReadLine();
            if (string.IsNullOrEmpty(playerName))
            {
                playerName = "Stranger";
            }
            Console.WriteLine();
            Console.CursorVisible = false;
            NewGame(playerName, sg);
        }

        public void NewGame(string? playerName, bool sg)
        {
            Console.CursorVisible = false;
            int x = 1;

            do
            {
                string? menuSelect = string.Empty;

                UI.UINewGame(playerName, x);

                Console.CursorVisible = false;

                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(16);
                }
                checkKey = Console.ReadKey(true);

                if (checkKey.Key == ConsoleKey.DownArrow) x++;
                if (x >= 3) x = 3;
                if (checkKey.Key == ConsoleKey.UpArrow) x--;
                if (x <= 1) x = 1;
                if (checkKey.Key == ConsoleKey.Enter) menuSelect = x.ToString();

                switch (menuSelect)
                {
                    case "1":
                        PreMadeMapSelect(playerName, sg);
                        return;
                    case "2":
                        CustomMapSelect(playerName, sg);
                        return;
                    case "3":
                        return;
                }
            } while (checkKey.Key != ConsoleKey.Escape);
        }

        public void PreMadeMapSelect(string? playerName, bool sg)
        {
            Console.CursorVisible = false;
            string levelFile;
            bool newGame = true;

            int x = 1;

            GameLoop start = new();

            do
            {
                string? menuSelect = string.Empty;

                UI.UIPreMadeMap(x);

                Console.CursorVisible = false;

                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(16);
                }
                checkKey = Console.ReadKey(true);

                if (checkKey.Key == ConsoleKey.DownArrow) x++;
                if (x >= 3) x = 3;
                if (checkKey.Key == ConsoleKey.UpArrow) x--;
                if (x <= 1) x = 1;
                if (checkKey.Key == ConsoleKey.Enter) menuSelect = x.ToString();

                switch (menuSelect)
                {
                    case "1":
                        levelFile = "Level1.txt";
                        start.StartUp(levelFile, playerName, newGame);
                        start.GameRunning(sg);
                        return;
                    case "2":
                        levelFile = "Level1_w_Boss.txt";
                        start.StartUp(levelFile, playerName, newGame);
                        start.GameRunning(sg);
                        return;
                    case "3":
                        return;
                }
            } while (checkKey.Key != ConsoleKey.Escape);
        }

        public void CustomMapSelect(string playerName, bool sg)
        {
            Console.CursorVisible = true;
            string? menuSelect = string.Empty;
            string levelFile;
            bool newGame = true;

            GameLoop start = new();


            while (menuSelect != "0")
            {
                UI.UICustomMap();
                Console.SetCursorPosition(Console.WindowWidth / 2, 6);
                menuSelect = Console.ReadLine();
                switch (menuSelect)
                {
                    case "help":
                        UI.UICustomMapHelp();
                        Console.ReadKey();
                        break;
                    case "Help":
                        UI.UICustomMapHelp();
                        Console.ReadKey();
                        break;
                    case "?":
                        UI.UICustomMapHelp();
                        Console.ReadKey();
                        break;
                    default:
                        levelFile = menuSelect;
                        start.StartUp(levelFile, playerName, newGame);
                        start.GameRunning(sg);
                        return;
                }
            }
        }

        public void LoadGameSelect()
        {
            bool sg = true;
            UI.UILoadGame();
            GameLoop start = new();

            string selectedGame = string.Empty;
            string playerName = string.Empty;
            bool newGame = false;
            var possibleSelections = new Dictionary<string, string>();
            var playerNames = new Dictionary<string, string>();
            int i = 0;

            using (var db = new SaveGameContext())
            {
                List<string> savedGames = [];

                var gameList = db.SaveGames.OrderByDescending(sg => sg.SaveDate).Distinct();

                foreach (var game in gameList)
                {
                    i++;
                    TextCenter.CenterText($"{i}: {game.PlayerName} | {game.MapName} | {game.SaveDate} ");
                    possibleSelections.Add(i.ToString(), game.Id.ToString());
                }
                if (possibleSelections.Count == 0)
                {
                    TextCenter.CenterText("No save games found.");
                    TextCenter.CenterText("Press enter/return to go back to the Main Menu.");
                }
                var currentLine = Console.GetCursorPosition().Top;
                Console.SetCursorPosition(Console.WindowWidth / 2, currentLine + 1);
                var PlayerID = Console.ReadLine();

                var idcheck = int.TryParse(PlayerID, out int x) == false ? 0 : x;

                Console.WriteLine();

                if (string.IsNullOrEmpty(PlayerID) || idcheck < 1 || idcheck > i)
                {
                    TextCenter.CenterText("No savegame selected.");
                    TextCenter.CenterText("Press any key to return to main menu.");
                    Console.CursorVisible = false;
                    Console.ReadKey();
                    return;
                }

                selectedGame = possibleSelections[PlayerID];
            }

            ClearConsole.ConsoleClear();
            TextCenter.CenterText($"Loading Game with Character: {playerName}");

            start.StartUp(selectedGame, playerName = string.Empty, newGame);
            start.GameRunning(sg);

        }

        public void ViewHighScore()
        {
            Console.CursorVisible = false;
            using (var db = new SaveGameContext())
            {
                var highscores = db.Highscores.OrderByDescending(s => s.Score).ToList();
                Console.Clear();
                TextCenter.CenterText("Highscores");
                Console.WriteLine();
                foreach (var highscore in highscores)
                {
                    TextCenter.CenterText("Player: " + highscore.PlayerName + " | Map: " + highscore.MapName + " | Score: " + highscore.Score + " | Date: " + highscore.SaveDate);
                }
            }
            Console.WriteLine();
            TextCenter.CenterText("Press any key to return...");
            Console.ReadKey();
            Console.CursorVisible = true;
        }
    }
}
