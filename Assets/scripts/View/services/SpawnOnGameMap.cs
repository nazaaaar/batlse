using System;
using System.Runtime.Serialization;
using nazaaaar.platformBattle.mini.view.services;
using nazaaaar.platformBattle.mini.viewAbstract.services;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nazaaaar.platformBattle.mini.view
{
    public class SpawnOnGameMap : SpawnOnTilemapMono, ISpawnOnGameMap
    {
        [SerializeField]
        private int sizeX;
        [SerializeField]
        private int sizeY;

        [SerializeField]
        private int offsetX;
        [SerializeField]
        private int offsetY;
        [SerializeField]
        private SpawnOnTilemapMono spawnOnTilemap;

        public GameObject SpawnPrefabOnRandomCell(GameObject prefab, Tilemap tilemap, Transform parent){
            Vector3Int cellPosition = new Vector3Int(UnityEngine.Random.Range(0,sizeX), UnityEngine.Random.Range(0,sizeY));
            return spawnOnTilemap.SpawnPrefabOnCell(prefab, tilemap,cellPosition, parent);   
        }

        public override GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition)
        {
            if (spawnOnTilemap == null) throw new Exception("NotInitialized");
            Debug.Log(cellPosition.x + ", " + cellPosition.y + ", " + cellPosition.z);
            if (cellPosition.x > sizeX || cellPosition.y > sizeY
            || cellPosition.x<0 || cellPosition.y<0) throw new Exception("OutOfMap");
            cellPosition.x += offsetX;
            cellPosition.y += offsetY;
            return  spawnOnTilemap.SpawnPrefabOnCell(prefab, tilemap, cellPosition);
        }

        public override GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition, Transform parent)
        {
            if (spawnOnTilemap == null) throw new Exception("NotInitialized");
            Debug.Log(cellPosition.x + ", " + cellPosition.y + ", " + cellPosition.z);
            if (cellPosition.x > sizeX || cellPosition.y > sizeY
            || cellPosition.x<0 || cellPosition.y<0) throw new Exception("OutOfMap");
            cellPosition.x += offsetX;
            cellPosition.y += offsetY;
            return  spawnOnTilemap.SpawnPrefabOnCell(prefab, tilemap, cellPosition, parent);
        }
    }

    public interface ISpawnOnGameMap: ISpawnOnTilemap
    {
        public GameObject SpawnPrefabOnRandomCell(GameObject prefab, Tilemap tilemap, Transform parent);
    }
}
