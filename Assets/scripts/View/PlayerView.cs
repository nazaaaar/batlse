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
        private CharacterController characterController;

        public bool IsInitialized { get => isInitialized; }
        public IContext Context => context;

        public float MoveSpeed { get; set; } = 5f;

        public Vector3 MovementDirection { get; private set; } = Vector3.zero;
        public event Action<Vector3> OnPlayerMoved;

        private void Awake()
        {
            
        }

        public void Initialize(IContext context)
        {
            if (!isInitialized)
            {
                if (characterController == null)
                {
                    throw new Exception("CharacterController component is missing on the player GameObject.");
                }
                isInitialized = true;
                this.context = context;

                this.context.CommandManager.AddCommandListener<PlayerMovePressedCommand>(OnPlayerMovePressed);
                this.context.CommandManager.AddCommandListener<MoveSpeedChangedCommand>(OnMoveSpeedChanged);
            }
        }

        private void OnPlayerMovePressed(PlayerMovePressedCommand e)
        {
            MovementDirection = new Vector3(e.Vector.x, 0, e.Vector.y);
        }

        private void OnMoveSpeedChanged(MoveSpeedChangedCommand e)
        {
            this.MoveSpeed = e.MoveSpeed;
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            Vector3 movement = Vector3.zero;
            if (MovementDirection != Vector3.zero)
            {
                // Rotate the player immediately towards the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(MovementDirection);
                transform.rotation = targetRotation;
                
                // Move the player forward
                movement = transform.forward * MoveSpeed * Time.fixedDeltaTime;
                characterController.Move(movement);
                
                
            }
            OnPlayerMoved?.Invoke(movement);
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized) { throw new Exception("MustBeInitialized"); }
        }

        
    }

    }
