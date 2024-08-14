
using System;
using nazaaaar.platformBattle.mini.model;
using RMC.Mini.View;
using UnityEngine;

namespace nazaaaar.slime.mini.viewAbstract
{
    public interface ISlimeAnimation : IView
    {
        event Action OnSlimeDiedEndAnimation;

        void DestroySelf();
    }
}