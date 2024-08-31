using nazaaaar.networkUtils;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using Unity.Netcode;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class CoinNetworkSpawner : NetworkBehaviour, ICoinNetworkSpawner
    {
        public bool IsInitialized{get; private set;}

        public IContext Context{get; private set;}

        [SerializeField]
        private GameObject coinPrefab;

        private Vector3 networkPos;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                Context.CommandManager.AddCommandListener<CoinNetworkSpawnCommand>(OnCoinSpawnRequest);
                Context.CommandManager.AddCommandListener<CoinNetworkDespawnCommand>(OnCoinDespawnRequest);
            }
        }

        private void OnCoinSpawnRequest(CoinNetworkSpawnCommand e)
        {
            networkPos = e.coinPos;

            
            CoinCreateRpc(networkPos.x, networkPos.y, networkPos.z);
        }

        private void OnCoinDespawnRequest(CoinNetworkDespawnCommand e)
        {   
            CoinDeleteRpc(e.coin.NetworkObjectId);
        }

        [Rpc(SendTo.Server)]
        private void CoinCreateRpc(float x, float y, float z){
            
            var networkObject = NetworkObjectPool.Singleton.GetNetworkObject(coinPrefab, new Vector3(x, y, z), Quaternion.identity);
            networkObject.Spawn();
        }


        [Rpc(SendTo.Server)]
        private void CoinDeleteRpc(ulong networkObjectId){
            if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject)){
                
                NetworkObjectPool.Singleton.ReturnNetworkObject(networkObject, coinPrefab);
                networkObject.Despawn(destroy: false);
                
            }
        }
        

        public void RequireIsInitialized()
        {
            if (!IsInitialized){
                throw new System.Exception("MustBeInitialized");
            }
        }
    }
}