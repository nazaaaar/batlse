using System;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class ShopZoneCollector : MonoBehaviour, IShopZoneCollector
    {
        public event Action OnShopZoneEntered;
        public event Action OnShopZoneExited;
        void OnTriggerEnter(Collider other){
            
            if (other.gameObject.tag == "ShopZone")
            {
                OnShopZoneEntered?.Invoke();
            }
        }
        void OnTriggerExit(Collider other){
            if (other.gameObject.tag == "ShopZone")
            {
                OnShopZoneExited?.Invoke();
            }
        }
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){throw new System.Exception("MustBeInitialized");}
        }
    }
}