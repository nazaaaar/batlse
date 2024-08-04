using nazaaaar.slime.mini.model;
using RMC.Mini.Controller.Commands;

namespace nazaaaar.slime.mini.controller.commands
{
    public class MonsterStateChangedCommand: ICommand
    {
        public MonterState monsterState;
    }
}