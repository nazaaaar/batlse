using RMC.Mini;
using UnityEngine;
using nazaaaar.slime.mini.viewAbstract;
using nazaaaar.slime.mini.controller.commands;
using nazaaaar.slime.mini.model;
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
        public event Action OnSlimeVictoryEndAnimation;

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;

                Context.CommandManager.AddCommandListener<MonsterStateChangedCommand>(OnMonsterStateChanged);
                Context.CommandManager.AddCommandListener<MonsterAttackStartCommand>(OnMonsterAttackStart);
            }
        }

        private void OnMonsterAttackStart(MonsterAttackStartCommand e)
        {
            Animator.SetTrigger("Idle");
            Animator.SetTrigger("Attack");
        }
        public void OnDiedAnimation(){
            OnSlimeDiedEndAnimation?.Invoke();
        }
        private void OnMonsterStateChanged(MonsterStateChangedCommand e)
        {
            switch (e.monsterState)
            {
                case MonsterState.Dying:
                    Animator.SetTrigger("Die");
                    
                    break;
                case MonsterState.Spawning:
                    Animator.SetTrigger("Idle");
                    break;
                case MonsterState.Moving:
                case MonsterState.Battling:
                    Animator.SetTrigger("Run");
                    break;
                case MonsterState.Victory:
                    Animator.SetTrigger("Victory");
                    StartCoroutine(ScheduleVictotyEnd(2.2f));
                    break;
            }

        }

        private IEnumerator ScheduleVictotyEnd(float delay){
            yield return new WaitForSeconds(delay);
            OnSlimeVictoryEndAnimation?.Invoke();
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
            Context.CommandManager.RemoveCommandListener<MonsterAttackStartCommand>(OnMonsterAttackStart);
        }
    }
}