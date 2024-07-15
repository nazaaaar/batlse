using RMC.Mini.Controller.Commands;

namespace nazaaaar.platformBattle.mini.controller.commands{
    public class MoveSpeedChangedCommand: ICommand
    {
        public float MoveSpeed { get; set; }
    }
}