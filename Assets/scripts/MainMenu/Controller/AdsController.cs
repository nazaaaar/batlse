using RMC.Mini;
using RMC.Mini.Controller;
using nazaaaar.platformBattle.MainMenu.model;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using System;
using nazaaaar.platformBattle.MainMenu.controller.commands;
using nazaaaar.platformBattle.MainMenu.service;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;
using Unity.Netcode;


namespace nazaaaar.platformBattle.MainMenu.controller
{
    public class AdsController : IController
    {
        public bool IsInitialized {get; private set; }
        public IContext Context {get; private set;}
        private readonly AdsInitializer adsInitializer;
        private readonly AdsManager adsManager;
        private readonly IRewardedAdsButton adsButton;

        public AdsController(IRewardedAdsButton adsButton, AdsManager adsManager, AdsInitializer adsInitializer)
        {
            this.adsButton = adsButton;
            this.adsManager = adsManager;
            this.adsInitializer = adsInitializer;
        }

        public void Dispose()
        {

        }

        public void Initialize(IContext context)
        {   
            if (!IsInitialized){
                IsInitialized = true;

                Context = context;

                adsInitializer.OnAdsInitialized+=OnAdsInitialized;
                adsInitializer.InitializeAds();
                
            }

        }

        private void OnAdsInitialized()
        {
            Debug.Log("try loading add");
            adsManager.OnAdLoaded+=OnAdsLoaded;
            adsManager.LoadAd();
        }

        private void OnAdsLoaded()
        {
            adsButton.OnAdClicked+=View_OnAdsButtonClicked;
            Context.CommandManager.InvokeCommand(new AdLoadedCommand());
        }

        private void View_OnAdsButtonClicked()
        {
            adsManager.ShowAd();
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){throw new System.Exception("MustBeInitialized");}

        }

        
    }
}