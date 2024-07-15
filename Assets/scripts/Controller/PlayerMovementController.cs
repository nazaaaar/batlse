using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using RMC.Mini.Controller;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.controller
{
    public class PlayerMovementController : IController
    {
        private readonly IPlayerInput playerInput;
        private readonly IPlayerView playerView;
        private readonly PlayerModel playerModel;
        private bool isInitialized;

        private IContext context;
        private DownPressedCommand downPressedCommand = new();
        private LeftPressedCommand leftPressedCommand = new();
        private RightPressedCommand rightPressedCommand  = new();
        private UpPressedCommand upPressedCommand  = new();

        public PlayerMovementController(IPlayerInput playerInput, IPlayerView playerView, PlayerModel playerModel)
        {
            this.playerInput = playerInput;
            this.playerView = playerView;
            this.playerModel = playerModel;
        }

        public bool IsInitialized => isInitialized;

        public IContext Context => context;

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IContext context)
        {
            if (!isInitialized){
                isInitialized=true;

                this.context = context;

                playerInput.OnDownPressedUp += View_OnDownPressedUp;
                playerInput.OnUpPressedUp += View_OnUpPressedUp;
                playerInput.OnRightPressedUp += View_OnRightPressedUp;
                playerInput.OnLeftPressedUp += View_OnLeftPressedUp;

                playerInput.OnDownPressedDown += View_OnDownPressedDown;
                playerInput.OnLeftPressedDown += View_OnLeftPressedDown;
                playerInput.OnRightPressedDown += View_OnRightPressedDown;
                playerInput.OnUpPressedDown += View_OnUpPressedDown;

                playerView.OnPlayerMoved += View_OnPlayerMoved;
    
            }
        }

     

        private void View_OnLeftPressedDown()
        {
            leftPressedCommand.isPressed=true;
            context.CommandManager.InvokeCommand(leftPressedCommand);
        }

        private void View_OnRightPressedDown()
        {
            rightPressedCommand.isPressed=true;
            context.CommandManager.InvokeCommand(rightPressedCommand);
        }

        private void View_OnUpPressedDown()
        {
            upPressedCommand.isPressed=true;
            context.CommandManager.InvokeCommand(upPressedCommand);
        }

        private void View_OnDownPressedDown()
        {
            downPressedCommand.isPressed=true;
            context.CommandManager.InvokeCommand(downPressedCommand);
        }

        private void View_OnLeftPressedUp()
        {
            leftPressedCommand.isPressed=false;
            context.CommandManager.InvokeCommand(leftPressedCommand);
        }

        private void View_OnRightPressedUp()
        {
            rightPressedCommand.isPressed=false;
            context.CommandManager.InvokeCommand(rightPressedCommand);
        }

        private void View_OnUpPressedUp()
        {
            upPressedCommand.isPressed=false;
            context.CommandManager.InvokeCommand(upPressedCommand);
        }

        private void View_OnDownPressedUp()
        {
            downPressedCommand.isPressed=false;
            context.CommandManager.InvokeCommand(downPressedCommand);
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