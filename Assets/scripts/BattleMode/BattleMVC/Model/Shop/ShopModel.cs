using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;

namespace nazaaaar.platformBattle.mini.model
{
    public class ShopModel : BaseModel
    {
        public Observable<ShopCard[]> ShopCards { get; } = new();
        public Observable<ShopCard[]> ActiveShopCards { get; } = new();

        public MonsterSOHashMap MonsterSOHashMap {get; private set;}

        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                ShopCards.OnValueChanged.AddListener(ShopCardValueChanged);
            }
        }

        private void ShopCardValueChanged(ShopCard[] oldValue, ShopCard[] newValue)
        {
            MonsterSOHashMap = new MonsterSOHashMap();
            foreach (var shopCard in newValue){
                MonsterSOHashMap.AddCard(shopCard.shopCardSO.monsterSO);
                UnityEngine.Debug.Log("id" + shopCard.shopCardSO.monsterSO.Id);
            }
        }
    }
}
