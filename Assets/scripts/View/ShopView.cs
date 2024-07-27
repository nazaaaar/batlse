

using System;
using nazaaaar.platformBattle.mini.viewAbstract;
using nazaaaar.platformBattle.mini.controller.commands;
using RMC.Mini;
using UnityEngine;
using nazaaaar.platformBattle.mini.model;

namespace nazaaaar.platformBattle.mini.view
{
    public class ShopView : MonoBehaviour, IShopView
    {
        public bool IsInitialized {get; private set;}

        [SerializeField]
        private ShopCardSO[] shopCardSOs;

        [SerializeField]
        private ShopCardView[] shopCards;

        public IContext Context {get; private set;}
        public event Action<ShopCardSO[]> OnAllShopCardsSoChanged;

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                context.CommandManager.AddCommandListener<ShopZoneEnteredCommand>(OnShopZoneEntered);
                context.CommandManager.AddCommandListener<ShopZoneExitedCommand>(OnShopZoneExited);

                context.CommandManager.AddCommandListener<ActiveShopCardsChangedCommand>(OnShopCardsChanged);
                context.CommandManager.AddCommandListener<CouldBeBoughtShopCardsChangedCommand>(OnCouldBeBoughtChanged);

                OnAllShopCardsSoChanged?.Invoke(shopCardSOs);
            }
        }

        private void OnCouldBeBoughtChanged(CouldBeBoughtShopCardsChangedCommand e)
        {
            for (int i = 0; i < shopCards.Length; i++)
            {
                shopCards[i].Interactable = e.CouldBeBought[i];
            }
        }

        private void OnShopCardsChanged(ActiveShopCardsChangedCommand e)
        {
            for (int i = 0; i < shopCards.Length; i++)
            {
                shopCards[i].ConfigWithShopCard(e.ShopCards[i]);
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