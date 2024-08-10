using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;
using TMPro;

namespace nazaaaar.platformBattle.mini.view
{
    public class OtherCoinAmountUI :MonoBehaviour, ICoinAmountUI
    {
        public bool IsInitialized {get; private set;}

        public IContext Context {get; private set;}

        [SerializeField]
        private TextMeshProUGUI textCoinAmount;

        public void Initialize(IContext context)
        {
            if (!IsInitialized) {
                IsInitialized = true;
                Context = context;
                context.CommandManager.AddCommandListener<OtherCoinAmountChangedCommand>(OnCoinAmountChanged);
            }
        }

        private void OnCoinAmountChanged(OtherCoinAmountChangedCommand e)
        {
            textCoinAmount.text = e.coinAmount.ToString();
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }
    }
}