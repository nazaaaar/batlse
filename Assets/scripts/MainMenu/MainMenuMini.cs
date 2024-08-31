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
        private readonly AdsInitializer adsInitializer;
        private readonly AdsManager adsManager;
        private readonly IRewardedAdsButton rewardedAdsButton;
        private PlayerModel playerModel;
        private MainMenuController mainMenuController;
        private AdsController adsController;


        public MainMenuMini(ICameraView cameraView, IMenuButtonsView menuButtonsView, IPageSwitcher pageSwitcher, LobbyManager lobbyManager, RelayManager relayManager, AdsInitializer adsInitializer, AdsManager adsManager, IRewardedAdsButton rewardedAdsButton)
        {
            this.cameraView = cameraView;
            this.menuButtonsView = menuButtonsView;
            this.pageSwitcher = pageSwitcher;
            this.lobbyManager = lobbyManager;
            this.relayManager = relayManager;
            this.adsInitializer = adsInitializer;
            this.adsManager = adsManager;
            this.rewardedAdsButton = rewardedAdsButton;
        }

        public void Initialize()
        {
            if (!isInitialized){
                isInitialized = true;
                               
                context = new Context ();

                playerModel = new();
                
                mainMenuController = new(playerModel, cameraView, menuButtonsView, lobbyManager, relayManager);
                adsController = new(rewardedAdsButton,adsManager,adsInitializer);

                mainMenuController.Initialize(context);
                adsController.Initialize(context);
                rewardedAdsButton.Initialize(context);
                adsManager.Initialize(context);
                adsInitializer.Initialize(context);
                
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
