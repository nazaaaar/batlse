using System;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using nazaaaar.slime.mini;
using nazaaaar.slime.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class MonsterSpawner : MonoBehaviour, IMonsterSpawner
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public event Action<IMonster> OnMonsterSpawned;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                Context.CommandManager.AddCommandListener<MonsterSpawnRequestCommand>(OnMonsterSpawnRequestCommand);
            }
        }

        private void OnMonsterSpawnRequestCommand(MonsterSpawnRequestCommand e)
        {
            var monsterView = Instantiate(e.MonsterSO.prefab, e.Position, Quaternion.identity);
            var slimeView = monsterView.GetComponent<ISlimeView>();
            SlimeMVC slimeMVC = new(Context, slimeView, e.MonsterSO);
            slimeMVC.Initialize();

            OnMonsterSpawned?.Invoke(slimeMVC.slimeController);
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized){throw new Exception("MustBeInitialized");}
        }

        
    }
}
