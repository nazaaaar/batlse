using RMC.Mini.View;
using System;
namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface IPlayerInput: IView
    {
        public Action OnRightPressedUp { get; set; }
        public Action OnLeftPressedUp { get; set; }
        public Action OnUpPressedUp { get; set; }
        public Action OnDownPressedUp { get; set; }
        public Action OnRightPressedDown { get; set; }
        public Action OnLeftPressedDown { get; set; }
        public Action OnUpPressedDown { get; set; }
        public Action OnDownPressedDown { get; set; }
    }
}
