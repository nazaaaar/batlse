using System;
using System.Collections;
using nazaaaar.platformBattle.MainMenu.controller.commands;
using nazaaaar.platformBattle.MainMenu.viewAbstract;
using RMC.Mini;
using UnityEngine;

namespace nazaaaar.platformBattle.MainMenu.view
{
    public class CameraView : MonoBehaviour, ICameraView
    {
        [SerializeField]
        private Transform startPageTransform;

        [SerializeField]
        private Transform findGameTransform;
        [SerializeField]
        private Transform settingsTranform;
        [SerializeField]
        private float smoothSpeed;

        private Vector3 velocity = Vector3.zero;
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        private Coroutine coroutine = null;

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;

                Context = context;

                Context.CommandManager.AddCommandListener<CurrentPageChangedCommand>(OnCurrentPageChanged);
            }
        }

        private void OnCurrentPageChanged(CurrentPageChangedCommand e)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            switch (e.CurrentPage){
                case model.CurrentPage.StartPage: 
                    coroutine = StartCoroutine(ChangePositionCoroutine
                        (startPageTransform.position, smoothSpeed));break;
                case model.CurrentPage.Settings: 
                    coroutine = StartCoroutine(ChangePositionCoroutine
                        (settingsTranform.position, smoothSpeed));break;;
                case model.CurrentPage.FindGame: 
                    coroutine = StartCoroutine(ChangePositionCoroutine
                        (findGameTransform.position, smoothSpeed));break;
                default: throw new Exception("NotSupportedCurrentPage");
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }

        private IEnumerator ChangePositionCoroutine(Vector3 desiredPosition, float smoothSpeed){
            while (transform.position !=desiredPosition){
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
                yield return WaitForEndPool.WaitForEndOfFrame();
            }
            Debug.Log("Routine stopped");
        }
    }
}