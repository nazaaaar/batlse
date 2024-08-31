using System;
using RMC.Mini;
using RMC.Mini.Service;
using UnityEngine;
using UnityEngine.Advertisements;
 
public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IService
{
   [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null;

    public bool IsInitialized{get; private set;}

    public IContext Context{get; private set;}

    public event Action OnAdLoaded;
    void Awake()
    {   
        #if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
        #elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
        #elif UNITY_EDITOR
            _adUnitId = _androidAdUnitId;
        #endif

    }

    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        OnAdLoaded?.Invoke();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //throw new NotImplementedException();
    }

    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //throw new NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //throw new NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        ///throw new NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
        }
    }

    public void Initialize(IContext context)
    {
        if (!IsInitialized){
            IsInitialized = true;
            Context = context;
        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized){
            throw new Exception("MustBeInitialized");
        }
    }
}