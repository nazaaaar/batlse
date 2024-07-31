using nazaaaar.platformBattle.mini.model;
using nazaaaar.slime.mini.controller;
using nazaaaar.slime.mini.model;
using nazaaaar.slime.mini.viewAbstract;
using RMC.Mini;
using System;
namespace nazaaaar.slime.mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: A mini MVCS for core gameplay logic
    /// </summary>
    public class SlimeMVC: ISimpleMiniMvcs
    {

        public bool IsInitialized => isInitialized;
    

        private bool isInitialized;

        private IContext GlobalContext {get; set;}
        private IContext LocalContext {get; set;}
        private readonly ISlimeView slimeView;
        private readonly MonsterSO monsterSO;

        public SlimeController slimeController {get; private set;}

        public SlimeMVC(IContext context, ISlimeView slimeView, MonsterSO monsterSO)
        {
            this.GlobalContext = context;
            this.slimeView = slimeView;
            this.monsterSO = monsterSO;
        }

        public void Initialize()
        {
            if (!isInitialized){
                isInitialized = true;

                SlimeModel slimeModel = new();
                slimeController = new(slimeModel, slimeView, GlobalContext);
                LocalContext = new Context();

                
                slimeController.Initialize(LocalContext);
                slimeModel.Initialize(LocalContext);
                slimeView.Initialize(LocalContext);

                slimeModel.AgroRange.Value = monsterSO.agroRange;
                slimeModel.AttackSpeed.Value = monsterSO.attackSpeed;
                slimeModel.Speed.Value = monsterSO.speed;
                slimeModel.Health.Value = monsterSO.health;
                slimeModel.Damage.Value = monsterSO.damage;
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
