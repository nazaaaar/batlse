using System;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.slime.mini.controller.commands;
using RMC.Mini;
using RMC.Mini.Controller;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.controller{
    public class MonstersController : IController
    {
        public bool IsInitialized{get; private set;}

        public IContext Context{get; private set;}

        private readonly MonstersList monstersList;

        public MonstersController(MonstersList monstersList)
        {
            this.monstersList = monstersList;
        }

        public void Dispose()
        {
    
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                Context.CommandManager.AddCommandListener<MonsterSpawnedCommand>(OnMonsterSpawned);
                Context.CommandManager.AddCommandListener<MonsterDeadCommand>(OnMonsterDead);
                
            }
        }

        private void OnMonsterDead(MonsterDeadCommand e)
        {
            if (e.monster.Team==Team.Red){
                monstersList.RedMonsters.Remove(e.monster);
                Debug.Log("Removed from red list");
            }
            else if (e.monster.Team==Team.Blue){
                monstersList.BlueMonsters.Remove(e.monster);
                Debug.Log("Removed from blue list");
            }
            Context.CommandManager.InvokeCommand(new MonsterDeathConfirmedCommand(){monster = e.monster});

        }

        private void OnMonsterSpawned(MonsterSpawnedCommand e)
        {
            switch (e.Monster.Team){
                case Team.Red: monstersList.RedMonsters.Add(e.Monster);break;
                case Team.Blue: monstersList.BlueMonsters.Add(e.Monster);break;
            }

            Debug.Log(e.Monster.Team.ToString()+"Added");
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){
                throw new System.Exception("MustBeInitialized");
            }
        }
    }
}