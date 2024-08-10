using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;

namespace nazaaaar.platformBattle.MainMenu.controller{
    public class OtherPlayerModel: BaseModel
    {
        public Observable<int> Money { get; } = new();

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);
            }
        }
    }
}