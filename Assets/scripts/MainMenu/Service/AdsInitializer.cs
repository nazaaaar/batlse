using System;
using RMC.Mini;
using RMC.Mini.Service;
using UnityEngine;
using UnityEngine.Advertisements;
 
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IService
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    public bool IsInitialized{get; private set;}

    public IContext Context{get; private set;}

    public event Action OnAdsInitialized;

    public void Initialize(IContext context)
    {
        if (!IsInitialized){
            IsInitialized = true;
            Context = context;
        }
    }

    public void InitializeAds()
    {
    #if UNITY_IOS
            _gameId = _iOSGameId;
    #elif UNITY_ANDROID
            _gameId = _androidGameId;
    #elif UNITY_EDITOR
            _gameId = _androidGameId; 
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

 
    public void OnInitializationComplete()
    {
        OnAdsInitialized?.Invoke();
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized){
            throw new Exception("MustBeInitialized");
        }
    }
}