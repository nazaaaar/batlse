using RMC.Mini.View;
using System;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface IPlayerView: IView
    {
        public event Action<Vector3> OnPlayerMoved;
    }
}
