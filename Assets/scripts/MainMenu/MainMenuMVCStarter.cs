using nazaaaar.platformBattle.MainMenu.view;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using UnityEngine;

namespace nazaaaar.platformBattle.MainMenu.mini
{
    class MainMenuMVCStarter: MonoBehaviour
    {
        public CameraView cameraView;
        public MenuButtonsView menuButtonsView;
        public PageSwitcher pageSwitcher;

        void Awake(){

            MainMenuMini platformBattleMini =
            new (cameraView, menuButtonsView, pageSwitcher);

            platformBattleMini.Initialize();
        }
    }
}