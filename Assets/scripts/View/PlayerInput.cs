
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using System;
using UnityEngine;
namespace nazaaaar.platformBattle.mini.view
{
    public class PlayerInput : MonoBehaviour, IPlayerInput
    {
        [SerializeField]
        private KeyCode RightKey {get; set;} = KeyCode.D;
        [SerializeField]
        private KeyCode LeftKey {get; set;}= KeyCode.A;
        [SerializeField]
        private KeyCode UpKey {get; set;}= KeyCode.W;
        [SerializeField]
        private KeyCode DownKey {get; set;}= KeyCode.S;

        private bool isInitialized {get; set;}

        private IContext context {get; set;}

        public bool IsInitialized { get => isInitialized; }

        public IContext Context => context;

        public Action OnRightPressedUp { get; set; }
        public Action OnLeftPressedUp { get; set; }
        public Action OnUpPressedUp { get; set; }
        public Action OnDownPressedUp { get; set; }
        public Action OnRightPressedDown { get; set; }
        public Action OnLeftPressedDown { get; set; }
        public Action OnUpPressedDown { get; set; }
        public Action OnDownPressedDown { get; set; }

        public void Initialize(IContext context)
        {
            if (!isInitialized){
                isInitialized = true;

                this.context = context;
                

                
            }
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized) {throw new Exception("MustBeInitialized");}
        }

        private void Update(){
            if (Input.GetKeyUp(UpKey)) OnUpPressedUp?.Invoke();
            if (Input.GetKeyUp(DownKey)) OnDownPressedUp?.Invoke();
            if (Input.GetKeyUp(LeftKey)) OnLeftPressedUp?.Invoke();
            if (Input.GetKeyUp(RightKey)) OnRightPressedUp?.Invoke();

            if (Input.GetKeyDown(UpKey)) OnUpPressedDown?.Invoke();
            if (Input.GetKeyDown(DownKey)) OnDownPressedDown?.Invoke();
            if (Input.GetKeyDown(LeftKey)) OnLeftPressedDown?.Invoke();
            if (Input.GetKeyDown(RightKey)) OnRightPressedDown?.Invoke();
        }
    }
}
