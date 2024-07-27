using System;
using nazaaaar.platformBattle.mini.model;
using RMC.Mini.View;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface IShopView: IView
    {
        public event Action<ShopCardSO[]> OnAllShopCardsSoChanged;
    }
}