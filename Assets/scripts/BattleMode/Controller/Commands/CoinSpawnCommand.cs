
using RMC.Mini.Controller.Commands;
using Unity.Netcode;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.controller.commands
{
    public class CoinNetworkSpawnCommand: ICommand
    {
        public Vector3 coinPos;
    }
}