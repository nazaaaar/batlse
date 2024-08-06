using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;
namespace nazaaaar.platformBattle.mini.model
{
    public class PlayerModel: BaseModel
    {
        private readonly Observable<int> money = new();

        public Observable<int> Money => money;

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);
                money.Value = 0;
            }
        }

        public Observable<Team> Team {get; } = new();
    }
}
