using System;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.slime.mini.controller.commands;
using nazaaaar.slime.mini.model;
using nazaaaar.slime.mini.viewAbstract;
using RMC.Mini;
using RMC.Mini.Controller;
using UnityEngine;

namespace nazaaaar.slime.mini.controller
{
    public class SlimeController : IController,  IMonster
    {
        public bool IsInitialized { get; private set; }

        public IContext Context { get; private set; }

        private readonly SlimeModel slimeModel;
        private readonly ISlimeView slimeView;
        private readonly IContext globalContext;

        public UnityEngine.Vector3 Position => slimeView.Position;

        public SlimeController(SlimeModel slimeModel, ISlimeView slimeView, IContext globalContext)
        {
            this.slimeModel = slimeModel;
            this.slimeView = slimeView;
            this.globalContext = globalContext;
        }

        public Team Team { get => this.slimeModel.Team.Value; set => this.slimeModel.Team.Value = value; }

        public MonterState MonterState => this.slimeModel.MonterState.Value;

        public float AttackSpeed => this.slimeModel.AttackSpeed.Value;

        public int Damage => this.slimeModel.Damage.Value;

        public float MoveSpeed => this.slimeModel.MoveSpeed.Value;

        public float AgroRange => this.slimeModel.AgroRange.Value;

        public float Speed =>  this.slimeModel.Speed.Value;

        private readonly MonsterStateChangedCommand monsterStateChangedCommand = new();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized=true;

                this.Context = context;

                slimeModel.MonterState.OnValueChanged.AddListener(Model_OnMonsterStateChandged);
            }
        }

        private void Model_OnMonsterStateChandged(MonterState oldValue, MonterState newValue)
        {
            monsterStateChangedCommand.monsterState = newValue;
            Context.CommandManager.InvokeCommand(monsterStateChangedCommand);
        }

        public void RecieveDamage(int amount)
        {
            slimeModel.Health.Value-=amount;
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized){throw new System.Exception("MustBeInitialized");}
        }
    }
}