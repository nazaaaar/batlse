using System;
using System.Collections;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nazaaaar.platformBattle.mini.view
{
    public class CoinView : MonoBehaviour, ICoinView
    {
        [SerializeField]
        private GameMapConfig redGameMapConfig;
        [SerializeField]
        private GameMapConfig blueGameMapConfig;

        [SerializeField]
        private GameObject coinPrefab;
        
        [SerializeField]
        private Tilemap coinTilemap;

        [SerializeField]
        private SpawnOnGameMap spawnOnTilemap;

        [SerializeField]
        private float coinPeriodTime;

        private bool canCoinFall = true;

        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public event Action<Vector3> OnCoinSpawnRequest;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;
                
                Context.CommandManager.AddCommandListener<TeamChangedCommand>(OnTeamChanged);
                
            }
        }

        private void OnTeamChanged(TeamChangedCommand e)
        {
            if (e.Team == model.Team.Red) 
                spawnOnTilemap.GameMapConfig = redGameMapConfig;
            if (e.Team == model.Team.Blue) 
                spawnOnTilemap.GameMapConfig = blueGameMapConfig;
            StartCoroutine(FallingCycle());
        }

        private IEnumerator FallingCycle(){
            while (canCoinFall){
                yield return WaitForEndPool.WaitForSeconds(coinPeriodTime);
                var coin = spawnOnTilemap.GetVector3(coinTilemap);    
             
                OnCoinSpawnRequest?.Invoke(coin);
            }
        }

        private void Spawn(Vector3Int vector3Int){
            spawnOnTilemap.SpawnPrefabOnCell(coinPrefab,coinTilemap,vector3Int);
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized){throw new Exception("MustBeInitialized");}
        }
    }
}
