using RMC.Mini;
using RMC.Mini.Controller;
using nazaaaar.platformBattle.MainMenu.model;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using System;
using nazaaaar.platformBattle.MainMenu.controller.commands;
using nazaaaar.platformBattle.MainMenu.service;

namespace nazaaaar.platformBattle.MainMenu.controller{
    public class MainMenuController : IController
    {
        public bool IsInitialized {get; private set; }
        public IContext Context {get; private set;}
        public PlayerModel PlayerModel { get; }
        public ICameraView CameraView { get; }
        public IMenuButtonsView MenuButtonsView { get; }
        public ConnectionService ConnectionService { get; }
        public CurrentPageChangedCommand CurrentPageChangedCommand { get; } = new CurrentPageChangedCommand();

        public MainMenuController(PlayerModel playerModel, ICameraView cameraView, IMenuButtonsView menuButtonsView, ConnectionService connectionService)
        {
            PlayerModel = playerModel;
            CameraView = cameraView;
            MenuButtonsView = menuButtonsView;
            ConnectionService = connectionService;
        }

        public void Dispose()
        {

        }

        public void Initialize(IContext context)
        {   
            if (!IsInitialized){
                IsInitialized = true;

                Context = context;

                PlayerModel.CurrentPage.OnValueChanged.AddListener(PlayerModel_OnCurrentPageChanged);
                MenuButtonsView.OnStartPressed += View_OnStartPressed;
                MenuButtonsView.OnSettingsPressed += View_OnSettingsPressed;
                MenuButtonsView.OnBackPressed += View_OnBackPressed;
                
            }

            ConnectionService.OnClientConnected += Service_OnClientConnected;
        }

        private void Service_OnClientConnected()
        {
            throw new Exception("WORKS!!");
        }

        private void View_OnBackPressed()
        {
            PlayerModel.CurrentPage.Value = CurrentPage.StartPage;
            ConnectionService.Disconnect();
        }

        private void View_OnSettingsPressed()
        {
            PlayerModel.CurrentPage.Value = CurrentPage.Settings;
        }

        private void View_OnStartPressed()
        {
            PlayerModel.CurrentPage.Value = CurrentPage.FindGame;
            ConnectionService.CheckServerAsync();
        }

        private void PlayerModel_OnCurrentPageChanged(CurrentPage oldValue, CurrentPage newValue)
        {
            CurrentPageChangedCommand.CurrentPage = newValue;
            Context.CommandManager.InvokeCommand(CurrentPageChangedCommand);
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){throw new System.Exception("MustBeInitialized");}

        }
    }
}