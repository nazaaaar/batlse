using RMC.Mini.View;
using System;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface ICoinCollector: IView
        {
            public event Action OnCoinCollected;
        }
}
