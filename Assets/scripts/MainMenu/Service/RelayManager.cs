using System.Threading.Tasks;
using RMC.Mini;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using IService = RMC.Mini.Service.IService;

namespace nazaaaar.platformBattle.MainMenu.service
{
    public class RelayManager : MonoBehaviour, IService
    {
        public bool IsInitialized{ get; private set; }

        public IContext Context{ get; private set; }


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

        public async void ConfigClientWithRelay(string joinCode){
            RequireIsInitialized();

            await ConfigClientWithRelayAsync(joinCode);
        }
        public async Task<string> ConfigHostWithRelayAsync(int maxConnections=1)
        {
            try{await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return joinCode;}
            catch(RelayServiceException ex){
                Debug.Log(ex);
                return null;
            }
        }

        public async Task ConfigClientWithRelayAsync(string joinCode)
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        }
    }
}