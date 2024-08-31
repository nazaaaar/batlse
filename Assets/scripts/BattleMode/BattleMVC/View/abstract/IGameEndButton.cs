using System;
using RMC.Mini.View;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface IGameEndButton: IView
    {
        event System.Action OnMainMenuClicked;
    }
}
