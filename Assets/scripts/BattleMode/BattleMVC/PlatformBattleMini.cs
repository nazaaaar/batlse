using nazaaaar.platformBattle.mini.controller;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.service;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using System;
using UnityEngine.InputSystem;
namespace nazaaaar.platformBattle.mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: A mini MVCS for core gameplay logic
    /// </summary>
    public class PlatformBattleMini: ISimpleMiniMvcs
    {

        public bool IsInitialized => isInitialized;

        public ShopController ShopController { get => shopController; set => shopController = value; }
        private readonly IPlayerView playerView;
        private readonly PlayerInput playerInput;
        private readonly IPlayerAnimation playerAnimation;
        private readonly CameraFollow cameraFollow;
        private readonly ICoinView coinView;
        private readonly ICoinCollector coinCollector;
        private readonly IShopView shopView;
        private readonly CoinRotation coinRotation;
        private readonly ICoinFall coinFall;
        private readonly IShopZoneCollector shopZoneCollector;
        private readonly SpawnPointer spawnPointer;
        private readonly ICoinAmountUI coinAmountUI;
        private readonly ICoinAmountUI otherCoinAmountUI;
        private readonly IMonsterSpawner monsterSpawner;
        private readonly ICoinNetworkSpawner coinNetworkSpawner;
        private readonly NetworkCoinsModel networkCoinsModel;
        private readonly TimerService timerService;
        private readonly ITimerView timerView;
        private readonly IGameEndView gameEndView;
        private readonly IGameEndButton gameEndButton;
        private readonly Team team;
        private readonly int maxTime;
        private MonstersController monstersController;
        private NetworkController networkController;
        private MonstersList monstersList;
        private ShopController shopController;
        private RewardController rewardController;

        private PlayerModel playerModel;
        private TimeModel timeModel;
        private TimeController timeController;
        private ShopModel shopModel;

        private IContext context;

        private PlayerMovementController playerMovementController;

        private bool isInitialized;
        

        public PlatformBattleMini(IPlayerView playerView, PlayerInput playerInput, IPlayerAnimation playerAnimation, CameraFollow cameraFollow, CoinView coinView, CoinCollector coinCollector, IShopView shopView, IShopZoneCollector shopZoneCollector, SpawnPointer spawnPointer, ICoinAmountUI coinAmountUI, ICoinAmountUI otherCoinAmountUI, IMonsterSpawner monsterSpawner, ICoinNetworkSpawner coinNetworkSpawner, NetworkCoinsModel networkCoinsModel, service.TimerService timerService, ITimerView timerView, IGameEndView gameEndView, IGameEndButton gameEndButton, Team team, int maxTime, IContext context)
        {
            this.playerView = playerView;
            this.playerInput = playerInput;
            this.playerAnimation = playerAnimation;
            this.cameraFollow = cameraFollow;
            this.coinView = coinView;
            this.coinCollector = coinCollector;
            this.shopView = shopView;
            this.shopZoneCollector = shopZoneCollector;
            this.spawnPointer = spawnPointer;
            this.coinAmountUI = coinAmountUI;
            this.otherCoinAmountUI = otherCoinAmountUI;
            this.monsterSpawner = monsterSpawner;
            this.coinNetworkSpawner = coinNetworkSpawner;
            this.networkCoinsModel = networkCoinsModel;
            this.timerService = timerService;
            this.timerView = timerView;
            this.gameEndView = gameEndView;
            this.gameEndButton = gameEndButton;
            this.team = team;
            this.maxTime = maxTime;
            this.context = context;
        }

        public void Initialize()
        {
            if (!isInitialized){
                isInitialized = true;
                               
                
                
                
                playerModel = new();
                timeModel = new(maxTime);
                monstersList = new();
                shopModel = new();
                timeController = new(timerService,timeModel,timerView, gameEndButton);
                rewardController = new();
                monstersController = new(monstersList);
                shopController = new (coinView, coinCollector, playerModel, shopZoneCollector, shopModel,shopView,spawnPointer, networkCoinsModel, team);
                playerMovementController = new (playerInput, playerView, playerModel);
                networkController = new(networkCoinsModel,coinCollector,coinView,coinNetworkSpawner,monsterSpawner);
                
                playerModel.Initialize (context);
                timeModel.Initialize(context);
                timerService.Initialize(context);
                timerView.Initialize(context);
                monstersList.Initialize(context);
                shopModel.Initialize (context);
                networkCoinsModel.Initialize(context);
                coinNetworkSpawner.Initialize(context);
                monsterSpawner.Initialize(context);
                coinCollector.Initialize(context);
                coinView.Initialize(context);
                cameraFollow.Initialize(context);
                spawnPointer.Initialize(context);
                playerView.Initialize (context);
                playerAnimation.Initialize(context);
                gameEndView.Initialize(context);
                shopController.Initialize(context);
                playerMovementController.Initialize (context);
                networkController.Initialize(context);
                timeController.Initialize(context);
                monstersController.Initialize(context);
                rewardController.Initialize(context);


                gameEndButton.Initialize(context);
                shopView.Initialize(context);
                shopZoneCollector.Initialize(context);
                
                coinAmountUI.Initialize(context);
                otherCoinAmountUI.Initialize(context);

                context.CommandManager.InvokeCommand(new GameLoadedCommand());
            }
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
    }
}
