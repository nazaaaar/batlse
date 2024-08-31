using RMC.Mini.Controller.Commands;

namespace nazaaaar.slime.mini.controller.commands
{
    public struct MonsterHealthChangedCommand: ICommand
    {
        public int amount;
    }
}