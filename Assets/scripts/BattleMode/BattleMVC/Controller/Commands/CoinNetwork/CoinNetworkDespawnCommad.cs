
using RMC.Mini.Controller.Commands;
using Unity.Netcode;

namespace nazaaaar.platformBattle.mini.controller.commands
{
    public class CoinNetworkDespawnCommand: ICommand
    {
        public NetworkObject coin;
    }
}