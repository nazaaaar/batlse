using System;
using nazaaaar.platformBattle.mini.controller.commands;
using RMC.Mini;
using RMC.Mini.Model;
using Unity.Netcode;

namespace nazaaaar.platformBattle.mini.model
{
    public class NetworkCoinsModel : NetworkBehaviour, IModel
    {
        public bool IsInitialized{get; private set;}

        public IContext Context{get; private set;}

        private NetworkVariable<int> BlueCoins { get; } = new();
        private NetworkVariable<int> RedCoins { get; } = new();

        public Func<NetworkVariable<int>> MyCoins;
        public Func<NetworkVariable<int>> OtherCoins;
        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                if (IsServer){
                    BlueCoins.Value = 0;
                    RedCoins.Value = 0;
                }

                Context.CommandManager.AddCommandListener<TeamChangedCommand>(OnTeamChanged);
            }
        }

        private void OnTeamChanged(TeamChangedCommand e)
        {
            UnityEngine.Debug.Log("TeamChanged On " + e.Team);
            if (e.Team.Equals(Team.Red)){
                MyCoins = ()=> RedCoins;
                OtherCoins = () => BlueCoins;
            }
            else if (e.Team.Equals(Team.Blue)){
                MyCoins = () => BlueCoins;
                OtherCoins = () => RedCoins;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){
                throw new System.Exception("MustBeInitialized");
            }
        }

        [Rpc(SendTo.Server)]
        public void AddCoinsRpc(int amount, Team team){
            if (team == Team.Red){
                RedCoins.Value += amount;
            }
            else if (team == Team.Blue){
                BlueCoins.Value += amount;
            }
        }
    }
}