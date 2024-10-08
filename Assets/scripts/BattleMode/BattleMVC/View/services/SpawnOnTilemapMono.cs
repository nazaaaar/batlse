using nazaaaar.platformBattle.mini.viewAbstract.services;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nazaaaar.platformBattle.mini.view.services
{
    public abstract class SpawnOnTilemapMono: MonoBehaviour, ISpawnOnTilemap
    {
        public abstract GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition);
        public abstract GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition, UnityEngine.Transform parent);
        public abstract Vector3 GetVector3(Tilemap tilemap, Vector3Int cellPosition);

    }
}
