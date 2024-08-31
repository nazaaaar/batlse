using nazaaaar.platformBattle.mini.model;
using RMC.Mini.Controller.Commands;

namespace nazaaaar.slime.mini.controller.commands
{
    public struct MonsterVictoryCommand: ICommand
    {
        public IMonster monster;
    }
}