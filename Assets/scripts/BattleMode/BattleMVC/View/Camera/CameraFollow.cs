using System;
using nazaaaar.platformBattle.mini.controller.commands;
using RMC.Mini;
using RMC.Mini.View;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class CameraFollow : MonoBehaviour, IView
    {
        public bool IsInitialized { get; private set; }

        public IContext Context { get; private set; }
        public Transform PlayerTransform { get => playerTransform; set => playerTransform = value; }
        private Transform playerTransform;
        [SerializeField]
        private model.CameraConfig cameraConfig;        
        private Vector3 offset = new Vector3(0, 5, -10);

        private float smoothSpeed = 0.125f;

        private Vector3 velocity = Vector3.zero;

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;

                context.CommandManager.AddCommandListener<PlayerMovedCommand>(OnPlayerMoved);
                context.CommandManager.AddCommandListener<TeamChangedCommand>(OnTeamChanged);
                context.CommandManager.AddCommandListener<GameFinishedCommand>(OnGameEnd);
            }
        }

        private void OnGameEnd(GameFinishedCommand e)
        {
            Context.CommandManager.RemoveCommandListener<PlayerMovedCommand>(OnPlayerMoved);
            Context.CommandManager.RemoveCommandListener<TeamChangedCommand>(OnTeamChanged);
        }

        private void OnTeamChanged(TeamChangedCommand e)
        {
            if (e.Team == model.Team.Red){
                this.offset =new Vector3(cameraConfig.Offset.x, cameraConfig.Offset.y, -cameraConfig.Offset.z);
                Debug.Log(this.offset);
                this.transform.rotation = cameraConfig.BlueRotation;

                transform.Rotate(0,180,0,Space.World);
            }
            else if (e.Team == model.Team.Blue){
                this.offset = cameraConfig.Offset;
                this.transform.rotation = cameraConfig.BlueRotation;
            }
            this.smoothSpeed = cameraConfig.SmoothSpeed;
        }

        private void OnPlayerMoved(PlayerMovedCommand e)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new Exception("MustBeInitialized");
        }
    }
}
