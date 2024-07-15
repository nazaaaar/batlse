using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using System;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        private bool isInitialized;
        private IContext context;
        [SerializeField]
        private Rigidbody rigidbody;

        public bool IsInitialized { get => isInitialized; }
        public IContext Context => context;

        public bool IsRightPressed { get; set; }
        public bool IsLeftPressed { get; set; }
        public bool IsUpPressed { get; set; }
        public bool IsDownPressed { get; set; }
        public float MoveSpeed { get; set; } = 5f;

        public event Action<Vector3> OnPlayerMoved;

        private void Awake()
        {
            
        }

        public void Initialize(IContext context)
        {
            if (!isInitialized)
            {
                if (rigidbody == null)
                {
                    throw new Exception("Rigidbody component is missing on the player GameObject.");
                }
                isInitialized = true;
                this.context = context;

                this.context.CommandManager.AddCommandListener<DownPressedCommand>(OnDownPressedCommand);
                this.context.CommandManager.AddCommandListener<UpPressedCommand>(OnUpPressedCommand);
                this.context.CommandManager.AddCommandListener<LeftPressedCommand>(OnLeftPressedCommand);
                this.context.CommandManager.AddCommandListener<RightPressedCommand>(OnRightPressedCommand);
                this.context.CommandManager.AddCommandListener<MoveSpeedChangedCommand>(OnMoveSpeedChanged);

                IsRightPressed = false;
                IsLeftPressed = false;
                IsUpPressed = false;
                IsDownPressed = false;
            }
        }

        private void OnMoveSpeedChanged(MoveSpeedChangedCommand e)
        {
            this.MoveSpeed = e.MoveSpeed;
        }

        private void OnRightPressedCommand(RightPressedCommand e)
        {
            IsRightPressed = e.isPressed;
        }

        private void OnLeftPressedCommand(LeftPressedCommand e)
        {
            IsLeftPressed = e.isPressed;
        }

        private void OnUpPressedCommand(UpPressedCommand e)
        {
            IsUpPressed = e.isPressed;
        }

        private void OnDownPressedCommand(DownPressedCommand e)
        {
            IsDownPressed = e.isPressed;
        }

        private void FixedUpdate()
        {
            CheckMovement();
        }

        public void CheckMovement()
        {
            Vector3 movementDirection = new();
            if (IsDownPressed) movementDirection -= transform.forward;
            if (IsUpPressed) movementDirection += transform.forward;
            if (IsRightPressed) movementDirection += transform.right;
            if (IsLeftPressed) movementDirection -= transform.right;
            movementDirection.Normalize();
            rigidbody.velocity = movementDirection * MoveSpeed;
            OnPlayerMoved?.Invoke(movementDirection);
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized) { throw new Exception("MustBeInitialized"); }
        }
    }
}
