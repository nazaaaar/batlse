using nazaaaar.platformBattle.mini.view;

using UnityEngine;

namespace nazaaaar.platform.battle.mini
{
    class PlarformBattleStarter: MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private PlayerView playerView;
        [SerializeField]
        private PlayerAnimation playerAnimation;

        void Awake(){

            PlatformBattleMini platformBattleMini = new (playerView, playerInput, playerAnimation);

            platformBattleMini.Initialize();
        }
    }
}