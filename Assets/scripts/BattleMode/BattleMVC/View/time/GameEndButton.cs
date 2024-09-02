using RMC.Mini;
using System;
using UnityEngine;
using nazaaaar.platformBattle.mini.viewAbstract;

namespace nazaaaar.platformBattle.mini.view
{
    public class GameEndButton : MonoBehaviour, IGameEndButton
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public event Action OnMainMenuClicked;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                this.Context = context;
            }
        }

        public void OnMainMenuButtonClicked(){
            OnMainMenuClicked?.Invoke();
            Debug.Log("INVOKE MAIN MENU");
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)throw new Exception("MustBeInitialized");
        }
    }
}
