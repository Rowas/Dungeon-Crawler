﻿using Dungeon_Crawler.GeneralMethods;

namespace Dungeon_Crawler.MainMenu
{
    internal class MainMenuUI
    {
        public void UIMainMenu(int x, bool sg)
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
            if (sg == false)
            {
                Console.WriteLine();
                if (x == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextCenter.CenterText("> Start a new game.");
                    Console.ResetColor();
                }
                else TextCenter.CenterText("Start a new game.");

                if (x == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextCenter.CenterText("> Exit the game.");
                    Console.ResetColor();
                }
                else TextCenter.CenterText("Exit the game.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                if (x == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextCenter.CenterText("> Start a new game.");
                    Console.ResetColor();
                }
                else TextCenter.CenterText("Start a new game.");

                if (x == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextCenter.CenterText("> Load a saved game.");
                    Console.ResetColor();
                }
                else TextCenter.CenterText("Load a saved game.");

                if (x == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextCenter.CenterText("> View highscores.");
                    Console.ResetColor();
                }
                else TextCenter.CenterText("View highscores.");
                Console.WriteLine();

                if (x == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    TextCenter.CenterText("> Exit the game.");
                    Console.ResetColor();
                }
                else TextCenter.CenterText("Exit the game.");
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            TextCenter.CenterText("Up arrow and Down arrow to move selection; Enter/Return to select");
        }

        public void UIPickName()
        {
            Console.Clear();
            TextCenter.CenterText("Tell me, Stranger, what is your name? ");
            Console.WriteLine();
            Console.SetCursorPosition(Console.WindowWidth / 2, 3);
        }

        public void UINewGame(string playerName, int x)
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
            if (x == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextCenter.CenterText("> Load a pre-made map.");
                Console.ResetColor();
            }
            else TextCenter.CenterText("Load a pre-made map.");

            if (x == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextCenter.CenterText("> Load a custom map.");
                Console.ResetColor();
            }
            else TextCenter.CenterText("Load a custom map.");

            Console.WriteLine();

            if (x == 3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextCenter.CenterText("> Return to Main Menu");
                Console.ResetColor();
            }
            else TextCenter.CenterText("Return to Main Menu");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            TextCenter.CenterText("Up arrow and Down arrow to move selection; Enter/Return to select");
        }

        public void UIPreMadeMap(int x)
        {
            Console.Clear();
            TextCenter.CenterText("It is time to indicate where, exactly, your adventure takes place..");
            TextCenter.CenterText("Select an option below, and we shall continue.");
            Console.WriteLine();
            TextCenter.CenterText("Select an option:");

            if (x == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextCenter.CenterText("> Level 1.");
                Console.ResetColor();
            }
            else TextCenter.CenterText("Level 1.");

            if (x == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextCenter.CenterText("> Level 1 (/w Boss & Items).");
                Console.ResetColor();
            }
            else TextCenter.CenterText("Level 1 (/w Boss & Items).");

            Console.WriteLine();

            if (x == 3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextCenter.CenterText("> Return to Main Menu");
                Console.ResetColor();
            }
            else TextCenter.CenterText("Return to Main Menu");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            TextCenter.CenterText("Up arrow and Down arrow to move selection; Enter/Return to select");
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
