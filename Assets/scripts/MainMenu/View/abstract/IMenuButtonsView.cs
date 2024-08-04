using System;
using RMC.Mini.View;

namespace nazaaaar.platformBattle.MainMenu.viewAbstract{
    public interface IMenuButtonsView: IView
    {
        public event Action OnStartPressed;
        public event Action OnSettingsPressed;
        public event Action OnBackPressed;
    }
}