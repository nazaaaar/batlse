using RMC.Mini;
using UnityEngine;
using nazaaaar.slime.mini.viewAbstract;
using nazaaaar.slime.mini.controller.commands;
using nazaaaar.slime.mini.model;
using nazaaaar.slime.mini.controller;

namespace nazaaaar.slime.mini.view
{
    public class SlimeView : MonoBehaviour, ISlimeView
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        public Vector3 Position => transform.position;

        private MonterState monsterState;

        private SlimeModel slimeModel;

        private SlimeController target;

        [SerializeField]
        CharacterController characterController;

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;

                context.CommandManager.AddCommandListener<MonsterStateChangedCommand>(OnMonsterStateChanged);
                slimeModel = context.ModelLocator.GetItem<SlimeModel>();
            }
        }

        private void OnMonsterStateChanged(MonsterStateChangedCommand e)
        {
            monsterState = e.monsterState;
        }

        void FixedUpdate(){
            switch (monsterState)
            {
                case MonterState.Moving: HandleMovement(); break;
                case MonterState.Battling: HandleBattle(); break;
                default: break;
            }
        }

        private void HandleBattle()
        {
            
        }

        private void HandleMovement()
        {
            characterController.Move(transform.forward * slimeModel.Speed.Value*Time.fixedDeltaTime);    
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }
    }
}