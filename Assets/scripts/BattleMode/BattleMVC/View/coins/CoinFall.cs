using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using Unity.Netcode;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view{
public class CoinFall: NetworkBehaviour, ICoinFall{
    
    [SerializeField]
    public float fallSpeed;

    [SerializeField]
    public float yLevel;

    private bool isFalled = false;

    public bool IsInitialized {get; private set;}

    public IContext Context {get; private set;}

    public void Initialize(IContext context)
    {
        if (!IsInitialized) {
            IsInitialized = true;
            Context = context;
        }
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized) {throw new System.Exception("MustBeInitialized");}
    }

    void FixedUpdate(){
        if (!isFalled)
        {
            this.transform.Translate(0,-fallSpeed*Time.fixedDeltaTime,0);
            if (transform.position.y <= yLevel)
            {
                isFalled = true;
                transform.position = new Vector3(transform.position.x, yLevel, transform.position.z);
            }
        }
    }

        public override void OnNetworkSpawn()
        {
            Reset();           
            base.OnNetworkSpawn();
        }

        public void Reset(){
        
        isFalled = false;
    }
}
}