using RMC.Mini;
using UnityEngine;
using nazaaaar.slime.mini.viewAbstract;
using nazaaaar.slime.mini.controller.commands;
using nazaaaar.slime.mini.model;
using Unity.Netcode;

namespace nazaaaar.slime.mini.view
{
    public class SlimeHealthBar : NetworkBehaviour, ISlimeHealthBar
    {
        public bool IsInitialized {get; private set;} = false;

        public IContext Context {get; private set;}

        private SlimeModel slimeModel;

        [SerializeField]
        private float maxScaleX;

        public override void OnNetworkSpawn(){
            base.OnNetworkSpawn();



            IsInitialized=false;
            
            Vector3 scale = transform.localScale;
            scale.Set(maxScaleX,transform.localScale.y,transform.localScale.z);
            transform.localScale = scale;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                slimeModel = Context.ModelLocator.GetItem<SlimeModel>();
                Context.CommandManager.AddCommandListener<MonsterHealthChangedCommand>(OnMonsterHealthChanged);
            }
        }

        private void OnMonsterHealthChanged(MonsterHealthChangedCommand e)
        {
            Debug.Log((float)e.amount/slimeModel.MaxHealth.Value*maxScaleX);
            Vector3 scale = transform.localScale;
            scale.Set((float)e.amount/slimeModel.MaxHealth.Value*maxScaleX,transform.localScale.y,transform.localScale.z);
            transform.localScale = scale;
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }

        void OnDisable(){
            if (IsInitialized)Context.CommandManager.RemoveCommandListener<MonsterHealthChangedCommand>(OnMonsterHealthChanged);           
        }
    }
}