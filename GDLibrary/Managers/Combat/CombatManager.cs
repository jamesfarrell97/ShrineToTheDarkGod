﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class CombatManager : PausableGameComponent
    {
        #region Fields
        private Keys[] combatKeys;
        private List<CharacterObject> characters;
        private PlayerObject player;
        private List<Enemy> enemies;
        private bool playerTurn;
        private ManagerParameters managerParameters;
        private Random random = new Random();
        #endregion

        public CombatManager(Game game, EventDispatcher eventDispatcher, StatusType statusType, 
            ManagerParameters managerParameters, Keys[] combatKeys) : 
            base(game, eventDispatcher, statusType)
        {
            this.enemies = new List<Enemy>();
            this.managerParameters = managerParameters;
            this.playerTurn = true;
            this.combatKeys = combatKeys;
        }

        public void AddPlayer(PlayerObject player)
        {
            this.player = player;
        }


        public void PopulateEnemies(List<Enemy> enemies)
        {
            foreach(Enemy enemy in enemies)
            {
                this.enemies.Add(enemy);
            }
   
        }

        public bool Remove(Predicate<Enemy> predicate)
        {
            Enemy enemy= this.enemies.Find(predicate);
            if (enemy != null)
                return this.enemies.Remove(enemy);

            return false;
        }

        public int RemoveAll(Predicate<Enemy> predicate)
        {
            return this.enemies.RemoveAll(predicate);
        }

        public Enemy GetEnemy(string id)
        {
            if(enemies != null)
            {
                //finds enemy where ID is equal to the passed ID
                return this.enemies.Find(x=> x.ID == id);
            }
            return null;
        }

        public override void Update(GameTime gameTime)
        {
            /*
            Console.WriteLine("Enemy: " + this.enemies[0].ID + "\n"
                               + "Health: " + this.enemies[0].Health + "\n"
                               + "Attack: " + this.enemies[0].Attack + "\n"
                               + "Defence: " + this.enemies[0].Defence + "\n");
            */

            HandleKeyBoardInput(gameTime);
            base.Update(gameTime);
        }


        public void PrintStats(Enemy enemy)
        {
            Console.WriteLine("Player: \n" + "Health: " + this.player.Health + "\n"
                                + " Attack: " + this.player.Attack + "\n"
                                + "Defence: " + this.player.Defence);



            Console.WriteLine("Enemy: " + enemy.ID + "\n"
                                 + "Health: " + enemy.Health + "\n"
                                 + "Attack: " + enemy.Attack + "\n"
                                 + "Defence: " + enemy.Defence + "\n");
        }


        protected virtual void HandleKeyBoardInput(GameTime gameTime)
        {
            

            Enemy enemyOnFocus = GetEnemy("skeleton");


            if(playerTurn)
            {

                if (this.managerParameters.KeyboardManager.IsFirstKeyPress(this.combatKeys[0]))
                {
                    PrintStats(enemyOnFocus);

                    float playerAttack = this.player.Attack;
                    float enemyDefence = enemyOnFocus.Defence;

                    float damage = playerAttack - enemyDefence;

                    if (damage > 0)
                    {
                        enemyOnFocus.takeDamage(damage);
                    } 
                } else if (this.managerParameters.KeyboardManager.IsFirstKeyPress(this.combatKeys[1]))
                {
                    PrintStats(enemyOnFocus);

                    float playerDefence = this.player.Defence;
                    float enemyAttack = enemyOnFocus.Attack;

                    float damage = enemyAttack - playerDefence;

                    if (damage > 0)
                    {
                        this.player.takeDamage(damage);
                    }


                } else if (this.managerParameters.KeyboardManager.IsFirstKeyPress(this.combatKeys[2]))
                {
                    PrintStats(enemyOnFocus);

                    int dodge = random.Next(1, 7);

                    if (dodge % 2 == 0)
                    {
                        return;
                    } else
                    {
                        player.takeDamage(enemyOnFocus.Attack);
                    }
                }


            }
            else{
                PrintStats(enemyOnFocus);

                float enemyAttack = enemyOnFocus.Attack;

                this.player.takeDamage(enemyAttack);

            }

        }
    }
}