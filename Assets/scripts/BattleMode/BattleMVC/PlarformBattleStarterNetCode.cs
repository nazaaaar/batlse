using System;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nazaaaar.platformBattle.mini
{
    class PlarformBattleStarterNetCode: MonoBehaviour
    {
        [SerializeField]
        private CameraFollow cameraFollow;
        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private PlayerView playerView;
        [SerializeField]
        private PlayerAnimation playerAnimation;
        
        [SerializeField]
        private CoinView coinView;

        [SerializeField]
        private CoinCollector coinCollector;
        [SerializeField] 
        private ShopView shopView;
        [SerializeField]
        private CoinRotation coinRotation;
        [SerializeField]
        private CoinFall coinFall;
        [SerializeField]
        private ShopZoneCollector shopZoneCollector;
        [SerializeField]
        private SpawnPointer pointer;
        [SerializeField]
        private CoinAmountUI coinAmountUI;
        [SerializeField]
        private OtherCoinAmountUI otherCoinAmountUI;
        [SerializeField]
        private MonsterSpawner monsterSpawner;

        [SerializeField]
        private LoadScreenView loadScreenView;
        [SerializeField]
        private CoinNetworkSpawner coinNetworkSpawner;
        [SerializeField]
        private NetworkCoinsModel networkCoinsModel;

        private IContext context;

        public void Awake(){
            context = new Context();
            loadScreenView.Initialize(context);            
        } 

        public void StartBattle(Transform playerPrefab, model.Team team)
        {

            
            playerView.CharacterController = playerPrefab.GetComponent<CharacterController>();
            playerAnimation.Animator = playerPrefab.GetComponent<Animator>();

            coinCollector = playerPrefab.GetComponent<CoinCollector>();
            shopZoneCollector = playerPrefab.GetComponent<ShopZoneCollector>();

            playerView.PlayerTransform = playerPrefab;
            pointer.Player = playerPrefab;
            cameraFollow.PlayerTransform = playerPrefab;

            PlatformBattleMini platformBattleMini =
            new (playerView,
                playerInput,
                playerAnimation,
                cameraFollow,
                coinView,
                coinCollector,
                shopView,
                coinRotation,
                coinFall,
                shopZoneCollector,
                pointer,
                coinAmountUI,
                otherCoinAmountUI,
                monsterSpawner,
                coinNetworkSpawner,
                networkCoinsModel,
                team,
                context
                );

            platformBattleMini.Initialize();
        }
    }
}