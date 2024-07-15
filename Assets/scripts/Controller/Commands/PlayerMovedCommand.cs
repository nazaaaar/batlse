using RMC.Mini.Controller.Commands;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.controller.commands
{
    public class PlayerMovedCommand : ICommand
    {
        public PlayerMovedCommand(Vector3 vector)
        {
            Vector = vector;
        }

        public Vector3 Vector { get; set;}
    }
}