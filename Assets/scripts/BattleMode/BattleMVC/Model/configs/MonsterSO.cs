using UnityEngine;

namespace nazaaaar.platformBattle.mini.model
{
    [CreateAssetMenu(fileName = "MonsterSO", menuName = "MonsterSO", order = 1)]
    public class MonsterSO: ScriptableObject{
        public GameObject prefab;

        public int health;
        public int damage;
        public float agroRange;
        public float speed;
        public float attackSpeed;
    }
}
