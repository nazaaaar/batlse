using nazaaaar.platformBattle.MainMenu.viewAbstract;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.service;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using RMC.Mini.Controller;
using System;
using System.Linq;
using UnityEngine;
namespace nazaaaar.platformBattle.mini.controller
{
    public class TimeController : IController
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}
        private readonly TimerService timerService;
        private readonly TimeModel timeModel;
        private readonly ITimerView timerView;
        private readonly IGameEndButton gameEndButton;

        public TimeController(TimerService timerService, TimeModel timeModel, ITimerView timerView, IGameEndButton gameEndButton)
        {
            this.timerService = timerService;
            this.timeModel = timeModel;
            this.timerView = timerView;
            this.gameEndButton = gameEndButton;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                Context = context;

                timeModel.Time.OnValueChanged.AddListener(Model_OnTimeChanged);

                Context.CommandManager.AddCommandListener<GameLoadedCommand>(OnGameLoaded);
                Context.CommandManager.AddCommandListener<GameTimePassed>(OnGameTimePassed);

                gameEndButton.OnMainMenuClicked += View_OnMainMenuClicked;
            }
        }

        private void View_OnMainMenuClicked()
        {
            Context.CommandManager.InvokeCommand(new MainMenuClickedCommand());
        }

        private void OnGameTimePassed(GameTimePassed e)
        {
            timerService.StopSecondTimer();
        }

        private void Model_OnTimeChanged(int oldValue, int newValue)
        {
            Context.CommandManager.InvokeCommand(new TimeChangedCommand(){time = newValue});
        }

        private void OnGameLoaded(GameLoadedCommand e)
        {
            timerService.StartSecondTimer(OnSecondPassed);
        }

        private void OnSecondPassed()
        {
            timeModel.Time.Value+=1;

            if (timeModel.Time.Value>=timeModel.MaxTime.Value){
                Context.CommandManager.InvokeCommand(new GameTimePassed());
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized){throw new Exception("MustBeInitialized");}
        }
    }
}
