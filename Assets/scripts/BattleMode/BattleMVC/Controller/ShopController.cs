using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using RMC.Mini.Controller;
using System;
using System.Linq;
using UnityEngine;
namespace nazaaaar.platformBattle.mini.controller
{
    public class ShopController : IController
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public readonly ICoinView coinView;
        private readonly ICoinCollector coinCollector;

        

        private readonly PlayerModel playerModel;
        private readonly IShopZoneCollector shopZoneCollector;
        private readonly ShopModel shopModel;
        private readonly IShopView shopView;
        private readonly ISpawnPointer spawnPointer;
        private readonly NetworkCoinsModel networkCoinsModel;
        private readonly Team team;
        private readonly ShopZoneEnteredCommand shopZoneEnteredCommand = new();
        private readonly ShopZoneExitedCommand shopZoneExitedCommand = new();
        private readonly ActiveShopCardsChangedCommand activeShopCardsChangedCommand = new();
        private readonly CouldBeBoughtShopCardsChangedCommand couldBeBoughtShopCardsChangedCommand = new();
        private readonly MonsterSpawnRequestCommand monsterSpawnRequestCommand = new();
        private readonly TeamChangedCommand teamChangedCommand = new();
        private readonly MoneyAddRequestCommand moneyAddRequestCommand = new();

        
        

        public ShopController(ICoinView coinView, ICoinCollector coinCollector, PlayerModel playerModel, IShopZoneCollector shopZoneCollector, ShopModel shopModel, IShopView shopView, ISpawnPointer spawnPointer, NetworkCoinsModel networkCoinsModel, Team team)
        {
            this.coinView = coinView;
            this.coinCollector = coinCollector;
            this.playerModel = playerModel;
            this.shopZoneCollector = shopZoneCollector;
            this.shopModel = shopModel;
            this.shopView = shopView;
            this.spawnPointer = spawnPointer;
            this.networkCoinsModel = networkCoinsModel;
            this.team = team;
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

                playerModel.Team.OnValueChanged.AddListener(TeamValueChanged);
                coinCollector.OnCoinCollected+=View_OnCoinCollected;
                
                shopZoneCollector.OnShopZoneExited += View_OnShopZoneExired;
                shopZoneCollector.OnShopZoneEntered += View_OnShopZoneEntered;
                shopModel.ShopCards.OnValueChanged.AddListener(ShopCardsValueChanged);
                shopModel.ActiveShopCards.OnValueChanged.AddListener(ActiveShopCardsValueChanged);
                shopView.OnAllShopCardsSoChanged += View_OnAllShopCardsSoChanged;
                shopView.OnShopCardClick += View_OnShopCardClick;

                Context.CommandManager.AddCommandListener<PlayerCoinAmountChangedCommand>(OnCoinAmountChange);
                

                playerModel.Team.Value = team;
            }
        }

        private void OnCoinAmountChange(PlayerCoinAmountChangedCommand e)
        {
            HandlePlayerMoneyChangedForCards();
        }

        private void TeamValueChanged(Team oldValue, Team newValue)
        {
            teamChangedCommand.Team = newValue;
            Context.CommandManager.InvokeCommand(teamChangedCommand);

            moneyAddRequestCommand.team=newValue;
        }

        private void View_OnShopCardClick(ShopCardSO sO)
        {
            monsterSpawnRequestCommand.Position = spawnPointer.Position;
            monsterSpawnRequestCommand.Position.y = 0;
            monsterSpawnRequestCommand.MonsterSO = sO.monsterSO;
            monsterSpawnRequestCommand.team = playerModel.Team.Value;
            Context.CommandManager.InvokeCommand(monsterSpawnRequestCommand);
            ShuffleActiveShopCards();
            //playerModel.Money.Value-=sO.cost;
            moneyAddRequestCommand.amount = -sO.cost;
            Context.CommandManager.InvokeCommand(moneyAddRequestCommand);
        }

        private void View_OnAllShopCardsSoChanged(ShopCardSO[] so)
        {
            ShopCard[] shopCards = new ShopCard[so.Length];
            for (int i = 0; i < so.Length; i++){
                shopCards[i] = new ShopCard
                {
                    shopCardSO = so[i]
                };
            }
            shopModel.ShopCards.Value = shopCards;
        }

        private void ActiveShopCardsValueChanged(ShopCard[] oldValue, ShopCard[] newValue)
        {
            CheckShopCardsForMoney(newValue);
            activeShopCardsChangedCommand.ShopCards = newValue;
            Context.CommandManager.InvokeCommand(activeShopCardsChangedCommand);
        }

        private void CheckShopCardsForMoney(ShopCard[] newValue)
        {
            foreach (var shopCard in newValue)
            {
                shopCard.CouldBeChanged = shopCard.shopCardSO.cost <= networkCoinsModel.MyCoins().Value;
            }
        }

        private void ShopCardsValueChanged(ShopCard[] oldValue, ShopCard[] newValue)
        {
            ShuffleActiveShopCards();
        }

        private void ShuffleActiveShopCards()
        {
            if (shopModel.ShopCards.Value == null || shopModel.ShopCards.Value.Length < 3)
            {
                throw new InvalidOperationException("Not enough shop cards to pick from.");
            }

            ShopCard[] shuffledShopCards = shopModel.ShopCards.Value.OrderBy(x => UnityEngine.Random.value).ToArray();

            ShopCard[] pickedCards = shuffledShopCards.Take(3).ToArray();

            shopModel.ActiveShopCards.Value = pickedCards;
        }

        private void View_OnShopZoneEntered()
        {
            Context.CommandManager.InvokeCommand(shopZoneEnteredCommand);
        }

        private void View_OnShopZoneExired()
        {
            Context.CommandManager.InvokeCommand(shopZoneExitedCommand);
        }

        

        private void HandlePlayerMoneyChangedForCards()
        {
            CheckShopCardsForMoney(shopModel.ActiveShopCards.Value);
            bool[] couldBeBought = new bool[3];
            for (int i = 0; i < shopModel.ActiveShopCards.Value.Length; i++)
            {
                couldBeBought[i] = shopModel.ActiveShopCards.Value[i].CouldBeChanged;
            }
            couldBeBoughtShopCardsChangedCommand.CouldBeBought = couldBeBought;
            Context.CommandManager.InvokeCommand(couldBeBoughtShopCardsChangedCommand);
        }

        private void View_OnCoinCollected(GameObject coin)
        {
            coin.SetActive(false);
            moneyAddRequestCommand.amount = 1;
            Context.CommandManager.InvokeCommand(moneyAddRequestCommand);
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized){throw new Exception("MustBeInitialized");}
        }
    }
}
