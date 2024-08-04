using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using RMC.Mini;
using RMC.Mini.Service;
using Unity.Netcode;
using UnityEngine;

namespace nazaaaar.platformBattle.MainMenu.service{
    public class ConnectionService : IService
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        private int port = 7777;

        public event Action OnClientConnected;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){
                throw new System.Exception("MustBeInitialized");
            }
        }

        public async Task CheckServerAsync()
        {
            RequireIsInitialized();
            bool serverExists = await IsServerRunning(port);
            if (serverExists)
            {
                // Start as client

                Debug.Log("Started as client");
                NetworkManager.Singleton.StartClient();
            }
            else
            {
                // Start as host
                Debug.Log("Started as host");
                NetworkManager.Singleton.StartHost();
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedF;
            }
        }

        private async Task<bool> IsServerRunning(int port)
        {
            bool isAvailable = true;

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(IPAddress.Loopback, port);
                    isAvailable = client.Connected;
                }
            }
            catch (SocketException)
            {
                isAvailable = false;
            }

            return isAvailable;
        }

        private void OnClientConnectedF(ulong clientId)
            {
                if (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClients.Count == 2)
                {
                    OnClientConnected?.Invoke();               
                }
            }

        public void Disconnect()
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedF;
            // Shutdown the NetworkManager if it is running
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.Shutdown();
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }
    }
}