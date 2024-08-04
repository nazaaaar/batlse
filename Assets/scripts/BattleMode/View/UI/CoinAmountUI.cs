using nazaaaar.platformBattle.mini.controller.commands;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using UnityEngine;
using TMPro;

namespace nazaaaar.platformBattle.mini.view
{
    public class CoinAmountUI :MonoBehaviour, ICoinAmountUI
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
                context.CommandManager.AddCommandListener<PlayerCoinAmountChangedCommand>(OnCoinAmountChanged);
            }
        }

        private void OnCoinAmountChanged(PlayerCoinAmountChangedCommand e)
        {
            textCoinAmount.text = e.coinAmount.ToString();
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized) throw new System.Exception("MustBeInitialized");
        }
    }
}