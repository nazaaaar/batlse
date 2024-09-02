using RMC.Mini.View;

namespace nazaaaar.platformBattle.mini.viewAbstract
{
    public interface IGameEndButton: IView
    {
        event System.Action OnMainMenuClicked;
    }
}
