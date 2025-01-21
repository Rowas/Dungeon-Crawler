namespace Dungeon_Crawler.GeneralMethods
{
    internal class CombatMethods
    {
        public void Encounter(Player player, Enemy enemy, char F, List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
        {

            var (firstActor, secondActor) = DetermineActors(player, enemy, F);

            var playerCombat = player.Combat();
            var enemyCombat = enemy.Combat();

            var playerDamage = CalculateDamage(playerCombat.Item1, enemyCombat.Item3);
            var enemyDamage = CalculateDamage(enemyCombat.Item1, playerCombat.Item3);

            ApplyDamage(player, enemy, playerDamage, enemyDamage);

            PrintCombatResult(playerCombat, enemyCombat, playerDamage, enemyDamage, firstActor, secondActor, player, enemy, elements, combatLog, logPosition);

        }

        public (string firstActor, string secondActor) DetermineActors(Player player, Enemy enemy, char F)
        {
            return F == 'P' ? (player.Name, enemy.Name) : (enemy.Name, player.Name);
        }

        public int CalculateDamage(int attack, int defense)
        {
            int damage = attack - defense;
            return damage < 1 ? 0 : damage;
        }

        public void ApplyDamage(Player player, Enemy enemy, int playerDamage, int enemyDamage)
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
                player.CurrentHealth -= enemyDamage;
                GameLoop.NewHP = player.CurrentHealth;
                if (player.CurrentHealth < 1)
                {
                    player.IsDead = true;
                }
            }
        }

        public void PrintCombatResult((int, string, int, string) playerCombat,
                                         (int, string, int, string) enemyCombat,
                                         int playerDamage,
                                         int enemyDamage,
                                         string firstActor,
                                         string secondActor,
                                         Player player,
                                         Enemy enemy,
                                         List<LevelElements> elements,
                                         Dictionary<int, string> combatLog,
                                         int logPosition)
        {
            combatLog.Add(logPosition++, "".PadRight(55));
            combatLog.Add(logPosition++, $"{secondActor} encountered.".PadRight(55));

            PrintAttackResult(firstActor, secondActor, playerCombat, enemyCombat, playerDamage, enemyDamage, player, combatLog, logPosition);

            if (firstActor == player.Name)
            {
                HandleAdventurerCombat(playerCombat, enemyCombat, enemyDamage, player, enemy, elements, combatLog, logPosition);
            }
            else
            {
                HandleEnemyCombat(playerCombat, enemyCombat, playerDamage, player, enemy, elements, combatLog, logPosition);
            }

            GameLoop.DrawGameState(elements, combatLog);
        }

        public void PrintAttackResult(string firstActor, string secondActor,
                                              (int, string, int, string) playerCombat,
                                              (int, string, int, string) enemyCombat,
                                              int playerDamage, int enemyDamage, Player player,
                                              Dictionary<int, string> combatLog,
                                              int logPosition)
        {

            combatLog.Add(logPosition++, "".PadRight(55));
            combatLog.Add(logPosition++, $"{firstActor} rolled {(firstActor == player.Name ? playerCombat.Item2 : enemyCombat.Item2)} to attack, result: {(firstActor == "Adventurer" ? playerCombat.Item1 : enemyCombat.Item1)}.".PadRight(55));
            combatLog.Add(logPosition++, $"{secondActor} defended using {(firstActor == player.Name ? enemyCombat.Item4 : playerCombat.Item4)}, result: {(firstActor == "Adventurer" ? enemyCombat.Item3 : playerCombat.Item3)}.".PadRight(55));
            combatLog.Add(logPosition++, $"Damage done by {firstActor} to {secondActor} is: {(firstActor == player.Name ? playerDamage : enemyDamage)}.".PadRight(55));
        }

        public void HandleAdventurerCombat((int, string, int, string) playerCombat,
                                                   (int, string, int, string) enemyCombat,
                                                   int enemyDamage,
                                                   Player player, Enemy enemy,
                                                   List<LevelElements> elements,
                                                   Dictionary<int, string> combatLog,
                                                   int logPosition)
        {
            if (enemy.IsDead)
            {
                PrintEnemySlain(enemy.Name, elements, enemy, combatLog, logPosition);
            }
            else
            {
                PrintCounterAttack(enemy.Name, player.Name, enemyCombat, playerCombat, enemyDamage, player, enemy, elements, combatLog, logPosition);
            }
        }

        public void HandleEnemyCombat((int, string, int, string) playerCombat,
                                              (int, string, int, string) enemyCombat,
                                              int playerDamage,
                                              Player player, Enemy enemy,
                                              List<LevelElements> elements, Dictionary<int, string> combatLog,
                                              int logPosition)
        {
            if (player.IsDead)
            {
                player.GameOver();
            }
            else
            {
                PrintCounterAttack(player.Name, enemy.Name, playerCombat, enemyCombat, playerDamage, player, enemy, elements, combatLog, logPosition);
            }
        }

        public void PrintCounterAttack(string secondActor, string firstActor,
                                               (int, string, int, string) attackCombat,
                                               (int, string, int, string) defenseCombat,
                                               int damage, Player player, Enemy enemy,
                                               List<LevelElements> elements,
                                               Dictionary<int, string> combatLog,
                                               int logPosition)
        {
            combatLog.Add(logPosition++, "".PadRight(55));
            combatLog.Add(logPosition++, $"Counter attack by {secondActor}, {attackCombat.Item2} with result: {attackCombat.Item1}.".PadRight(55));
            combatLog.Add(logPosition++, $"{firstActor} defended with {defenseCombat.Item4}, result: {defenseCombat.Item3}.".PadRight(55));
            combatLog.Add(logPosition++, $"Counter attack by {secondActor} against {firstActor} did {damage}.".PadRight(55));

            if (player.IsDead)
            {
                player.GameOver();
            }
            if (enemy.IsDead)
            {
                Player.CollectedPointMods += enemy.PointModifier;
                PrintEnemySlain(enemy.Name, elements, enemy, combatLog, logPosition);
            }
        }

        public void PrintEnemySlain(string actor, List<LevelElements> elements, Enemy enemy, Dictionary<int, string> combatLog, int logPosition)
        {
            combatLog.Add(logPosition++, " ".PadRight(55));
            combatLog.Add(logPosition++, $"{actor} has been slain.".PadRight(55));
            Console.ResetColor();
            Player.CollectedPointMods += enemy.PointModifier;
            enemy.Die(elements);
        }

        public void EquipmentPickup(Player player, Equipment equipment, List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
        {
            combatLog.Add(logPosition++, " ".PadRight(55));
            combatLog.Add(logPosition++, $"The {equipment.Name} have been acquired.".PadRight(55));
            switch (equipment.Name)
            {
                case "Magic Sword":
                    combatLog.Add(logPosition++, "Attack have been increased to 2D10+2.".PadRight(55));
                    Player.CollectedPointMods += equipment.PointModifier;
                    UpdatingStats(player, equipment.DamageDices, equipment.DmgDiceSides, equipment.DmgDiceModifier, isAttack: true);
                    player.SwordAquired = true;
                    break;
                case "Magic Armor":
                    combatLog.Add(logPosition++, "Defense have been increased to 2D8+2.".PadRight(55));
                    Player.CollectedPointMods += equipment.PointModifier;
                    UpdatingStats(player, equipment.DefenseDice, equipment.DefDiceSides, equipment.DefDiceModifier, isAttack: false);
                    player.ArmorAquired = true;
                    break;
            }
            equipment.IsDead = true;
            equipment.Die(elements);

            static void UpdatingStats(Player player, int dices, int sides, int modifier, bool isAttack)
            {
                if (isAttack)
                {
                    player.DmgDices = dices;
                    player.DmgDiceSides = sides;
                    player.DmgDiceModifier = modifier;
                }
                else
                {
                    player.DefDices = dices;
                    player.DefDiceSides = sides;
                    player.DefDiceModifier = modifier;
                }
            }
        }
        public void ItemPickup(Player player, Items item, List<LevelElements> elements, Dictionary<int, string> combatLog, int logPosition)
        {
            combatLog.Add(logPosition++, " ".PadRight(55));
            combatLog.Add(logPosition++, $"{item.Name} acquired, HP restored with {item.HealthRestore}.".PadRight(55));

            switch (item.Name)
            {
                case "Food":
                    {
                        player.CurrentHealth += item.HealthRestore;
                        Player.CollectedPointMods += item.PointModifier;
                        break;
                    }
                case "Potion":
                    {
                        player.CurrentHealth += item.HealthRestore;
                        Player.CollectedPointMods += item.PointModifier;
                        break;
                    }
            }

            GameLoop.NewHP = player.CurrentHealth > player.maxHealth ? player.maxHealth : player.CurrentHealth;

            item.IsDead = true;
            item.Die(elements);
        }
    }
}