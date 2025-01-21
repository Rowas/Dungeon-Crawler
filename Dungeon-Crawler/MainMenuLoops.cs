using Dungeon_Crawler.DBModel;
using Dungeon_Crawler.GeneralMethods;

namespace Dungeon_Crawler
{
    internal class MainMenuLoops
    {
        MainMenuUI UI = new();
        public void MainMenu()
        {
            string? menuSelect = string.Empty;

            while (menuSelect != "0")
            {
                UI.UIMainMenu();
                Console.SetCursorPosition(Console.WindowWidth / 2, 14);
                Console.CursorVisible = true;
                menuSelect = Console.ReadLine();

                switch (menuSelect)
                {
                    case "1":
                        PickName();
                        break;
                    case "2":
                        LoadGameSelect();
                        break;
                    case "3":
                        ViewHighScore();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public void PickName()
        {
            UI.UIPickName();
            Console.SetCursorPosition(Console.WindowWidth / 2, 2);
            string? playerName = Console.ReadLine();
            if (playerName == "")
            {
                playerName = "Stranger";
            }
            Console.WriteLine();
            NewGame(playerName);
        }

        public void NewGame(string? playerName)
        {
            string? menuSelect = string.Empty;

            while (menuSelect != "0")
            {
                UI.UINewGame(playerName);
                Console.SetCursorPosition(Console.WindowWidth / 2, 14);
                menuSelect = Console.ReadLine();

                switch (menuSelect)
                {
                    case "1":
                        PreMadeMapSelect(playerName);
                        return;
                    case "2":
                        CustomMapSelect(playerName);
                        return;
                    case "0":
                        return;
                }
            }
        }

        public void PreMadeMapSelect(string? playerName)
        {
            string levelFile;
            string? menuSelect = string.Empty;
            bool newGame = true;

            GameLoop start = new();


            while (menuSelect != "0")
            {
                UI.UIPreMadeMap();
                Console.SetCursorPosition(Console.WindowWidth / 2, 10);
                menuSelect = Console.ReadLine();

                switch (menuSelect)
                {
                    case "1":
                        levelFile = "Level1.txt";
                        start.StartUp(levelFile, playerName, newGame);
                        start.GameRunning();
                        return;
                    case "2":
                        levelFile = "Level1_w_Boss.txt";
                        start.StartUp(levelFile, playerName, newGame);
                        start.GameRunning();
                        return;
                    case "0":
                        break;
                }
            }
        }

        public void CustomMapSelect(string playerName)
        {
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
                    case "0":
                        break;
                    default:
                        levelFile = menuSelect;
                        start.StartUp(levelFile, playerName, newGame);
                        start.GameRunning();
                        return;
                }
            }
        }

        public void LoadGameSelect()
        {
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
                var currentLine = Console.GetCursorPosition().Top;
                Console.SetCursorPosition(Console.WindowWidth / 2, currentLine + 1);
                var PlayerID = Console.ReadLine();

                var idcheck = Int32.TryParse(PlayerID, out int x) == false ? 0 : x;

                Console.WriteLine();

                if (string.IsNullOrEmpty(PlayerID) || idcheck < 1 || idcheck > i)
                {
                    TextCenter.CenterText("No savegame selected.");
                    TextCenter.CenterText("Press any key to return to main menu.");
                    Console.ReadKey();
                    return;
                }

                selectedGame = possibleSelections[PlayerID];
            }

            ClearConsole.ConsoleClear();
            TextCenter.CenterText($"Loading Game with Character: {playerName}");

            start.StartUp(selectedGame, playerName = string.Empty, newGame);
            start.GameRunning();

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
