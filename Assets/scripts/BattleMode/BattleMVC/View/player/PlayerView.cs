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
        [SerializeField]
        private Vector3 bluePosition;
        [SerializeField]
        private Vector3 redPosition;
        

        public bool IsInitialized { get => isInitialized; }
        public IContext Context => context;

        public float MoveSpeed { get; set; } = 5f;

        public Vector3 MovementDirection { get; private set; } = Vector3.zero;
        public CharacterController CharacterController { get => characterController; set => characterController = value; }
        public Transform PlayerTransform { get; set; }

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

                this.context.CommandManager.AddCommandListener<TeamChangedCommand>(OnTeamChanged);
            }
        }

        private void OnTeamChanged(TeamChangedCommand e)
        {
            if (e.Team==model.Team.Blue) {
                characterController.enabled = false;
                PlayerTransform.position = bluePosition;
                characterController.enabled = true;
                
                }
            if (e.Team==model.Team.Red){
                characterController.enabled = false;
                PlayerTransform.position = redPosition;
                PlayerTransform.Rotate(0,180,0,Space.World);
                characterController.enabled = true;
                
                }

            this.context.CommandManager.AddCommandListener<PlayerMovePressedCommand>(OnPlayerMovePressed);
            this.context.CommandManager.AddCommandListener<MoveSpeedChangedCommand>(OnMoveSpeedChanged);
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
                Quaternion targetRotation = Quaternion.LookRotation(MovementDirection);
                PlayerTransform.rotation = targetRotation;
                
                movement = PlayerTransform.forward * MoveSpeed * Time.fixedDeltaTime;
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
