
using System;
using nazaaaar.platformBattle.mini.model;
using RMC.Mini.View;
using UnityEngine;

namespace nazaaaar.slime.mini.viewAbstract
{
    public interface ISlimeView : IView
    {
        Vector3 Position { get;}

        event Action<IMonster> OnTargetHit;
        void DestroySelf();

        GameObject GameObject{get;}
        event Action OnEndLinePassed;
    }
}