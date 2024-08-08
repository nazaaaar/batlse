using System;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{

    public class PlayerAnimation: MonoBehaviour, IPlayerAnimation
    {
        [SerializeField]
        private Animator animator;

        private bool isInitialized;

        private IContext context;

        public bool IsInitialized { get => isInitialized; }

        public IContext Context => context;

        public Animator Animator { get => animator; set => animator = value; }

        public void Initialize(IContext context)
        {
            if (!isInitialized){
                isInitialized = true;

                this.context = context;
                
                context.CommandManager.AddCommandListener<PlayerMovedCommand>(OnPlayerMovedCommand);
                
            }
        }

        private void OnPlayerMovedCommand(PlayerMovedCommand e)
        {
            bool IsPlayerMoving = e.Vector != Vector3.zero;
            
            animator.SetFloat("DirZ", IsPlayerMoving?0:1);

            animator.SetBool("IsPlayerMoving", IsPlayerMoving);
            
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized) {throw new Exception("MustBeInitialized");}
        }

    }
}