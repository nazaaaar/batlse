using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;

namespace nazaaaar.platformBattle.MainMenu.model
{
    public class PlayerModel: BaseModel
    {
        public Observable<CurrentPage> CurrentPage { get; } = new();

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);

                CurrentPage.Value = model.CurrentPage.StartPage;
            }
        }
    }
}