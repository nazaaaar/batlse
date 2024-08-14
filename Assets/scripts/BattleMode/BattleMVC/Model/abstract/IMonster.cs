namespace nazaaaar.platformBattle.mini.model
{
    public interface IMonster: IDamagable{
       slime.mini.model.MonsterState MonterState {get;}

       float AttackSpeed {get;}

       int Damage {get;}
       int Health{get;}

       float MoveSpeed {get;}

       float AgroRange {get;}
       float Speed {get;}
       UnityEngine.Vector3 Position {get;}

       float AttackRange { get; }
    }
}
