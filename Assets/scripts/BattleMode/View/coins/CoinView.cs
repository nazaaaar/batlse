using System;
using System.Collections;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nazaaaar.platformBattle.mini.view
{
    public class CoinView : MonoBehaviour, ICoinView
    {

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

        public event Action OnCoinTouched;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;
            }
        }

        private void Start(){
            StartCoroutine(FallingCycle());
        }

        private IEnumerator FallingCycle(){
            while (canCoinFall){
                yield return new WaitForSeconds(coinPeriodTime);
                spawnOnTilemap.SpawnPrefabOnRandomCell(coinPrefab, coinTilemap, transform);    
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
