using nazaaaar.platformBattle.mini.model;
using RMC.Mini.Controller.Commands;

namespace nazaaaar.platformBattle.mini.controller.commands{
    public class ActiveShopCardsChangedCommand: ICommand
    {
        public ShopCard[] ShopCards;
    }
}