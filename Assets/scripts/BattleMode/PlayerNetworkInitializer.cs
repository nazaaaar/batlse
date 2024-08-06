using nazaaaar.platformBattle.mini;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.platformBattle.mini.model;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

class PlayerNetworkInitializer: NetworkBehaviour
{
    [SerializeField]
    private PlayerCharacterControllerConfig playerCharacterControllerConfig;
    public override void OnNetworkSpawn(){
        if (!IsOwner) return;
        gameObject.AddComponent<ShopZoneCollector>();
        gameObject.AddComponent<CoinCollector>();
        var characterController = gameObject.AddComponent<CharacterController>();
        characterController.center = playerCharacterControllerConfig.center;
        characterController.radius = playerCharacterControllerConfig.radius;
        characterController.height = playerCharacterControllerConfig.height;
        FindAnyObjectByType<PlarformBattleStarterNetCode>().StartBattle(transform, IsHost?Team.Red:Team.Blue);
    }
}
