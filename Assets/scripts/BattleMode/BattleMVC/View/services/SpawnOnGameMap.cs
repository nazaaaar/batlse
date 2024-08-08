using System;
using nazaaaar.platformBattle.mini.view.services;
using nazaaaar.platformBattle.mini.viewAbstract.services;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nazaaaar.platformBattle.mini.view
{
    public class SpawnOnGameMap : MonoBehaviour, ISpawnOnGameMap
    {
        public model.GameMapConfig GameMapConfig { get; set; }
        [SerializeField]
        private SpawnOnTilemapMono spawnOnTilemap;

        public GameObject SpawnPrefabOnRandomCell(GameObject prefab, Tilemap tilemap, Transform parent){
            if (GameMapConfig==null) throw new Exception("GameMapConfigIsNull");
            Vector3Int cellPosition = new Vector3Int(UnityEngine.Random.Range(0,GameMapConfig.sizeX), UnityEngine.Random.Range(0,GameMapConfig.sizeY));
            return this.SpawnPrefabOnCell(prefab, tilemap,cellPosition, parent);   
        }

        public GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition)
        {
            if (spawnOnTilemap == null) throw new Exception("NotInitialized");
            
            if (cellPosition.x > GameMapConfig.sizeX || cellPosition.y > GameMapConfig.sizeY
            || cellPosition.x<0 || cellPosition.y<0) throw new Exception("OutOfMap");
            cellPosition.x += GameMapConfig.offsetX;
            cellPosition.y += GameMapConfig.offsetY;
            return  spawnOnTilemap.SpawnPrefabOnCell(prefab, tilemap, cellPosition);
        }


        public GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition, Transform parent)
        {
            if (spawnOnTilemap == null) throw new Exception("NotInitialized");

            if (cellPosition.x > GameMapConfig.sizeX || cellPosition.y > GameMapConfig.sizeY
            || cellPosition.x<0 || cellPosition.y<0) throw new Exception("OutOfMap");
            cellPosition.x += GameMapConfig.offsetX;
            cellPosition.y += GameMapConfig.offsetY;
            return  spawnOnTilemap.SpawnPrefabOnCell(prefab, tilemap, cellPosition, parent);
        }

        public Vector3 GetVector3(Tilemap tilemap)
        {
            Vector3Int cellPosition = new Vector3Int(UnityEngine.Random.Range(0,GameMapConfig.sizeX), UnityEngine.Random.Range(0,GameMapConfig.sizeY));
            if (spawnOnTilemap == null) throw new Exception("NotInitialized");
            
            if (cellPosition.x > GameMapConfig.sizeX || cellPosition.y > GameMapConfig.sizeY
            || cellPosition.x<0 || cellPosition.y<0) throw new Exception("OutOfMap");
            cellPosition.x += GameMapConfig.offsetX;
            cellPosition.y += GameMapConfig.offsetY;

            return spawnOnTilemap.GetVector3(tilemap, cellPosition);
        }
    }

    public interface ISpawnOnGameMap: ISpawnOnTilemap
    {
        public GameObject SpawnPrefabOnRandomCell(GameObject prefab, Tilemap tilemap, Transform parent);
        Vector3 GetVector3(Tilemap tilemap);
    }
}
