using nazaaaar.platformBattle.mini.model;
using RMC.Mini.Controller.Commands;

namespace nazaaaar.platformBattle.mini.controller.commands
{
    public struct MoneyAddRequestCommand: ICommand
    {
        public int amount;
        public Team team;
    }
}