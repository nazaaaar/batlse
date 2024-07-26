

using System;
using nazaaaar.platform.battle.mini.viewAbstract;
using nazaaaar.platformBattle.mini.controller.commands;
using RMC.Mini;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class ShopView : MonoBehaviour, IShopView
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                context.CommandManager.AddCommandListener<ShopZoneEnteredCommand>(OnShopZoneEntered);
                context.CommandManager.AddCommandListener<ShopZoneExitedCommand>(OnShopZoneExited);
            }
        }

        private void OnShopZoneExited(ShopZoneExitedCommand e)
        {
            gameObject.SetActive(false);
        }

        private void OnShopZoneEntered(ShopZoneEnteredCommand e)
        {
            gameObject.SetActive(true);
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }
    }
}