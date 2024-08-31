using RMC.Mini.Controller.Commands;

namespace nazaaaar.platformBattle.mini.controller.commands
{
    public struct GameFinishedCommand: ICommand
    {
        public GameEndState gameEndState;

        public enum GameEndState
        {
            Win,
            Lose,
            Draw
        }
    }

    
}