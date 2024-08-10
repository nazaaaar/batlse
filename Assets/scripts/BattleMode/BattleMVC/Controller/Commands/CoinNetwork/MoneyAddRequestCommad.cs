
using nazaaaar.platformBattle.mini.model;
using RMC.Mini.Controller.Commands;
using Unity.Netcode;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.controller.commands
{
    public class MoneyAddRequestCommand: ICommand
    {
        public int amount;
        public Team team;
    }
}