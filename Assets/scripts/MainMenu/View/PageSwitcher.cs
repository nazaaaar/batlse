using System;
using nazaaaar.platformBattle.MainMenu.controller.commands;
using RMC.Mini;
using UnityEngine;
namespace nazaaaar.platformBattle.MainMenu.viewAbstract
{
    public class PageSwitcher : MonoBehaviour, IPageSwitcher
    {
        [SerializeField]
        private GameObject startPage;
        [SerializeField]
        private GameObject settingsPage;
        [SerializeField]
        private GameObject findGamePage;

        private GameObject activePage;

        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                activePage = startPage;

                context.CommandManager.AddCommandListener<CurrentPageChangedCommand>(OnCurrentPageChanged);
            }
        }

        private void OnCurrentPageChanged(CurrentPageChangedCommand e)
        {
            switch (e.CurrentPage){
                    case model.CurrentPage.StartPage: 
                        SwitchPageTo(startPage);break;
                    case model.CurrentPage.Settings: 
                        SwitchPageTo(settingsPage);break;
                    case model.CurrentPage.FindGame: 
                        SwitchPageTo(findGamePage);break;
                    default: throw new Exception("NotSupportedCurrentPage");
                }
        }

        private void SwitchPageTo(GameObject page){
            activePage.SetActive(false);
            activePage = page;
            activePage.SetActive(true);
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){
                throw new System.Exception("MustBeInitialized");
            }
        }
    }
}