using RMC.Mini;
using System;
using UnityEngine;
using nazaaaar.platformBattle.mini.viewAbstract;
using TMPro;
using nazaaaar.platformBattle.mini.controller.commands;

namespace nazaaaar.platformBattle.mini.view
{
    public class TimerView : MonoBehaviour, ITimerView
    {
        public bool IsInitialized {get; private set;}
        public TextMeshProUGUI timeText;

        public IContext Context {get; private set;}

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized = true;
                this.Context = context;

                Context.CommandManager.AddCommandListener<TimeChangedCommand>(OnTimeChanged);
            }
        }

        private void OnTimeChanged(TimeChangedCommand e)
        {
            RenderTime(e.time);
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)throw new Exception("MustBeInitialized");
        }

        private void RenderTime(int newValue)
        {
            var minutes = (int)(newValue / 60f);
            string seconds = (newValue - minutes * 60).ToString();
            if (seconds.Length<2) seconds = "0"+seconds;
            timeText.text = minutes + ":" + seconds;
        }
    }
}
