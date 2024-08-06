using nazaaaar.platformBattle.MainMenu.controller;
using nazaaaar.platformBattle.MainMenu.model;
using nazaaaar.platformBattle.MainMenu.service;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using RMC.Mini;
using System;
namespace nazaaaar.platformBattle.MainMenu.mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: A mini MVCS for core gameplay logic
    /// </summary>
    public class MainMenuMini: ISimpleMiniMvcs
    {

        public bool IsInitialized => isInitialized;
        private bool isInitialized;

        private Context context;
        private readonly ICameraView cameraView;
        private readonly IMenuButtonsView menuButtonsView;
        private readonly IPageSwitcher pageSwitcher;
        private readonly LobbyManager lobbyManager;
        private readonly RelayManager relayManager;
        private PlayerModel playerModel;
        private MainMenuController mainMenuController;


        public MainMenuMini(ICameraView cameraView, IMenuButtonsView menuButtonsView, IPageSwitcher pageSwitcher, LobbyManager lobbyManager, RelayManager relayManager)
        {
            this.cameraView = cameraView;
            this.menuButtonsView = menuButtonsView;
            this.pageSwitcher = pageSwitcher;
            this.lobbyManager = lobbyManager;
            this.relayManager = relayManager;
        }

        public void Initialize()
        {
            if (!isInitialized){
                isInitialized = true;
                               
                context = new Context ();

                playerModel = new();
                
                mainMenuController = new(playerModel, cameraView, menuButtonsView, lobbyManager, relayManager);

                mainMenuController.Initialize(context);
                lobbyManager.Initialize(context);
                relayManager.Initialize(context);
                cameraView.Initialize(context);
                pageSwitcher.Initialize(context);
                menuButtonsView.Initialize(context);
                playerModel.Initialize(context);
                
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
