using System.Collections;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class LoadScreenView : MonoBehaviour, ILoadScreenView
{
    public bool IsInitialized { get; private set; }

    public IContext Context { get; private set; }

    [SerializeField]
    private Transform LoadingSlider;

    [SerializeField]
    private float fakeLoadingDuration = 7.0f; 
    [SerializeField]
    private float timeout = 0.2f; 

    private Coroutine loadingCoroutine;

    private void Awake()
    {
        loadingCoroutine = StartCoroutine(FakeLoadingProcess());
    }

    public void Initialize(IContext context)
    {
        if (!IsInitialized)
        {
            IsInitialized = true;
            Context = context;

            Context.CommandManager.AddCommandListener<GameLoadedCommand>(OnGameLoaded);
        }
    }

    private void OnGameLoaded(GameLoadedCommand e)
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }

        SetSliderProgress(1.0f);

        Context.CommandManager.RemoveCommandListener<GameLoadedCommand>(OnGameLoaded);
        
        StartCoroutine(DeleteAfterTimeOut(timeout));
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized)
        {
            throw new System.Exception("MustBeInitialized");
        }
    }

    private IEnumerator DeleteAfterTimeOut(float timeout){
        yield return WaitForEndPool.WaitForSeconds(timeout);
        Destroy(gameObject);
    }
    private IEnumerator FakeLoadingProcess()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fakeLoadingDuration-1)
        {
            float progress = elapsedTime / fakeLoadingDuration;
            SetSliderProgress(progress);

            elapsedTime += Time.deltaTime;
            yield return WaitForEndPool.WaitForEndOfFrame();
        }

        SetSliderProgress(0.9f);
    }

    private void SetSliderProgress(float progress)
    {
        if (LoadingSlider != null)
        {
            LoadingSlider.localScale = new Vector3(progress, LoadingSlider.localScale.y, LoadingSlider.localScale.z);
        }
    }
}
}