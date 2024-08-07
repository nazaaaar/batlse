using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System.Threading.Tasks;
using RMC.Mini.Service;
using IService = RMC.Mini.Service.IService;
using RMC.Mini;
using System;
namespace nazaaaar.platformBattle.MainMenu.service{
    public class LobbyManager : MonoBehaviour, IService
    {
        private Lobby currentLobby;
        private ILobbyEvents lobbyEvents;
        private const int MaxPlayers = 2;
        private const int HeartbeatInterval = 15;
        private const string JOIN_CODE = "JoinCode";

        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public event Action OnLobbyFull;

        public event Action<string> OnLobbyCodeConfigured;

        async void Start()
        {
            var options = new InitializationOptions();
            var profile = Guid.NewGuid().ToString().Substring(0, 8);
            options.SetProfile(profile);

            await UnityServices.InitializeAsync(options);
            await SignInAnonymouslyAsync();
        }

        private async Task SignInAnonymouslyAsync()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        public void QuickJoinLobby(){
            RequireIsInitialized();

            QuickJoinLobbyAsync();
        }
        private async Task QuickJoinLobbyAsync()
        {
            try
            {
                var joinResponse = await Lobbies.Instance.QuickJoinLobbyAsync();
                currentLobby = joinResponse;
                Debug.Log("Joined Lobby: " + currentLobby.Id);
                var callbacks = new LobbyEventCallbacks();
                callbacks.LobbyChanged += OnLobbyChangedClient;

                try {
                    lobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(currentLobby.Id, callbacks);
                }
                catch (LobbyServiceException ex)
                {
                    switch (ex.Reason) {
                        case LobbyExceptionReason.AlreadySubscribedToLobby: Debug.LogWarning($"Already subscribed to lobby[{currentLobby.Id}]. We did not need to try and subscribe again. Exception Message: {ex.Message}"); break;
                        case LobbyExceptionReason.SubscriptionToLobbyLostWhileBusy: Debug.LogError($"Subscription to lobby events was lost while it was busy trying to subscribe. Exception Message: {ex.Message}"); throw;
                        case LobbyExceptionReason.LobbyEventServiceConnectionError: Debug.LogError($"Failed to connect to lobby events. Exception Message: {ex.Message}"); throw;
                        default: throw;
                    }
                }
            }
            catch (LobbyServiceException)
            {
                CreateLobby();
            }
        }

        private void OnLobbyChangedClient(ILobbyChanges changes)
        {
            if(!changes.LobbyDeleted){
                changes.ApplyToLobby(currentLobby);

                if (currentLobby.Data.ContainsKey(JOIN_CODE) && currentLobby.Data[JOIN_CODE]!= null){
                    OnLobbyCodeConfigured?.Invoke(currentLobby.Data[JOIN_CODE].Value);
                }
            }
        }

        private async void CreateLobby()
        {
            try
            {
                var lobbyOptions = new CreateLobbyOptions
                {
                    IsPrivate = false
                };
    
                var createResponse = await Lobbies.Instance.CreateLobbyAsync("LobbyName", MaxPlayers, lobbyOptions);
                currentLobby = createResponse;
                Debug.Log("Created Lobby: " + currentLobby.Id);
                InvokeRepeating("SendHeartbeat", 0, HeartbeatInterval);

                var callbacks = new LobbyEventCallbacks();
                callbacks.LobbyChanged += OnLobbyChanged;

                try {
                    lobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(currentLobby.Id, callbacks);
                }
                catch (LobbyServiceException ex)
                {
                    switch (ex.Reason) {
                        case LobbyExceptionReason.AlreadySubscribedToLobby: Debug.LogWarning($"Already subscribed to lobby[{currentLobby.Id}]. We did not need to try and subscribe again. Exception Message: {ex.Message}"); break;
                        case LobbyExceptionReason.SubscriptionToLobbyLostWhileBusy: Debug.LogError($"Subscription to lobby events was lost while it was busy trying to subscribe. Exception Message: {ex.Message}"); throw;
                       case LobbyExceptionReason.LobbyEventServiceConnectionError: Debug.LogError($"Failed to connect to lobby events. Exception Message: {ex.Message}"); throw;
                        default: throw;
                    }
                }
                CheckForRelay();
            }
            catch (LobbyServiceException e)
            {
                
                throw e;
            }
        }

        private void OnLobbyChanged(ILobbyChanges changes)
        {
            if (changes.PlayerJoined.Changed){
                changes.ApplyToLobby(currentLobby);
                CheckForRelay();
            }
        }

        private async void SendHeartbeat()
        {
            
            if (currentLobby != null)
            {
                await Lobbies.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            }
        }

        private void CheckForRelay()
        {
            if (currentLobby.Players.Count == currentLobby.MaxPlayers)
            {
                OnLobbyFull?.Invoke();
            }
        }

        public bool IsHost => currentLobby.Players[0].Id == AuthenticationService.Instance.PlayerId;

        public void Initialize(IContext context)
        {
            if (!IsInitialized) {
                IsInitialized = true;
                Context = context;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }

        public void SetLobbyJoinCode(string joinCode)
        {
            RequireIsInitialized();

            SetLobbyJoinCodeAsync(joinCode);
        }

        private async void SetLobbyJoinCodeAsync(string joinCode)
        {
            try{
                Debug.Log("AsyncSet");
                currentLobby = await Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions{
                Data = new System.Collections.Generic.Dictionary<string, DataObject>{
                    {JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, joinCode)}
                }
            });}
            catch (LobbyServiceException ex){
                Debug.Log(ex);
            }


        }
    }
}