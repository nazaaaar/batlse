using System;
using System.Linq;
using nazaaaar.networkUtils;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using nazaaaar.slime.mini;
using nazaaaar.slime.mini.viewAbstract;
using RMC.Mini;
using Unity.Netcode;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class MonsterNetworkSpawner : NetworkBehaviour, IMonsterSpawner
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public event Action<IMonster> OnMonsterSpawned;

        private ShopModel shopModel;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                Context.CommandManager.AddCommandListener<MonsterSpawnRequestCommand>(OnMonsterSpawnRequestCommand);
                shopModel = Context.ModelLocator.GetItem<ShopModel>();

                Context.CommandManager.AddCommandListener<MonsterDespawnRequestCommand>(OnMonsterDespawnRequest);
            }
        }

        private void OnMonsterDespawnRequest(MonsterDespawnRequestCommand e)
        {
            if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(e.monster.GetComponent<NetworkObject>().NetworkObjectId, out NetworkObject networkObject)){
                
                NetworkObjectPool.Singleton.ReturnNetworkObject(networkObject, e.monsterSO.prefab);
                networkObject.Despawn(destroy: false);
                
            }
        }

        private void OnMonsterSpawnRequestCommand(MonsterSpawnRequestCommand e)
        {   
            MonsterSpawnRpc(e.Position,e.team, e.MonsterSO.Id);
        }


        [Rpc(SendTo.Server)]
        private void MonsterSpawnRpc(Vector3 position, Team team, string monsterSoId)
        {

            MonsterSO monsterSO = shopModel.MonsterSOHashMap.SearchByID(monsterSoId);

            SlimeInitializer slimeInitializer = new(team);

            var monsterView = NetworkObjectPool.Singleton.GetNetworkObject(monsterSO.prefab, position, slimeInitializer.Rotation);
            monsterView.Spawn();
            
            SlimeMVC slimeMVC = slimeInitializer.Initialize(monsterSO, monsterView, Context);

            OnMonsterSpawned?.Invoke(slimeMVC.slimeController);
        }

        

        public void RequireIsInitialized()
        {
            if(!IsInitialized){throw new Exception("MustBeInitialized");}
        }        
    }
}
