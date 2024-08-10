using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;
namespace nazaaaar.platformBattle.mini.model
{
    public class PlayerModel: BaseModel
    {

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);

            }
        }

        public Observable<Team> Team {get; } = new();
    }
}
