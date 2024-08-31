using System;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using RMC.Mini.Controller;
using Unity.Netcode;
using UnityEngine;
using static nazaaaar.platformBattle.mini.controller.commands.GameFinishedCommand;

namespace nazaaaar.platformBattle.mini.controller
{
    public class NetworkController : IController
    {
        public bool IsInitialized{get; private set;}

        public IContext Context{get; private set;}

        private readonly ICoinNetworkSpawner coinNetworkSpawner;
        private readonly ICoinView coinView;
        private readonly ICoinCollector coinCollector;
        private readonly NetworkCoinsModel networkCoinsModel;
        private readonly IMonsterSpawner monsterNetworkSpawner;
        private readonly PlayerCoinAmountChangedCommand playerCoinAmountChangedCommand = new();
        private readonly OtherCoinAmountChangedCommand otherCoinAmountChangedCommand = new();

        private readonly CoinNetworkSpawnCommand coinNetworkSpawnCommand = new();
        private readonly CoinNetworkDespawnCommand coinNetworkDespawnCommand = new();

        private readonly MonsterSpawnedCommand monsterSpawnedCommand = new();


        public NetworkController(NetworkCoinsModel networkCoinsModel, ICoinCollector coinCollector, ICoinView coinView, ICoinNetworkSpawner coinNetworkSpawner, IMonsterSpawner monsterNetworkSpawner)
        {
            this.networkCoinsModel = networkCoinsModel;
            this.coinCollector = coinCollector;
            this.coinView = coinView;
            this.coinNetworkSpawner = coinNetworkSpawner;
            this.monsterNetworkSpawner = monsterNetworkSpawner;
        }

        public void Dispose()
        {
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;   
                Context = context;

                coinView.OnCoinSpawnRequest += View_OnCoinSpawnRequest;
                coinCollector.OnCoinCollected+=View_OnCoinCollected;
                networkCoinsModel.MyCoins().OnValueChanged += PlayerMoneyValueChanged;
                networkCoinsModel.OtherCoins().OnValueChanged += OtherMoneyValueChanged;
                monsterNetworkSpawner.OnMonsterSpawned+=View_OnMonsterSpawned;

                Context.CommandManager.AddCommandListener<MoneyAddRequestCommand>(OnMoneyAddRequest);
                Context.CommandManager.AddCommandListener<GameTimePassed>(OnGameTimePassed);
                Context.CommandManager.AddCommandListener<MainMenuClickedCommand>(OnMainMenuClicked);
            }
        }

        private void OnMainMenuClicked(MainMenuClickedCommand e)
        {
            NetworkManager.Singleton.Shutdown();
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameStart");
        }

        private void OnGameTimePassed(GameTimePassed e)
        {
            GameEndState gameEndState;
            if (networkCoinsModel.MyCoins().Value > networkCoinsModel.OtherCoins().Value){
                gameEndState = GameEndState.Win;
            }
            else if (networkCoinsModel.MyCoins().Value < networkCoinsModel.OtherCoins().Value)
                gameEndState = GameEndState.Lose;
            else gameEndState = GameEndState.Draw;

            Context.CommandManager.InvokeCommand(new GameFinishedCommand(){gameEndState = gameEndState});
        }

        private void View_OnMonsterSpawned(IMonster monster)
        {
            monsterSpawnedCommand.Monster = monster;
            Context.CommandManager.InvokeCommand(monsterSpawnedCommand);
        }

        private void OtherMoneyValueChanged(int previousValue, int newValue)
        {
            otherCoinAmountChangedCommand.coinAmount=newValue;
            Context.CommandManager.InvokeCommand(otherCoinAmountChangedCommand);
        }

        private void OnMoneyAddRequest(MoneyAddRequestCommand e)
        {
            networkCoinsModel.AddCoinsRpc(e.amount,e.team);
        }

        private void PlayerMoneyValueChanged(int oldValue, int newValue)
        {
            playerCoinAmountChangedCommand.coinAmount = newValue;
            Context.CommandManager.InvokeCommand(playerCoinAmountChangedCommand);
        }

        private void View_OnCoinSpawnRequest(Vector3 vector3)
        {
            coinNetworkSpawnCommand.coinPos = vector3;
            Context.CommandManager.InvokeCommand(coinNetworkSpawnCommand);
        }

        private void View_OnCoinCollected(GameObject coin)
        {
            coinNetworkDespawnCommand.coin = coin.GetComponent<NetworkObject>();
            Context.CommandManager.InvokeCommand(coinNetworkDespawnCommand);   
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){
                throw new System.Exception("MustBeInitialized");
            }
        }
    }
}