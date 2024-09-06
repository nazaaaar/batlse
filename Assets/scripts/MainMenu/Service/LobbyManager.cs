using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System.Threading.Tasks;
using IService = RMC.Mini.Service.IService;
using RMC.Mini;
using System;
using System.Collections.Generic;
namespace nazaaaar.platformBattle.MainMenu.service
{
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
        public event Action OnLobbyCreated;
        public event Action OnLobbyDeleted;

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
            try{
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            }
            catch (RequestFailedException e){
                if (e.Message.Contains("Cannot resolve destination host")){
                    Debug.Log("No Internet");//TODO handle properly
                }
            }
        }

        private async void OnLobbyEventConnectionStateChanged(LobbyEventConnectionState state)
        {
            switch (state)
            {
                case LobbyEventConnectionState.Unsubscribed: {await DeleteLobby();} /* Update the UI if necessary, as the subscription has been stopped. */ break;
                case LobbyEventConnectionState.Subscribing: /* Update the UI if necessary, while waiting to be subscribed. */ break;
                case LobbyEventConnectionState.Subscribed: /* Update the UI if necessary, to show subscription is working. */ break;
                case LobbyEventConnectionState.Unsynced:  /* Update the UI to show connection problems. Lobby will attempt to reconnect automatically. */ break;
                case LobbyEventConnectionState.Error: /* Update the UI to show the connection has errored. Lobby will not attempt to reconnect as something has gone wrong. */
                break;
            }

            Debug.Log(state.ToString());
        }

        public void QuickJoinLobby(){
            RequireIsInitialized();

            _ = QuickJoinLobbyAsync();
        }
        private async Task QuickJoinLobbyAsync()
        {
            try
            {
                var joinResponse = await Task.Run(()=>Lobbies.Instance.QuickJoinLobbyAsync());
                currentLobby = joinResponse;
                if (joinResponse.Players.Count == 1){
                    await DeleteLobby();
                    return;
                }
                Debug.Log("Joined Lobby: " + currentLobby.Id);
                Debug.Log(currentLobby.Players.Count);
                var callbacks = new LobbyEventCallbacks();
                callbacks.LobbyChanged += OnLobbyChangedClientAsync;
                
                

                try {
                    lobbyEvents = await Task.Run(()=>Lobbies.Instance.SubscribeToLobbyEventsAsync(currentLobby.Id, callbacks));
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
                MarkAsConnected();
                Debug.Log("Subsctibed");
            }
            catch (LobbyServiceException)
            {
                if (currentLobby == null)
                CreateLobby();
            }
        }

        private async void OnLobbyChangedClientAsync(ILobbyChanges changes)
        {
            Debug.Log("change: is lobby deleted?");
            Debug.Log(currentLobby.Players.Count);
            if (!changes.LobbyDeleted){
                changes.ApplyToLobby(currentLobby);
                if (changes.PlayerLeft.Added == true){
                    await DeleteLobby();
                }
                if (currentLobby?.Data != null) 
                if (currentLobby.Data.ContainsKey(JOIN_CODE)) 
                if (currentLobby.Data[JOIN_CODE] != null) 
                if (currentLobby.Data[JOIN_CODE].Value != null){
                    OnLobbyCodeConfigured?.Invoke(currentLobby.Data[JOIN_CODE].Value);
                }
            }
            else{
                currentLobby=null;
                OnLobbyDeleted?.Invoke();
                
            }
        }

        private async Task DeleteLobby()
        {
            try
            {
                await Task.Run(() => LobbyService.Instance.DeleteLobbyAsync(currentLobby.Id));
                currentLobby=null;
                OnLobbyDeleted?.Invoke();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
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
    
                var createResponse = await Task.Run(()=>Lobbies.Instance.CreateLobbyAsync("LobbyName", MaxPlayers, lobbyOptions));
                currentLobby = createResponse;
                Debug.Log("Created Lobby: " + currentLobby.Id);
                
                InvokeRepeating("SendHeartbeat", 0, HeartbeatInterval);

                var callbacks = new LobbyEventCallbacks();
                callbacks.LobbyChanged += OnLobbyChanged;
                callbacks.LobbyEventConnectionStateChanged+= OnLobbyEventConnectionStateChanged;

                try {
                    lobbyEvents = await Task.Run(()=>Lobbies.Instance.SubscribeToLobbyEventsAsync(currentLobby.Id, callbacks));
                    MarkAsConnected();
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
                OnLobbyCreated?.Invoke();
                
            }
            catch (LobbyServiceException e)
            {
                throw e;
            }
        }

        private async void OnLobbyChanged(ILobbyChanges changes)
        {
        
            if (!changes.LobbyDeleted){
                changes.ApplyToLobby(currentLobby);

                if (changes.PlayerLeft.Changed)
                {
                    if (currentLobby.Players.Count < 2)
                    await DeleteLobby();
                }    

                if (changes.PlayerData.Changed){
                    CheckForRelay();}
            
            }
            
        }

        private async void SendHeartbeat()
        {
            
            if (currentLobby != null)
            {
                await Task.Run(() => Lobbies.Instance.SendHeartbeatPingAsync(currentLobby.Id));
            }
        }

        private void CheckForRelay()
        {
            if (currentLobby.Players.Count == currentLobby.MaxPlayers)
            {
                foreach (var player in currentLobby.Players)
                {                            
                    if (player == null) continue;

                    if (player.Data == null) continue;

                    if (!player.Data.ContainsKey("Connected") || (player.Data["Connected"]?.Value) != "true")
                    {
                        break;
                    }
                }
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
                currentLobby = await Task.Run(()=>Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions{
                Data = new Dictionary<string, DataObject>{
                    {JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, joinCode)}
                }
                
            }));
                Debug.Log(JOIN_CODE + " " + joinCode );
            }
            catch (LobbyServiceException ex){
                Debug.Log(ex);
            }


        }

        private async void MarkAsConnected(){
        try
        {
            UpdatePlayerOptions options = new UpdatePlayerOptions();

            options.Data = new Dictionary<string, PlayerDataObject>()
            {
                {
                    "Connected", new PlayerDataObject(
                        visibility: PlayerDataObject.VisibilityOptions.Member,
                        value: "true")
                }
            };

            string playerId = AuthenticationService.Instance.PlayerId;

            await Task.Run(() => LobbyService.Instance.UpdatePlayerAsync(currentLobby.Id, playerId, options));
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    }

    
}