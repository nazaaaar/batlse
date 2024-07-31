using nazaaaar.platformBattle.mini.model;
using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;

namespace nazaaaar.slime.mini.model
{
    public class SlimeModel: BaseModel
    {
        public Observable<int> Damage {get;} = new();
        public Observable<float> MoveSpeed {get;} = new();

        public Observable<MonterState> MonterState {get; } = new();

        public Observable<Team> Team { get; } = new();

        public Observable<int> Health { get; } = new();
        public Observable<float> AttackSpeed { get; } = new();
        public Observable<float> AgroRange { get; } = new();
        public Observable<float> Speed { get; } = new();

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);

                MonterState.Value = model.MonterState.Moving;
            }
        }
    }
}
