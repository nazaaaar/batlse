using System;
using RMC.Mini.View;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface IShopZoneCollector: IView
    {
        public event Action OnShopZoneEntered;
        public event Action OnShopZoneExited;
    }
}