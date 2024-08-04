using nazaaaar.platformBattle.MainMenu.model;
using RMC.Mini.Controller.Commands;

namespace nazaaaar.platformBattle.MainMenu.controller.commands{
    public class CurrentPageChangedCommand: ICommand
    {
        public CurrentPage CurrentPage { get; set; }
    }
}