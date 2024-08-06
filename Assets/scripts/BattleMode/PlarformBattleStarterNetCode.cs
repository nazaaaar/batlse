using nazaaaar.platformBattle.mini.view;
using Unity.Netcode;
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
        private MonsterSpawner monsterSpawner;

        public void Awake(){
            if (PlayerPrefs.GetInt("isHost") == 1){
                Debug.Log("Starting host");
                NetworkManager.Singleton.StartHost();
            }
            else{
                Debug.Log("Starting client");
            
                NetworkManager.Singleton.StartClient();
            }            
        } 

        public void StartBattle(Transform playerPrefab){

            playerInput = playerPrefab.GetComponent<PlayerInput>();
            playerView = playerPrefab.GetComponent<PlayerView>();
            playerAnimation = playerPrefab.GetComponent<PlayerAnimation>();
            coinCollector = playerPrefab.GetComponent<CoinCollector>();
            shopZoneCollector = playerPrefab.GetComponent<ShopZoneCollector>();

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
                monsterSpawner
                );

            platformBattleMini.Initialize();
        }
    }
}