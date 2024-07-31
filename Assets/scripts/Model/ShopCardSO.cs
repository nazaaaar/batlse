using UnityEngine;

namespace nazaaaar.platformBattle.mini.model
{
    [CreateAssetMenu(fileName = "ShopCard", menuName = "ShopCard", order = 0)]
    public class ShopCardSO: ScriptableObject
    {
        public string title;
        public Sprite image;
        public string description;
        public int cost;

        public MonsterSO monsterSO;
    }
}
