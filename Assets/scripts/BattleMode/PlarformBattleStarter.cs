using nazaaaar.platformBattle.mini.view;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nazaaaar.platformBattle.mini
{
    class PlarformBattleStarter: MonoBehaviour
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
        private MonsterSpawner monsterSpawner;

        void Awake(){

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
                monsterSpawner
                );

            platformBattleMini.Initialize();
        }
    }
}