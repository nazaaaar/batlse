using nazaaaar.platformBattle.mini.model;
using nazaaaar.slime.mini.controller;
using nazaaaar.slime.mini.model;
using nazaaaar.slime.mini.view;
using nazaaaar.slime.mini.viewAbstract;
using RMC.Mini;
using System;
using UnityEngine;
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
        private readonly SlimeFinder slimeFinder;
        private readonly SlimeAnimation slimeAnimation;
        private readonly SlimeHealthBar slimeHealthBar;
        private readonly MonsterSO monsterSO;
        private readonly Quaternion rotation;
        private readonly Team team;

        public SlimeController slimeController {get; private set;}

        public SlimeMVC(IContext context, ISlimeView slimeView, SlimeFinder slimeFinder,SlimeAnimation slimeAnimation, SlimeHealthBar slimeHealthBar, MonsterSO monsterSO, UnityEngine.Quaternion rotation, Team team)
        {
            this.GlobalContext = context;
            this.slimeView = slimeView;
            this.slimeFinder = slimeFinder;
            this.slimeAnimation = slimeAnimation;
            this.slimeHealthBar = slimeHealthBar;
            this.monsterSO = monsterSO;
            this.rotation = rotation;
            this.team = team;
        }

        public void Initialize()
        {
            if (!isInitialized){
                isInitialized = true;

                SlimeModel slimeModel = new();
                slimeController = new(slimeModel, slimeView, slimeFinder, slimeAnimation,monsterSO, GlobalContext);
                LocalContext = new Context();

                
                slimeModel.Initialize(LocalContext);

                slimeModel.AgroRange.Value = monsterSO.agroRange;
                slimeModel.AttackSpeed.Value = monsterSO.attackSpeed;
                slimeModel.Speed.Value = monsterSO.speed;
                slimeModel.MaxHealth.Value = monsterSO.health;
                slimeModel.Health.Value = monsterSO.health;
                slimeModel.Damage.Value = monsterSO.damage;
                slimeModel.AttackRange.Value = monsterSO.attackRange;
                slimeModel.Team.Value = team;
                slimeModel.BaseDirection.Value = rotation;
                slimeModel.FirstAttackDelay.Value = monsterSO.firstAttackDelay;
                if (team == Team.Red){
                    slimeModel.BoundZ.Value = 2.5f;
                }
                else if (team == Team.Blue){
                    slimeModel.BoundZ.Value = 10.5f;
                }

                slimeAnimation.Initialize(LocalContext);
                slimeFinder.Initialize(LocalContext);
                slimeView.Initialize(LocalContext);
                slimeHealthBar.Initialize(LocalContext);
                slimeController.Initialize(LocalContext);
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
