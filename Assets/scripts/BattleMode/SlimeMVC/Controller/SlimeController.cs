using System;
using nazaaaar.platformBattle.mini.controller.commands;
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
        private readonly SlimeFinder slimeFinder;
        private readonly IContext globalContext;

        public UnityEngine.Vector3 Position => slimeView.Position;

        private readonly ISlimeAnimation slimeAnimation;
        private readonly MonsterSO monsterSO;

        public SlimeController(SlimeModel slimeModel, ISlimeView slimeView, SlimeFinder slimeFinder,ISlimeAnimation slimeAnimation, MonsterSO monsterSO, IContext globalContext)
        {
            this.slimeModel = slimeModel;
            this.slimeView = slimeView;
            this.slimeFinder = slimeFinder;
            this.slimeAnimation = slimeAnimation;
            this.monsterSO = monsterSO;
            this.globalContext = globalContext;
            this.monstersList = globalContext.ModelLocator.GetItem<MonstersList>();
            
        }


        public Team Team { get => this.slimeModel.Team.Value; set => this.slimeModel.Team.Value = value; }

        public MonsterState MonterState => this.slimeModel.MonterState.Value;

        public float AttackSpeed => this.slimeModel.AttackSpeed.Value;

        public int Damage => this.slimeModel.Damage.Value;

        public float MoveSpeed => this.slimeModel.MoveSpeed.Value;

        public float AgroRange => this.slimeModel.AgroRange.Value;

        public float Speed =>  this.slimeModel.Speed.Value;

        public float AttackRange => this.slimeModel.AttackRange.Value;

        public int Health => this.slimeModel.Health.Value;

        public float FirstAttackDelay => this.slimeModel.FirstAttackDelay.Value;

        private readonly MonsterStateChangedCommand monsterStateChangedCommand = new();

        private readonly MonstersList monstersList;

        public void Dispose()
        {
                slimeModel.AttackRange.OnValueChanged.RemoveListener(Model_OnAttackRangeChanged);
                slimeModel.MonterState.OnValueChanged.RemoveListener(Model_OnMonsterStateChandged);
                slimeModel.Target.OnValueChanged.RemoveListener(Model_TargetChanged);
                slimeModel.Health.OnValueChanged.RemoveListener(Model_OnHealthChanged);

                this.slimeFinder.OnMonsterFound-=Service_OnMonsterFound;
                slimeView.OnTargetHit -= View_OnTargetHit;
                slimeAnimation.OnSlimeDiedEndAnimation -= View_OnDeathAnimationEnd;
                globalContext.CommandManager.RemoveCommandListener<MonsterDeathConfirmedCommand>(Gloabal_OnMonsterDeathConfirmed);

                GameObject gameObject = slimeView.GameObject;

                slimeView.DestroySelf();
                slimeAnimation.DestroySelf();
                slimeFinder.enabled=false;

                globalContext.CommandManager.InvokeCommand(new MonsterDespawnRequestCommand(){monster = gameObject, monsterSO=monsterSO});
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized){
                IsInitialized=true;

                this.Context = context;

                slimeModel.AttackRange.OnValueChanged.AddListener(Model_OnAttackRangeChanged);
                slimeModel.MonterState.OnValueChanged.AddListener(Model_OnMonsterStateChandged);
                slimeModel.Target.OnValueChanged.AddListener(Model_TargetChanged);
                slimeModel.Health.OnValueChanged.AddListener(Model_OnHealthChanged);
                slimeModel.MonterState.Value = MonsterState.Moving;
                this.slimeFinder.OnMonsterFound+=Service_OnMonsterFound;
                slimeView.OnTargetHit += View_OnTargetHit;
                slimeModel.AttackRange.Value = slimeModel.AttackRange.Value;
                slimeAnimation.OnSlimeDiedEndAnimation+= View_OnDeathAnimationEnd;
                slimeView.OnEndLinePassed += View_OnEndLinePassed;

                globalContext.CommandManager.AddCommandListener<MonsterDeathConfirmedCommand>(Gloabal_OnMonsterDeathConfirmed);
            }
        }

        private void View_OnEndLinePassed()
        {
            slimeView.OnEndLinePassed -= View_OnEndLinePassed;
            var deathCommand = new MonsterDeadCommand(){monster = this};
            globalContext.CommandManager.InvokeCommand(deathCommand);   
            
        }

        private void View_OnDeathAnimationEnd()
        {
            Dispose();
        }

        private void Gloabal_OnMonsterDeathConfirmed(MonsterDeathConfirmedCommand e)
        {
            UnityEngine.Debug.Log("death confirmed for: "+ e.monster +" " + e.monster.Team);
            if (slimeModel.Target.Value==e.monster){
                slimeModel.Target.Value = null;
                slimeModel.MonterState.Value = MonsterState.Moving;
                
                UnityEngine.Debug.Log(Team+": target is " + slimeModel.Target.Value );
            }
        }

        private void Model_OnHealthChanged(int oldValue, int newValue)
        {
            if (newValue <= 0)
            {
                if (slimeModel.MonterState.Value != MonsterState.Dying)
                {
                   HendleDeath();
                }
            }
            Context.CommandManager.InvokeCommand(new MonsterHealthChangedCommand(){amount = slimeModel.Health.Value});
        }

        private void HendleDeath()
        {
            slimeModel.MonterState.Value=MonsterState.Dying;
            slimeModel.Health.Value=0;

            var deathCommand = new MonsterDeadCommand(){monster = this};

            Context.CommandManager.InvokeCommand(deathCommand);
            globalContext.CommandManager.InvokeCommand(deathCommand);            
        }

        private void Model_OnAttackRangeChanged(float oldValue, float newValue)
        {
            Context.CommandManager.InvokeCommand(new MonsterAttackRangeCommand(){attackRange=newValue});
        }

        private void View_OnTargetHit(IMonster monster)
        {
            Context.CommandManager.InvokeCommand(new MonsterAttackStartCommand());
            monster.RecieveDamage(Damage);
            UnityEngine.Debug.Log(monster.Team+": "+monster.Health);
        }

        private void Model_TargetChanged(IMonster oldValue, IMonster newValue)
        {
            Context.CommandManager.InvokeCommand(new MonsterTargetCommand(){target = newValue});
        }

        private void Service_OnMonsterFound(IMonster monster)
        {
            slimeModel.Target.Value = monster;
            slimeModel.MonterState.Value = MonsterState.Battling;
        }

        private void Model_OnMonsterStateChandged(MonsterState oldValue, MonsterState newValue)
        {
            monsterStateChangedCommand.monsterState = newValue;
            Context.CommandManager.InvokeCommand(monsterStateChangedCommand);
            CheckSlimeFinder(newValue);
        }

        private void CheckSlimeFinder(MonsterState newValue)
        {
            
            //if state is moving, than find
            //othervise, no
            if (newValue == MonsterState.Moving){

                switch (Team)
                {
                    case Team.Red:
                        slimeFinder.StartSearching(this, monstersList.BlueMonsters, AgroRange);
                        break;
                    case Team.Blue:
                        slimeFinder.StartSearching(this, monstersList.RedMonsters, AgroRange);
                        break;
                }
            }
            else{
                slimeFinder.enabled=false;
            }
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