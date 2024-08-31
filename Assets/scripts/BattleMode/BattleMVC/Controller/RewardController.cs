using nazaaaar.platformBattle.MainMenu.viewAbstract;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.service;
using nazaaaar.platformBattle.mini.viewAbstract;
using nazaaaar.slime.mini.controller.commands;
using RMC.Mini;
using RMC.Mini.Controller;
using System;
using System.Linq;
using UnityEngine;
namespace nazaaaar.platformBattle.mini.controller
{
    public class RewardController : IController
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}
        

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                Context.CommandManager.AddCommandListener<MonsterVictoryCommand>(OnMonsterVictory);
                Context.CommandManager.AddCommandListener<MonsterDeadCommand>(OnMonsterDeath);
            }
        }

        private void OnMonsterDeath(MonsterDeadCommand e)
        {
            Context.CommandManager.InvokeCommand(new MoneyAddRequestCommand(){
                amount = 1,
                team = e.monster.Team == Team.Red ? Team.Red : Team.Blue
            });
        }

        private void OnMonsterVictory(MonsterVictoryCommand e)
        {
            Context.CommandManager.InvokeCommand(new MoneyAddRequestCommand(){
                amount = 8,
                team = e.monster.Team
            });
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized){throw new Exception("MustBeInitialized");}
        }
    }
}
