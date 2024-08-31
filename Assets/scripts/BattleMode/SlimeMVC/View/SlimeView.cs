using RMC.Mini;
using UnityEngine;
using nazaaaar.slime.mini.viewAbstract;
using nazaaaar.slime.mini.controller.commands;
using nazaaaar.slime.mini.model;
using nazaaaar.platformBattle.mini.model;
using System;
using System.Collections;

namespace nazaaaar.slime.mini.view
{
    public class SlimeView : MonoBehaviour, ISlimeView
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public Vector3 Position => transform.position;
        [field: SerializeField]
        public CharacterController CharacterController { get; set; }

        private const float DeathDelay = 0.2f; //ActiveHitting after death
        private MonsterState monsterState = MonsterState.Spawning;

        private SlimeModel slimeModel;

        private IMonster target;

        private float sqrAttackRange;

        private Coroutine battleRoutine = null;
        private float rotationSpeed = 5;

        private Coroutine rotationRoutine = null;

        public event Action<IMonster> OnTargetHit;
        public event Action OnEndLinePassed;

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
            Debug.Log(slimeModel.Team.Value +": target is "+e.target);
        }

        private void OnMonsterStateChanged(MonsterStateChangedCommand e)
        {
            CheckPrevMonsterStateValue();
            monsterState = e.monsterState;
            MonsterStateChanged();

        }

        private void MonsterStateChanged()
        {
            CheckRotationOnStateChanged();
            if (monsterState == MonsterState.Dying){
                StopFightingScheduled(DeathDelay);
            }
        }

        private IEnumerator StopFightingScheduled(float delay){
            yield return new WaitForSeconds(delay);
            if (battleRoutine != null){
                StopCoroutine(battleRoutine);
                battleRoutine = null;
            }
        }

        private void CheckRotationOnStateChanged()
        {
            if (rotationRoutine != null && monsterState == MonsterState.Battling)
            {
                StopCoroutine(rotationRoutine);
                rotationRoutine = null;
            }
            if (monsterState == MonsterState.Dying && rotationRoutine != null)
            {
                StopCoroutine(rotationRoutine);
            }
        }

        private void CheckPrevMonsterStateValue()
        {
            if (monsterState == MonsterState.Battling)
            {
                rotationRoutine = StartCoroutine(RotateToBase());
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
            if (target == null) return;
            Vector3 difference = target.Position - transform.position;

            difference.y = 0;
            RotateTowards(difference);

            if (battleRoutine == null)
            {
                if (difference.sqrMagnitude <= sqrAttackRange)
                {
                    battleRoutine = StartCoroutine(StartFight(slimeModel.FirstAttackDelay.Value));
                }
                else HandleBattleMovement(difference);
            }
            else if (battleRoutine != null && difference.sqrMagnitude > sqrAttackRange)
            {
                StopCoroutine(battleRoutine);
                battleRoutine = null;
            }
        }

        private void RotateTowards(Vector3 difference)
        {
            Quaternion targetRotation = Quaternion.LookRotation(difference);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        private void HandleBattleMovement(Vector3 difference)
        {
            CharacterController?.Move(transform.forward * slimeModel.Speed.Value*Time.fixedDeltaTime);    
            if (transform.position.z >= slimeModel.BoundZ.Value){
                OnEndLinePassed?.Invoke();
            }
        }
        private IEnumerator StartFight(float delay){
            yield return new WaitForSeconds(delay);
            battleRoutine = StartCoroutine(Fight(slimeModel.AttackSpeed.Value));
        }

        private IEnumerator Fight(float time){
            while (true){
                if (target==null) {
                    battleRoutine=null;
                    yield break;
                }
                OnTargetHit?.Invoke(target);

                if (monsterState==MonsterState.Dying) {
                    battleRoutine=null;
                    yield break;
                }
                yield return new WaitForSeconds(time);
            }
        }

        private void HandleMovement()
        {
            CharacterController?.Move(transform.forward * slimeModel.Speed.Value*Time.fixedDeltaTime);    
            if (transform.position.z >= slimeModel.BoundZ.Value){
                OnEndLinePassed?.Invoke();
            }
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

        private IEnumerator RotateToBase(){
            while (transform.rotation != slimeModel.BaseDirection.Value){
                transform.rotation = Quaternion.Slerp(transform.rotation, slimeModel.BaseDirection.Value, rotationSpeed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}