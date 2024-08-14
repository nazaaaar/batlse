using nazaaaar.platformBattle.mini.model;
using nazaaaar.slime.mini.model;
using RMC.Mini.Controller.Commands;

namespace nazaaaar.slime.mini.controller.commands
{
    public struct MonsterAttackRangeCommand: ICommand
    {
        public float attackRange;
    }
}