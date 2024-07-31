using RMC.Mini;
using RMC.Mini.View;
using UnityEngine;

namespace nazaaaar.platformBattle.mini.view
{
    public class CoinRotation: MonoBehaviour, IView
{
    [SerializeField]
    public float rotationSpeed;

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
        transform.Rotate(new Vector3(0, rotationSpeed * Time.fixedDeltaTime,0));
    }
}
}