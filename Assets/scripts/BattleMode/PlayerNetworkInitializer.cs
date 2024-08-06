using nazaaaar.platformBattle.mini;
using Unity.Netcode;
using UnityEngine;

class PlayerNetworkInitializer: NetworkBehaviour
{
    public override void OnNetworkSpawn(){
        if (!IsOwner) return;
        FindAnyObjectByType<PlarformBattleStarterNetCode>().StartBattle(transform);
    }
}