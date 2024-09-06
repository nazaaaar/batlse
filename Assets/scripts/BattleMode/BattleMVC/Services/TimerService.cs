using System.Collections;
using RMC.Mini;
using RMC.Mini.Service;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.service{
    public class TimerService : MonoBehaviour, IService
    {
        public bool IsInitialized{get; private set;}

        public IContext Context{get; private set;}

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;
            }
        }

        private Coroutine timeRoutine;
        public void StopSecondTimer(){
            if (timeRoutine != null){
                StopCoroutine(timeRoutine);
            }
        }
        public void StartSecondTimer(System.Action action)
        {
            timeRoutine = StartCoroutine(SecondTimerCoroutine(action));
        }

        private IEnumerator SecondTimerCoroutine(System.Action onSecondPassed)
        {
            while (true)
            {
                yield return WaitForEndPool.WaitForSeconds(1f);
                onSecondPassed?.Invoke();
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){
                throw new System.Exception("MustBeInitialized");
            }
        }
    }
}