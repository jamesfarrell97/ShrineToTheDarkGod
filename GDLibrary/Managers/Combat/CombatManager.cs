﻿using System;
using System.Collections.Generic;
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
        private bool combat;
        private bool playerTurn;
        private ManagerParameters managerParameters;
        private Random random = new Random();
        #endregion

        #region Constructor

        public CombatManager(Game game, EventDispatcher eventDispatcher, StatusType statusType, 
            ManagerParameters managerParameters, Keys[] combatKeys) : 
            base(game, eventDispatcher, statusType)
        {
            this.enemies = new List<Enemy>();
            this.managerParameters = managerParameters;
            this.playerTurn = true;
            this.combat = false;
            this.combatKeys = combatKeys;
        }

        #endregion

        #region Event Handling
        protected override void RegisterForEventHandling(EventDispatcher eventDispatcher)
        {
            eventDispatcher.CombatEvent += EventDispatcher_CombatEvent;
            base.RegisterForEventHandling(eventDispatcher);
        }

        protected void EventDispatcher_CombatEvent(EventData eventData)
        {
            if (eventData.EventType == EventActionType.OnInitiateBattle)
            {
                //Combat started
                this.combat = true;
            }
            else if (eventData.EventType == EventActionType.OnPlayerAttack)
            {
                Console.WriteLine("Player Attack Event");
                //Object[] additionalParameters = {"playerAttack"};
                //EventDispatcher.Publish(new EventData(EventActionType.OnPlayerAttack, EventCategoryType.Sound2D, additionalParameters));
            }
            else if (eventData.EventType == EventActionType.OnPlayerDefend)
            {
                Console.WriteLine("Player Defend Event");
                //Object[] additionalParameters = {"playerDefend"};
                //EventDispatcher.Publish(new EventData(EventActionType.OnPlayerDefend, EventCategoryType.Sound2D, additionalParameters));
            }
            else if (eventData.EventType == EventActionType.OnEnemyAttack)
            {
                Console.WriteLine("Enemy Attack Event");
                //Object[] additionalParameters = {"enemyAttack"};
                //EventDispatcher.Publish(new EventData(EventActionType.OnEnemyAttack, EventCategoryType.Sound2D, additionalParameters));
            }
            else if (eventData.EventType == EventActionType.OnBattleEnd)
            {
                //Combat ended
                this.combat = false;
            }
            else if (eventData.EventType == EventActionType.OnPlayerDeath)
            {
                //Exit the game for now
                this.Game.Exit();
            }
        }
        #endregion

        #region Methods
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
                //Finds enemy where ID is equal to the passed ID
                return this.enemies.Find(x => x.ID == id);
            }
            return null;
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyBoardInput(gameTime);
            base.Update(gameTime);
        }


        public void PrintStats(Enemy enemy)
        {
            Console.WriteLine(
                "Player: \n" 
                + "Health: " + this.player.Health + "\n"
                + " Attack: " + this.player.Attack + "\n"
                + "Defence: " + this.player.Defence
            );

            Console.WriteLine(
                "Enemy: " + enemy.ID + "\n"
                + "Health: " + enemy.Health + "\n"
                + "Attack: " + enemy.Attack + "\n"
                + "Defence: " + enemy.Defence + "\n"
            );
        }

        protected virtual void HandleKeyBoardInput(GameTime gameTime)
        {
            //Grabbing a known Enemy for now 
            Enemy enemyOnFocus = GetEnemy("Skeleton");

            if (this.combat)
            { 
                if(playerTurn)
                {
                    if (this.managerParameters.KeyboardManager.IsFirstKeyPress(this.combatKeys[0]))
                    {
                        PrintStats(enemyOnFocus);
                        float playerAttack = this.player.Attack;
                        float enemyDefence = enemyOnFocus.Defence;
                        float damage = playerAttack - enemyDefence;

                        EventDispatcher.Publish(
                            new EventData(
                                EventActionType.OnPlayerAttack,
                                EventCategoryType.Combat)
                            );

                        if (damage > 0)
                        {
                            enemyOnFocus.TakeDamage(damage);
                        }

                        playerTurn = false;
                    }
                    else if (this.managerParameters.KeyboardManager.IsFirstKeyPress(this.combatKeys[1]))
                    {
                        PrintStats(enemyOnFocus);
                        float playerDefence = this.player.Defence;
                        float enemyAttack = enemyOnFocus.Attack;
                        float damage = enemyAttack - playerDefence;

                        EventDispatcher.Publish(
                            new EventData(
                                EventActionType.OnPlayerDefend,
                                EventCategoryType.Combat
                            )
                        );

                        if (damage > 0)
                        {
                            this.player.TakeDamage(damage);
                        }

                        playerTurn = false;
                    }
                    else if (this.managerParameters.KeyboardManager.IsFirstKeyPress(this.combatKeys[2]))
                    {
                        PrintStats(enemyOnFocus);
                        int dodge = random.Next(1, 7);

                        if (dodge % 2 == 0)
                        {
                            EventDispatcher.Publish(
                            new EventData(
                                EventActionType.OnPlayerDodge,
                                EventCategoryType.Combat)
                            );

                            return;
                        }
                        else
                        {
                            player.TakeDamage(enemyOnFocus.Attack);
                        }
                    }
                }
                else
                {
                    PrintStats(enemyOnFocus);
                    float enemyAttack = enemyOnFocus.Attack;
                    EventDispatcher.Publish(
                        new EventData(
                            EventActionType.OnEnemyAttack,
                            EventCategoryType.Combat
                        )
                    );

                    this.player.TakeDamage(enemyAttack);
                }
            }
        }
        #endregion
    }
}