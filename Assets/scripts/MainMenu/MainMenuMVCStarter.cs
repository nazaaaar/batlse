using nazaaaar.platformBattle.MainMenu.service;
using nazaaaar.platformBattle.MainMenu.view;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using Unity.Services.Lobbies;
using UnityEngine;

namespace nazaaaar.platformBattle.MainMenu.mini
{
    class MainMenuMVCStarter: MonoBehaviour
    {
        public CameraView cameraView;
        public MenuButtonsView menuButtonsView;
        public PageSwitcher pageSwitcher;
        public LobbyManager lobbyManager;
        public RelayManager relayManager;

        void Awake(){

            MainMenuMini platformBattleMini =
            new (cameraView, menuButtonsView, pageSwitcher, lobbyManager, relayManager);

            platformBattleMini.Initialize();
        }
    }
}