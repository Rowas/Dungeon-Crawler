﻿namespace Dungeon_Crawler.GeneralMethods
{
    internal class CombatMethods
    {
        public static void Encounter(Player player, Enemy enemy, char F, List<LevelElements> elements)
        {

            var (firstActor, secondActor) = DetermineActors(player, enemy, F);

            var playerCombat = player.Combat();
            var enemyCombat = enemy.Combat();

            var playerDamage = CalculateDamage(playerCombat.Item1, enemyCombat.Item3);
            var enemyDamage = CalculateDamage(enemyCombat.Item1, playerCombat.Item3);

            ApplyDamage(player, enemy, playerDamage, enemyDamage);

            PrintCombatResult(playerCombat, enemyCombat, playerDamage, enemyDamage, firstActor, secondActor, player, enemy, elements);

        }

        private static (string firstActor, string secondActor) DetermineActors(Player player, Enemy enemy, char F)
        {
            return F == 'P' ? (player.Name, enemy.Name) : (enemy.Name, player.Name);
        }

        private static int CalculateDamage(int attack, int defense)
        {
            int damage = attack - defense;
            return damage < 1 ? 0 : damage;
        }

        private static void ApplyDamage(Player player, Enemy enemy, int playerDamage, int enemyDamage)
        {
            if (playerDamage > 0)
            {
                enemy.CurrentHealth -= playerDamage;
                if (enemy.CurrentHealth < 1)
                {
                    enemy.IsDead = true;
                }
            }

            if (enemyDamage > 0 && !enemy.IsDead)
            {
                player.currentHealth -= enemyDamage;
                GameLoop.NewHP = player.currentHealth;
                if (player.currentHealth < 1)
                {
                    player.IsDead = true;
                }
            }
        }

        public static void PrintCombatResult((int, string, int, string) playerCombat,
                                         (int, string, int, string) enemyCombat,
                                         int playerDamage,
                                         int enemyDamage,
                                         string firstActor,
                                         string secondActor,
                                         Player player,
                                         Enemy enemy,
                                         List<LevelElements> elements)
        {
            GameLoop.combatLog.Add(GameLoop.logPosition++, "".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"{secondActor} encountered.".PadRight(55));

            PrintAttackResult(firstActor, secondActor, playerCombat, enemyCombat, playerDamage, enemyDamage, player);

            if (firstActor == player.Name)
            {
                HandleAdventurerCombat(playerCombat, enemyCombat, playerDamage, enemyDamage, player, enemy, elements);
            }
            else
            {
                HandleEnemyCombat(playerCombat, enemyCombat, playerDamage, enemyDamage, player, enemy, elements);
            }

            GameLoop.DrawGameState(elements, GameLoop.combatLog);
        }

        private static void PrintAttackResult(string firstActor, string secondActor,
                                              (int, string, int, string) playerCombat,
                                              (int, string, int, string) enemyCombat,
                                              int playerDamage, int enemyDamage, Player player)
        {
            GameLoop.combatLog.Add(GameLoop.logPosition++, "".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"{firstActor} rolled {(firstActor == player.Name ? playerCombat.Item2 : enemyCombat.Item2)} to attack, result: {(firstActor == "Adventurer" ? playerCombat.Item1 : enemyCombat.Item1)}.".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"{secondActor} defended using {(firstActor == player.Name ? enemyCombat.Item4 : playerCombat.Item4)}, result: {(firstActor == "Adventurer" ? enemyCombat.Item3 : playerCombat.Item3)}.".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"Damage done by {firstActor} to {secondActor} is: {(firstActor == player.Name ? playerDamage : enemyDamage)}.".PadRight(55));
        }

        private static void HandleAdventurerCombat((int, string, int, string) playerCombat,
                                                   (int, string, int, string) enemyCombat,
                                                   int playerDamage, int enemyDamage,
                                                   Player player, Enemy enemy,
                                                   List<LevelElements> elements)
        {
            if (enemy.IsDead)
            {
                PrintEnemySlain(enemy.Name, elements, enemy, isCounter: false);
            }
            else
            {
                PrintCounterAttack(enemy.Name, player.Name, enemyCombat, playerCombat, enemyDamage, player, enemy, elements);
            }
        }

        private static void HandleEnemyCombat((int, string, int, string) playerCombat,
                                              (int, string, int, string) enemyCombat,
                                              int playerDamage, int enemyDamage,
                                              Player player, Enemy enemy,
                                              List<LevelElements> elements)
        {
            if (player.IsDead)
            {
                player.GameOver();
            }
            else
            {
                PrintCounterAttack(player.Name, enemy.Name, playerCombat, enemyCombat, playerDamage, player, enemy, elements);
            }
        }

        private static void PrintCounterAttack(string secondActor, string firstActor,
                                               (int, string, int, string) attackCombat,
                                               (int, string, int, string) defenseCombat,
                                               int damage, Player player, Enemy enemy,
                                               List<LevelElements> elements)
        {
            GameLoop.combatLog.Add(GameLoop.logPosition++, "".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"Counter attack by {secondActor}, {attackCombat.Item2} with result: {attackCombat.Item1}.".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"{firstActor} defended with {defenseCombat.Item4}, result: {defenseCombat.Item3}.".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"Counter attack by {secondActor} against {firstActor} did {damage}.".PadRight(55));

            if (player.IsDead)
            {
                player.GameOver();
            }
            if (enemy.IsDead)
            {
                Player.CollectedPointMods = Player.CollectedPointMods + enemy.PointModifier;
                PrintEnemySlain(enemy.Name, elements, enemy, isCounter: true);
            }
        }

        private static void PrintEnemySlain(string actor, List<LevelElements> elements, Enemy enemy, bool isCounter)
        {
            GameLoop.combatLog.Add(GameLoop.logPosition++, " ".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"{actor} has been slain.".PadRight(55));
            Console.ResetColor();
            Player.CollectedPointMods = Player.CollectedPointMods + enemy.PointModifier;
            enemy.Die(elements);
        }

        public static void EquipmentPickup(Player player, Equipment equipment, List<LevelElements> elements)
        {
            GameLoop.combatLog.Add(GameLoop.logPosition++, " ".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"The {equipment.Name} have been acquired.".PadRight(55));
            switch (equipment.Name)
            {
                case "Magic Sword":
                    GameLoop.combatLog.Add(GameLoop.logPosition++, "Attack have been increased to 2D10+2.".PadRight(55));
                    Player.CollectedPointMods = Player.CollectedPointMods + equipment.PointModifier;
                    UpdatingStats(player, equipment.DamageDices, equipment.DmgDiceSides, equipment.DmgDiceModifier, isAttack: true);
                    player.swordAquired = true;
                    break;
                case "Magic Armor":
                    GameLoop.combatLog.Add(GameLoop.logPosition++, "Defense have been increased to 2D8+2.".PadRight(55));
                    Player.CollectedPointMods = Player.CollectedPointMods + equipment.PointModifier;
                    UpdatingStats(player, equipment.DefenseDice, equipment.DefDiceSides, equipment.DefDiceModifier, isAttack: false);
                    player.armorAquired = true;
                    break;
            }
            equipment.IsDead = true;
            equipment.Die(elements);

            void UpdatingStats(Player player, int dices, int sides, int modifier, bool isAttack)
            {
                if (isAttack)
                {
                    player.dmgDices = dices;
                    player.dmgDiceSides = sides;
                    player.dmgDiceModifier = modifier;
                }
                else
                {
                    player.defDices = dices;
                    player.defDiceSides = sides;
                    player.defDiceModifier = modifier;
                }
            }
        }
        public static void ItemPickup(Player player, Items item, List<LevelElements> elements)
        {
            GameLoop.combatLog.Add(GameLoop.logPosition++, " ".PadRight(55));
            GameLoop.combatLog.Add(GameLoop.logPosition++, $"{item.Name} acquired, HP restored with {item.HealthRestore}.".PadRight(55));

            switch (item.Name)
            {
                case "Food":
                    {
                        player.currentHealth = player.currentHealth + item.HealthRestore;
                        Player.CollectedPointMods = Player.CollectedPointMods + item.PointModifier;
                        break;
                    }
                case "Potion":
                    {
                        player.currentHealth = player.currentHealth + item.HealthRestore;
                        Player.CollectedPointMods = Player.CollectedPointMods + item.PointModifier;
                        break;
                    }
            }

            GameLoop.NewHP = player.currentHealth > player.maxHealth ? player.maxHealth : player.currentHealth;

            item.IsDead = true;
            item.Die(elements);
        }
    }
}