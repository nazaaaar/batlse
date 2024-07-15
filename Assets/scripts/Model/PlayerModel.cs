using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;
namespace nazaaaar.platformBattle.mini.model
{
    public class PlayerModel: BaseModel
    {
        private readonly Observable<int> playerMoney = new();

        public Observable<int> PlayerMoney => playerMoney;

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);
                playerMoney.Value = 0;
            }
        }
    }
}
