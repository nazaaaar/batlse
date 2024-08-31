
using System;
using RMC.Mini.View;

namespace nazaaaar.slime.mini.viewAbstract
{
    public interface ISlimeAnimation : IView
    {
        event Action OnSlimeDiedEndAnimation;

        void DestroySelf();
    }
}