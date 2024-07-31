using System;
using nazaaaar.platformBattle.mini.model;
using RMC.Mini.View;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface IMonsterSpawner: IView
    {
        public event Action<IMonster> OnMonsterSpawned;
    }
}
