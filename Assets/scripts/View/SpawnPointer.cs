using System;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class SpawnPointer : MonoBehaviour, ISpawnPointer
    {
        [SerializeField]
        private Transform player;
        [SerializeField]
        private Transform pointer;
        [SerializeField]
        private GameObject[] tilemapGameObjects;
        [SerializeField]
        private Vector3 offset;
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized=true;
                Context = context;
                context.CommandManager.AddCommandListener<ShopZoneEnteredCommand>(OnShopEntered);
                context.CommandManager.AddCommandListener<ShopZoneExitedCommand>(OnShopExited);
            }
        }

        private void OnShopExited(ShopZoneExitedCommand e)
        {
            Context.CommandManager.RemoveCommandListener<PlayerMovedCommand>(OnPlayerMoved);
            pointer.gameObject.SetActive(false);
        }

        private void OnShopEntered(ShopZoneEnteredCommand e)
        {
            Context.CommandManager.AddCommandListener<PlayerMovedCommand>(OnPlayerMoved);
            MovePointerToClosestTile();
            pointer.gameObject.SetActive(true);
        }

        private void OnPlayerMoved(PlayerMovedCommand e)
        {
            MovePointerToClosestTile();
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }

        private void MovePointerToClosestTile()
        {
            GameObject closestTile = null;
            float minDistance = Mathf.Infinity;
            Vector3 playerPosition = player.position;

        
            foreach (GameObject tile in tilemapGameObjects)
            {
                float distance = Vector3.SqrMagnitude(playerPosition - tile.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTile = tile;
                }
            }


            if (closestTile != null)
            {
                pointer.position = closestTile.transform.position + offset;
            }
        }
    }
}