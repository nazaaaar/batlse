using nazaaaar.platformBattle.mini.model;
using RMC.Mini.Controller.Commands;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.controller.commands
{
    public struct MonsterDespawnRequestCommand: ICommand
    {
        public GameObject monster;
        public MonsterSO monsterSO;
    }
}