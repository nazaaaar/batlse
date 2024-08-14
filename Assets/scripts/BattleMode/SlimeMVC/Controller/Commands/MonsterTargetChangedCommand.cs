using nazaaaar.platformBattle.mini.model;
using nazaaaar.slime.mini.model;
using RMC.Mini.Controller.Commands;

namespace nazaaaar.slime.mini.controller.commands
{
    public struct MonsterTargetCommand: ICommand
    {
        public IMonster target;
    }
}