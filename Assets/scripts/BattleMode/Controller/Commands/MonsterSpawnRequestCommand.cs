
using nazaaaar.platformBattle.mini.model;
using RMC.Mini.Controller.Commands;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.controller.commands{
    public class MonsterSpawnRequestCommand: ICommand
    {
        public MonsterSO MonsterSO;
        public Vector3 Position;

        public Team team;
    }
}