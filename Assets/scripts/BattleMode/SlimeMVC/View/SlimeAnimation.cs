using RMC.Mini;
using UnityEngine;
using nazaaaar.slime.mini.viewAbstract;
using nazaaaar.slime.mini.controller.commands;
using nazaaaar.slime.mini.model;
using nazaaaar.slime.mini.controller;
using nazaaaar.platformBattle.mini.model;
using System;
using System.Collections;

namespace nazaaaar.slime.mini.view
{
    public class SlimeAnimation : MonoBehaviour, ISlimeAnimation
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        [field: SerializeField]
        public Animator Animator { get; set; }

        public event Action OnSlimeDiedEndAnimation;

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;

                context.CommandManager.AddCommandListener<MonsterStateChangedCommand>(OnMonsterStateChanged);
            }
        }

        private void OnMonsterStateChanged(MonsterStateChangedCommand e)
        {
            if (e.monsterState == MonsterState.Dying){
                OnSlimeDiedEndAnimation?.Invoke();
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }

        public void DestroySelf()
        {
            Destroy(this);
        }

        void OnDestroy(){
            Context.CommandManager.RemoveCommandListener<MonsterStateChangedCommand>(OnMonsterStateChanged);
        }
    }
}