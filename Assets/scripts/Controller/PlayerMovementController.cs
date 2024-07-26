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
    public class PlayerMovementController : IController
    {
        private readonly UnityEngine.InputSystem.PlayerInput playerInput;
        private readonly IPlayerView playerView;
        private readonly PlayerModel playerModel;
        private bool isInitialized;

        private IContext context;
        private PlayerMovePressedCommand playerMovePressedCommand = new();

        public PlayerMovementController(PlayerInput playerInput, IPlayerView playerView, PlayerModel playerModel)
        {
            this.playerInput = playerInput;
            this.playerView = playerView;
            this.playerModel = playerModel;
        }

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

                playerInput.actions["Move"].performed+=View_OnMovePressed;
                playerInput.actions["Move"].canceled+=View_OnMovePressed;

                playerView.OnPlayerMoved += View_OnPlayerMoved;
    
            }
        }

        private void View_OnMovePressed(InputAction.CallbackContext context)
        {
            playerMovePressedCommand.Vector = context.ReadValue<Vector2>();
            this.context.CommandManager.InvokeCommand(playerMovePressedCommand);
        }

        private void View_OnPlayerMoved(Vector3 vector)
        {
            context.CommandManager.InvokeCommand(new PlayerMovedCommand(vector));
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized){throw new System.Exception("MustBeInitialized");}
        }
    }
}