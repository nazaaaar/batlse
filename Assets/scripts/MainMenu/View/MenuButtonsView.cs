using System;
using System.Collections;
using System.Collections.Generic;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using RMC.Mini;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsView : MonoBehaviour, IMenuButtonsView
{
    public bool IsInitialized {get; private set;}

    public IContext Context {get; private set;}

    public event Action OnStartPressed;
    public event Action OnSettingsPressed;
    public event Action OnBackPressed;

    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Button SettingsButton;
    [SerializeField]
    private List<Button> BackButtons;

    public void Initialize(IContext context)
    {
        if (!IsInitialized){
            IsInitialized = true;
            Context = context;

            StartButton.onClick.AddListener(OnStartBtnPressed);
            SettingsButton.onClick.AddListener(OnSettingsBtnPressed);

            foreach (var button in BackButtons)
            {
                button.onClick.AddListener(OnBackBtnPressed);
            }
        }
    }

    private void OnBackBtnPressed()
    {
        OnBackPressed?.Invoke();
    }

    private void OnSettingsBtnPressed()
    {
        OnSettingsPressed?.Invoke();
    }

    private void OnStartBtnPressed()
    {
        OnStartPressed?.Invoke();
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized){throw new Exception("MustBeInitialized");}
    }   

}
