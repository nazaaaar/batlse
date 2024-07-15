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
            animator.SetFloat("DirX", e.Vector.x);
            animator.SetFloat("DirZ", e.Vector.z);
            
            animator.SetBool("IsPlayerMoving", (e.Vector.x != 0 || e.Vector.z != 0));
            
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized) {throw new Exception("MustBeInitialized");}
        }

    }
}