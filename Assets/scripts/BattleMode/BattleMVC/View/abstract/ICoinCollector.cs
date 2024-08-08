using RMC.Mini.View;
using System;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface ICoinCollector: IView
        {
            public event Action<GameObject> OnCoinCollected;
        }
}
