using System;
using nazaaaar.platformBattle.mini.controller.commands;
using RMC.Mini;
using RMC.Mini.View;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view{
    public class CameraFollow : MonoBehaviour, IView
    {
        public bool IsInitialized { get; private set; }

        public IContext Context { get; private set; }
        public Transform PlayerTransform { get => playerTransform; set => playerTransform = value; }

        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private Vector3 offset = new Vector3(0, 5, -10);

        [SerializeField]
        private float smoothSpeed = 0.125f;

        private Vector3 velocity = Vector3.zero;

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;

                context.CommandManager.AddCommandListener<PlayerMovedCommand>(OnPlayerMoved);
            }
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
