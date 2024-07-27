using System;
using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using RMC.Mini.Controller;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nazaaaar.platformBattle.mini.controller
{
    public class SlimeController : IController
    {
        private IContext context;
        private bool isInitialized;

        public bool IsInitialized => isInitialized;

        public IContext Context => context;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IContext context)
        {
            if (!isInitialized){
                isInitialized=true;

                this.context = context;
            }
        }
        public void RequireIsInitialized()
        {
            if (!isInitialized){throw new System.Exception("MustBeInitialized");}
        }
    }
}