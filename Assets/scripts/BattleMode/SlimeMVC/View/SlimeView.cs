using RMC.Mini;
using UnityEngine;
using nazaaaar.slime.mini.viewAbstract;
using nazaaaar.slime.mini.controller.commands;
using nazaaaar.slime.mini.model;
using nazaaaar.slime.mini.controller;
using nazaaaar.platformBattle.mini.model;
using System;
using System.Collections;
using Unity.Netcode;

namespace nazaaaar.slime.mini.view
{
    public class SlimeView : MonoBehaviour, ISlimeView
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public Vector3 Position => transform.position;
        [field: SerializeField]
        public CharacterController CharacterController { get; set; }

        private MonsterState monsterState = MonsterState.Spawning;

        private SlimeModel slimeModel;

        private IMonster target;

        private float sqrAttackRange;

        private Coroutine battleRoutine = null;

        public event Action<IMonster> OnTargetHit;

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;

                context.CommandManager.AddCommandListener<MonsterStateChangedCommand>(OnMonsterStateChanged);
                context.CommandManager.AddCommandListener<MonsterTargetCommand>(OnMonsterTargetChanged);
                context.CommandManager.AddCommandListener<MonsterAttackRangeCommand>(OnAttackRangeChanged);
                slimeModel = context.ModelLocator.GetItem<SlimeModel>();
            }
        }

        void OnDestroy(){
                Context.CommandManager.RemoveCommandListener<MonsterStateChangedCommand>(OnMonsterStateChanged);
                Context.CommandManager.RemoveCommandListener<MonsterTargetCommand>(OnMonsterTargetChanged);
                Context.CommandManager.RemoveCommandListener<MonsterAttackRangeCommand>(OnAttackRangeChanged);
        }

        private void OnAttackRangeChanged(MonsterAttackRangeCommand e)
        {
            sqrAttackRange = e.attackRange * e.attackRange;
        }

        private void OnMonsterTargetChanged(MonsterTargetCommand e)
        {
            target = e.target;
        }

        private void OnMonsterStateChanged(MonsterStateChangedCommand e)
        {
            monsterState = e.monsterState;
            if (monsterState==MonsterState.Dying){
                if (battleRoutine!=null)
                StopCoroutine(battleRoutine);
            }
        }

        void FixedUpdate(){
            switch (monsterState)
            {
                case MonsterState.Moving: HandleMovement(); break;
                case MonsterState.Battling: HandleBattle(); break;
                default: break;
            }
        }

        private void HandleBattle()
        {
            if (target==null) return;
            Vector3 difference = target.Position - transform.position;

            if (battleRoutine == null)
            {
                if (difference.sqrMagnitude <= sqrAttackRange)
                {
                    battleRoutine = StartCoroutine(Fight(slimeModel.AttackSpeed.Value));
                }
                else HandleBattleMovement(difference);
            }
            else if (battleRoutine != null && difference.sqrMagnitude > sqrAttackRange)
            {
                StopCoroutine(battleRoutine);
                battleRoutine = null;
            }
        }

        private void HandleBattleMovement(Vector3 difference)
        {
            var dir = difference.normalized;
            CharacterController?.Move(dir * slimeModel.Speed.Value*Time.fixedDeltaTime);    
        }

        private IEnumerator Fight(float time){
            while (true){
                if (target==null) {
                    battleRoutine=null;
                    yield break;
                }
                OnTargetHit?.Invoke(target);
                yield return new WaitForSeconds(time);
            }
        }

        private void HandleMovement()
        {
            CharacterController?.Move(transform.forward * slimeModel.Speed.Value*Time.fixedDeltaTime);    
        }

        public GameObject GameObject => gameObject;

    

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }

        public void DestroySelf()
        {
            CharacterController.enabled=false;
            Destroy(this);
        }
    }
}