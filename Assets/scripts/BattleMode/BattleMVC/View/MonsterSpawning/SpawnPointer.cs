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
        
        private GameObject[] tilemapGameObjects;
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private GameObject[] redGameObjects;
        [SerializeField]
        private GameObject[] blueGameObjects;
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public Vector3 Position => pointer.position;

        public Transform Player { get => player; set => player = value; }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized=true;
                Context = context;
                
                context.CommandManager.AddCommandListener<TeamChangedCommand>(OnTeamChanged);
                context.CommandManager.AddCommandListener<GameFinishedCommand>(OnGameEnd);
            }
        }

        private void OnGameEnd(GameFinishedCommand e)
        {
            Context.CommandManager.RemoveCommandListener<TeamChangedCommand>(OnTeamChanged);
            Context.CommandManager.RemoveCommandListener<ShopZoneEnteredCommand>(OnShopEntered);
            Context.CommandManager.RemoveCommandListener<ShopZoneExitedCommand>(OnShopExited);
            Context.CommandManager.RemoveCommandListener<PlayerMovedCommand>(OnPlayerMoved);
        }

        private void OnTeamChanged(TeamChangedCommand e)
        {
            if (e.Team == model.Team.Blue) {tilemapGameObjects = blueGameObjects;}
            else if (e.Team == model.Team.Red) {tilemapGameObjects = redGameObjects;}

            Context.CommandManager.AddCommandListener<ShopZoneEnteredCommand>(OnShopEntered);
            Context.CommandManager.AddCommandListener<ShopZoneExitedCommand>(OnShopExited);
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