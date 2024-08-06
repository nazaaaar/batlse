using RMC.Mini;
using RMC.Mini.Controller;
using nazaaaar.platformBattle.MainMenu.model;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using System;
using nazaaaar.platformBattle.MainMenu.controller.commands;
using nazaaaar.platformBattle.MainMenu.service;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine;

namespace nazaaaar.platformBattle.MainMenu.controller{
    public class MainMenuController : IController
    {
        public bool IsInitialized {get; private set; }
        public IContext Context {get; private set;}
        public PlayerModel PlayerModel { get; }
        public ICameraView CameraView { get; }
        public IMenuButtonsView MenuButtonsView { get; }
        public LobbyManager LobbyManager { get; }
        public RelayManager RelayManager { get; }
        public CurrentPageChangedCommand CurrentPageChangedCommand { get; } = new CurrentPageChangedCommand();

        public MainMenuController(PlayerModel playerModel, ICameraView cameraView, IMenuButtonsView menuButtonsView, LobbyManager lobbyManager, RelayManager relayManager)
        {
            PlayerModel = playerModel;
            CameraView = cameraView;
            MenuButtonsView = menuButtonsView;
            LobbyManager = lobbyManager;
            RelayManager = relayManager;
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
                LobbyManager.OnLobbyFull += Service_OnLobbyFull;
                LobbyManager.OnLobbyCodeConfigured += Service_OnLobbyCodeConfigured;
                
            }

        }

        private async void Service_OnLobbyCodeConfigured(string lobbyCode)
        {
            PlayerPrefs.SetInt("isHost", 0);
            await RelayManager.ConfigClientWithRelayAsync(lobbyCode);

            SceneManager.LoadScene(1);
        }

        private async void Service_OnLobbyFull()
        {
            Debug.Log("Lobby is full");
            string res = await RelayManager.ConfigHostWithRelayAsync();
            
            
            LobbyManager.SetLobbyJoinCode(res);
            PlayerPrefs.SetInt("isHost", 1);
            SceneManager.LoadScene(1);
        }

        private void View_OnBackPressed()
        {
            PlayerModel.CurrentPage.Value = CurrentPage.StartPage;
        }

        private void View_OnSettingsPressed()
        {
            PlayerModel.CurrentPage.Value = CurrentPage.Settings;
        }

        private void View_OnStartPressed()
        {
            PlayerModel.CurrentPage.Value = CurrentPage.FindGame;
        }

        private void PlayerModel_OnCurrentPageChanged(CurrentPage oldValue, CurrentPage newValue)
        {
            CurrentPageChangedCommand.CurrentPage = newValue;
            Context.CommandManager.InvokeCommand(CurrentPageChangedCommand);

            if (newValue == CurrentPage.FindGame)
            {
                LobbyManager.QuickJoinLobby();
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){throw new System.Exception("MustBeInitialized");}

        }
    }
}