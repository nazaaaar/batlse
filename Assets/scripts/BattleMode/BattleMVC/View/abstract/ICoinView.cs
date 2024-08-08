using System;
using RMC.Mini.View;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface ICoinView: IView
    {
        public event Action<Vector3> OnCoinSpawnRequest;
    }
}
