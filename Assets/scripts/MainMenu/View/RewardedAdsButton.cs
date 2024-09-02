using UnityEngine;
using UnityEngine.UI;
using RMC.Mini;
using nazaaaar.platformBattle.MainMenu.controller.commands;
using System;

public class RewardedAdsButton : MonoBehaviour, IRewardedAdsButton
{
    [SerializeField] Button _showAdButton;

    public event Action OnAdClicked;

    public bool IsInitialized{get; private set;}

    public IContext Context{get; private set;}

    public void Awake(){
        _showAdButton.interactable = false;
    }

    public void Initialize(IContext context)
    {
        if (!IsInitialized){
            IsInitialized = true;
            Context = context;

            Context.CommandManager.AddCommandListener<AdLoadedCommand>(OnAdLoaded);
        }
    }

    private void OnAdLoaded(AdLoadedCommand e)
    {
        _showAdButton.interactable = true;
        _showAdButton.onClick.AddListener(OnAdButtonClicked);
    }

    private void OnAdButtonClicked()
    {
        OnAdClicked?.Invoke();
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized){
            throw new Exception("MustBeInitialized");
        }
    }

    void OnDestroy()
    {
        _showAdButton.onClick.RemoveAllListeners();
    }
}
