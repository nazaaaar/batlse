using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Core.Events;
using RMC.Mini;
using RMC.Mini.Controller;
using System;
using System.Diagnostics;
namespace nazaaaar.platformBattle.mini.controller
{
    public class PlatformBattleController : IController
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public readonly ICoinView coinView;
        private readonly ICoinCollector coinCollector;

        private readonly PlayerCoinAmountChangedCommand playerCoinAmountChangedCommand = new();

        private readonly PlayerModel playerModel;
        private readonly IShopZoneCollector shopZoneCollector;

        private readonly ShopZoneEnteredCommand shopZoneEnteredCommand = new();
        private readonly ShopZoneExitedCommand shopZoneExitedCommand = new();

        public PlatformBattleController(ICoinView coinView, ICoinCollector coinCollector, PlayerModel playerModel, IShopZoneCollector shopZoneCollector)
        {
            this.coinView = coinView;
            this.coinCollector = coinCollector;
            this.playerModel = playerModel;
            this.shopZoneCollector = shopZoneCollector;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;
                coinCollector.OnCoinCollected+=View_OnCoinCollected;
                playerModel.Money.OnValueChanged.AddListener(PlayerMoneyValueChanged);
                shopZoneCollector.OnShopZoneExited += View_OnShopZoneExired;
                shopZoneCollector.OnShopZoneEntered += View_OnShopZoneEntered;
            }
        }

        private void View_OnShopZoneEntered()
        {
            Context.CommandManager.InvokeCommand(shopZoneEnteredCommand);
        }

        private void View_OnShopZoneExired()
        {
            Context.CommandManager.InvokeCommand(shopZoneExitedCommand);
        }

        private void PlayerMoneyValueChanged(int oldValue, int newValue)
        {
            playerCoinAmountChangedCommand.coinAmount = newValue;
            Context.CommandManager.InvokeCommand(playerCoinAmountChangedCommand);
        }

        private void View_OnCoinCollected()
        {
            playerModel.Money.Value+=1;
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized){throw new Exception("MustBeInitialized");}
        }
    }
}
