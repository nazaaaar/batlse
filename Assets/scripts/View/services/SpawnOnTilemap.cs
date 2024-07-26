using nazaaaar.platformBattle.mini.view.services;
using nazaaaar.platformBattle.mini.viewAbstract.services;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nazaaaar.platformBattle.mini.view
{
    public class SpawnOnTilemap : SpawnOnTilemapMono
    {
        [SerializeField]
        private float yLevel;

        public override GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition)
        {
            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);

            Vector3 cellCenterPosition = worldPosition + tilemap.cellSize / 2;

            cellCenterPosition.y = yLevel;

            return Instantiate(prefab, cellCenterPosition, Quaternion.identity);
        }

        public override GameObject SpawnPrefabOnCell(GameObject prefab, Tilemap tilemap, Vector3Int cellPosition, Transform parent)
        {
            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);

            Vector3 cellCenterPosition = worldPosition + tilemap.cellSize / 2;

            cellCenterPosition.y = yLevel;

            return Instantiate(prefab, cellCenterPosition, Quaternion.identity, parent);
        }
    }
}
