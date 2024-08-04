using UnityEngine;
using UnityEngine.Tilemaps;

namespace nazaaaar.platformBattle.mini.viewAbstract.services
{
    public interface ISpawnOnTilemap
    {
        public GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition);
        public GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition, Transform parent);
    }
}
