using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;
using TMPro;
using System;

namespace nazaaaar.platformBattle.mini.view
{
    public class GameEndView :MonoBehaviour, IGameEndView
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}
        [SerializeField]
        private GameObject WinText;
        [SerializeField]
        private GameObject LoseText;
        [SerializeField]
        private GameObject DrawText;


        public void Initialize(IContext context)
        {
            if (!IsInitialized) {
                IsInitialized = true;
                Context = context;
                
                Context.CommandManager.AddCommandListener<GameFinishedCommand>(OnGameFinished);
            }
        }

        private void OnGameFinished(GameFinishedCommand e)
        {
            ConfigMessage(e);
            gameObject.SetActive(true);
        }

        private void ConfigMessage(GameFinishedCommand e)
        {

            switch (e.gameEndState)
            {
                case GameFinishedCommand.GameEndState.Win:
                    WinText.SetActive(true);
                    break;
                case GameFinishedCommand.GameEndState.Lose:
                    LoseText.SetActive(true);
                    break;
                case GameFinishedCommand.GameEndState.Draw:
                    DrawText.SetActive(true);
                    break;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }
    }

}