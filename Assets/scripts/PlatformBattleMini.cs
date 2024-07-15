using nazaaaar.platformBattle.mini.controller;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.platformBattle.mini.viewAbstract;
using RMC.Mini;
using System;
namespace nazaaaar.platform.battle.mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: A mini MVCS for core gameplay logic
    /// </summary>
    public class PlatformBattleMini: ISimpleMiniMvcs
    {

        public bool IsInitialized => isInitialized;

        public PlatformBattleController PlatformBattleController { get => platformBattleController; set => platformBattleController = value; }
        private readonly IPlayerView playerView;
        private readonly IPlayerInput playerInput;
        private readonly IPlayerAnimation playerAnimation;
        private PlatformBattleController platformBattleController;

        private PlayerModel playerModel;

        

        private PlayerMovementController playerMovementController;

        private bool isInitialized;

        private Context context;
        

        public PlatformBattleMini(IPlayerView playerView, IPlayerInput playerInput, IPlayerAnimation playerAnimation)
            {
                this.playerView = playerView;
                this.playerInput = playerInput;
                this.playerAnimation = playerAnimation;
            }

        public void Initialize()
        {
            if (!isInitialized){
                isInitialized = true;
                               
                context = new Context ();
                
                

                platformBattleController = new PlatformBattleController();
                playerMovementController = new (playerInput, playerView, playerModel);
                playerModel = new();


                playerInput.Initialize (context);
                playerView.Initialize (context);
                playerAnimation.Initialize(context);
             //   platformBattleController.Initialize(context);
                playerMovementController.Initialize (context);
                playerModel.Initialize (context);
                
            }
        }

        public void RequireIsInitialized()
        {
            if (!isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        


    }
}
