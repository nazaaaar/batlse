using RMC.Mini;
using System;
using UnityEngine;
using nazaaaar.platformBattle.mini.viewAbstract;

namespace nazaaaar.platformBattle.mini.view
{
    public class CoinCollector : MonoBehaviour, ICoinCollector
    {
        public event Action OnCoinCollected;
        void OnControllerColliderHit(ControllerColliderHit hit){
            if (hit.gameObject.tag == "Coin")
            {
                OnCoinCollected?.Invoke();
                Destroy(hit.gameObject);
            }
        }
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                this.Context = context;
            
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)throw new Exception("MustBeInitialized");
        }
    }
}
