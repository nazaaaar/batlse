using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;

namespace nazaaaar.platformBattle.mini.model
{
    public class ShopModel : BaseModel
    {
        public Observable<ShopCard[]> ShopCards { get; } = new();
        public Observable<ShopCard[]> ActiveShopCards { get; } = new();

        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
            }
        }        
    }
}
