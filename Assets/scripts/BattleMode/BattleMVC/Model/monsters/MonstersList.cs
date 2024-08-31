using System.Collections.Generic;
using RMC.Mini;
using RMC.Mini.Model;
namespace nazaaaar.platformBattle.mini.model
{
    public class MonstersList: BaseModel
    {
        public List<IMonster> BlueMonsters { get; private set; } = new();
        public List<IMonster> RedMonsters { get; private set; } = new();

        public override void Initialize(IContext context)
        {
            if (!IsInitialized){
                base.Initialize(context);
            }
        }
        
    }
}
