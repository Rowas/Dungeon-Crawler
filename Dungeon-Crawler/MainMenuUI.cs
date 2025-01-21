using Dungeon_Crawler.GeneralMethods;

namespace Dungeon_Crawler
{
    internal class MainMenuUI
    {
        public void UIMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            TextCenter.CenterText("Main Menu");
            Console.ResetColor();
            Console.WriteLine();
            TextCenter.CenterText("Welcome, Stranger.");
            TextCenter.CenterText("You have entered a dark place.");
            TextCenter.CenterText("Let's hope you survive...");
            Console.WriteLine();
            TextCenter.CenterText("Pick an option: ");
            Console.WriteLine();
            TextCenter.CenterText("1. Start a new game.");
            TextCenter.CenterText("2. Load a saved game.");
            TextCenter.CenterText("3. View highscores.");
            Console.WriteLine();
            TextCenter.CenterText("0. Exit the game.");
            Console.WriteLine();
        }

        public void UIPickName()
        {
            Console.Clear();
            TextCenter.CenterText("Tell me, Stranger, what is your name? ");
            Console.WriteLine();
            Console.SetCursorPosition(Console.WindowWidth / 2, 3);
        }

        public void UINewGame(string playerName)
        {

            if (playerName == "Stranger")
            {
                ClearConsole.ConsoleClear();
                TextCenter.CenterText($"{playerName} ...");
                TextCenter.CenterText("Well, you need not tell me your name to continue.");
            }
            else
            {
                Console.Clear();
                TextCenter.CenterText("Ah, " + playerName + ", I greet you. ");
            }
            TextCenter.CenterText("I hope that your quest will be a fortuitous one.");
            TextCenter.CenterText("But... That is something that time will tell, is it not?");
            Console.WriteLine();
            TextCenter.CenterText("Now, let us begin.");
            Console.WriteLine();
            TextCenter.CenterText("Pick and option below: ");
            Console.WriteLine();
            TextCenter.CenterText("1. Load a pre-made map. (Default)");
            TextCenter.CenterText("2. Load a custom map.");
            Console.WriteLine();
            TextCenter.CenterText("0. Return to Main Menu");
            Console.WriteLine();
        }

        public void UIPreMadeMap()
        {
            Console.Clear();
            TextCenter.CenterText("It is time to indicate where, exactly, your adventure takes place..");
            TextCenter.CenterText("Select an option below, and we shall continue.");
            Console.WriteLine();
            TextCenter.CenterText("Select an option:");
            TextCenter.CenterText("1. Level 1 (Default).");
            TextCenter.CenterText("2. Level 1 (/w Boss & Items).");
            Console.WriteLine();
            TextCenter.CenterText("0. Return to Main Menu");
            Console.WriteLine();
        }

        public void UICustomMap()
        {
            Console.Clear();
            TextCenter.CenterText("Enter the name of your custom map.");
            TextCenter.CenterText("Map file have to be placed in .\\Levels\\");
            TextCenter.CenterText("as a .txt file and correctly formatted to work.");
            TextCenter.CenterText("Consult pre-made maps for requirements.");
            TextCenter.CenterText("For further information, type 'Help' or '?'.");
            Console.WriteLine();
        }

        public void UICustomMapHelp()
        {
            Console.CursorVisible = false;
            Console.Clear();
            TextCenter.CenterText("The following characters are required for a functioning map: ");
            TextCenter.CenterText("@ = Player tile");
            TextCenter.CenterText("# = Wall tile");
            Console.WriteLine();
            TextCenter.CenterText("The following characters are optional for the map: ");
            TextCenter.CenterText("r = Rats");
            TextCenter.CenterText("s = Snakes");
            TextCenter.CenterText("G = Guards");
            TextCenter.CenterText("F/P = Restorative items (25/50 HP)");
            TextCenter.CenterText("A = Magic Armor");
            TextCenter.CenterText("W = Magic Sword");
            TextCenter.CenterText("B = Boss monster");
            Console.WriteLine();
            TextCenter.CenterText("After having created a custom map, place it in .\\Levels\\ as a txt-file and reload the program.");
            Console.CursorVisible = true;
        }

        public void UILoadGame()
        {
            ClearConsole.ConsoleClear();
            TextCenter.CenterText("Please select the character to load:");
            TextCenter.CenterText("(Leave blank to go back)");
            Console.WriteLine();
        }
    }
}
