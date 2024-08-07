using nazaaaar.platformBattle.mini;
using nazaaaar.platformBattle.mini.view;
using nazaaaar.platformBattle.mini.model;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

class PlayerNetworkInitializer: NetworkBehaviour
{
    [SerializeField]
    private PlayerCharacterControllerConfig playerCharacterControllerConfig;
    public void Awake(){
        NetworkManager.SceneManager.OnLoadEventCompleted += PlayerNetworkManager_SceneManager_OnLoadEvent;
    }

    private void PlayerNetworkManager_SceneManager_OnLoadEvent(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (!IsOwner) return;
        if (clientsCompleted.Count!=2){throw new Exception("Client not loaded");}
        if (sceneName == "PlayMode"){
            OnPlayModeStart();
        }
    }

    public void OnPlayModeStart(){
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
