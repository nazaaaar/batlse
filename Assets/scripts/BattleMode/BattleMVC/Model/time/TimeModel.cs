using RMC.Core.Observables;
using RMC.Mini;
using RMC.Mini.Model;
namespace nazaaaar.platformBattle.mini.model
{
    public class TimeModel: BaseModel
    {
        public TimeModel(int maxTime)
        {
            MaxTime.Value = maxTime;
        }

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);

            }
        }

        public Observable<int> Time {get; } = new();
        public Observable<int> MaxTime {get; } = new();
    }
}
