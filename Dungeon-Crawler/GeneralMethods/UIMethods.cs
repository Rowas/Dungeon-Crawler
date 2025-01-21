namespace Dungeon_Crawler.GeneralMethods
{
    internal class UIMethods
    {
        public static void PrintUI(Player player, int turnCounter, string mapName)
        {
            Dice defDice = new(player.DefDices, player.DefDiceSides, player.DefDiceModifier);
            Dice dmgDice = new(player.DmgDices, player.DmgDiceSides, player.DmgDiceModifier);
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.Write($"Player: {player.Name} | HP: {player.CurrentHealth} / {player.maxHealth}  Turn: {turnCounter}         ");
            Console.WriteLine($"Current Damage: {dmgDice} | Current Defense: {defDice}");
            Console.Write($"Items aquired: Magic Sword: {player.SwordAquired} | Magic Armor: {player.ArmorAquired}  ");
            Console.WriteLine($"Current Map: {mapName} | Current Score: {Player.CollectedPointMods * 100}".PadRight(50));

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 24);
            Console.WriteLine("Use arrow keys to move, space to wait, and escape to return to the Main Menu.");
            Console.WriteLine("Press \"L\" to open the combat log.");
            Console.WriteLine("Press \"S\" to save your game.");
        }

        public static void DrawCombatLog(Player? player, Dictionary<int, string> combatLog)
        {

            if (combatLog.Count > 0)
            {
                List<string> output = [];
                Console.ResetColor();
                Console.SetCursorPosition(0, 3);

                int n1 = combatLog.Count;
                int n2 = n1 - 9;

                for (int i = 4; i < 24; i++)
                {
                    Console.WriteLine(" ".PadRight(55));
                }

                Console.SetCursorPosition(0, 3);

                var spotCheck = combatLog.Values.Reverse().Take(1).ToList();
                var combatCheck = combatLog.Values.Reverse().Take(2).ToArray();

                if (combatCheck[0].Contains("Counter attack") || combatCheck[0].Contains("slain"))
                {
                    spotCheck = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                    if (combatCheck[0].Contains("slain"))
                    {
                        if (spotCheck[1].Contains("Rat"))
                        {
                            spotCheck = combatLog.Where(s => s.Key >= n2 - 2 && s.Key <= n1).Select(s => s.Value).ToList();
                        }
                        else
                        {
                            spotCheck.RemoveRange(0, 2);
                        }
                    }
                    if (spotCheck[1].Contains($"{player.Name}"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        for (int i = 0; i < 6; i++)
                        {
                            Console.WriteLine(spotCheck[i]);
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        for (int i = 0; i < 6; i++)
                        {
                            Console.WriteLine(spotCheck[i]);
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    n2 = n1 - 3;
                    output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                    spotCheck = combatLog.Values.Reverse().Take(1).ToList();

                    if (spotCheck[0].Contains("slain") && spotCheck[0].Contains("Rat") == false)
                    {
                        n2 = n1 - 1;
                        output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        foreach (var log in output)
                        {
                            Console.WriteLine(log);
                        }
                    }
                    else if (spotCheck[0].Contains("slain") && spotCheck[0].Contains("Rat"))
                    {
                        output = combatLog.Where(s => s.Key >= n2 - 2 && s.Key <= n1).Select(s => s.Value).ToList();
                        foreach (var log in output)
                        {
                            if (log.Contains("slain"))
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            }
                            Console.WriteLine(log);
                        }
                    }
                    else
                    {
                        foreach (var log in output)
                        {
                            Console.WriteLine(log);
                        }
                    }
                }

                if (spotCheck[0].Contains("Food ") || spotCheck[0].Contains("Potion "))
                {
                    n2 = n1 - 1;
                    output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    foreach (var log in output)
                    {
                        Console.WriteLine(log);
                    }
                }

                if (spotCheck[0].Contains("Attack ") || spotCheck[0].Contains("Defense "))
                {
                    n2 = n1 - 1;
                    output = combatLog.Where(s => s.Key >= n2 && s.Key <= n1).Select(s => s.Value).ToList();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    foreach (var log in output)
                    {
                        Console.WriteLine(log);
                    }
                }
            }
        }
    }
}
