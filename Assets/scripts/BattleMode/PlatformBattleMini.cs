using nazaaaar.platformBattle.mini.controller;
using nazaaaar.platformBattle.mini.model;
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

        public PlatformBattleController PlatformBattleController { get => platformBattleController; set => platformBattleController = value; }
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
        private readonly IMonsterSpawner monsterSpawner;
        private PlatformBattleController platformBattleController;

        private PlayerModel playerModel;
        private ShopModel shopModel;

        

        private PlayerMovementController playerMovementController;

        private bool isInitialized;

        private Context context;
        

        public PlatformBattleMini(IPlayerView playerView, PlayerInput playerInput, IPlayerAnimation playerAnimation, CameraFollow cameraFollow, CoinView coinView, CoinCollector coinCollector, IShopView shopView, CoinRotation coinRotation, ICoinFall coinFall, IShopZoneCollector shopZoneCollector, SpawnPointer spawnPointer, ICoinAmountUI coinAmountUI, IMonsterSpawner monsterSpawner)
        {
            this.playerView = playerView;
            this.playerInput = playerInput;
            this.playerAnimation = playerAnimation;
            this.cameraFollow = cameraFollow;
            this.coinView = coinView;
            this.coinCollector = coinCollector;
            this.shopView = shopView;
            this.coinRotation = coinRotation;
            this.coinFall = coinFall;
            this.shopZoneCollector = shopZoneCollector;
            this.spawnPointer = spawnPointer;
            this.coinAmountUI = coinAmountUI;
            this.monsterSpawner = monsterSpawner;
        }

        public void Initialize()
        {
            if (!isInitialized){
                isInitialized = true;
                               
                context = new Context ();
                
                
                playerModel = new();
                shopModel = new();
                platformBattleController = new (coinView, coinCollector, playerModel, shopZoneCollector, shopModel,shopView,spawnPointer);
                playerMovementController = new (playerInput, playerView, playerModel);
                
                playerModel.Initialize (context);
                monsterSpawner.Initialize(context);
                coinCollector.Initialize(context);
                coinView.Initialize(context);
                cameraFollow.Initialize(context);
                playerView.Initialize (context);
                playerAnimation.Initialize(context);
                platformBattleController.Initialize(context);
                playerMovementController.Initialize (context);
             
                shopView.Initialize(context);
                coinFall?.Initialize(context);
                coinRotation?.Initialize(context);
                shopZoneCollector.Initialize(context);
                spawnPointer.Initialize(context);
                coinAmountUI.Initialize(context);

             
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
