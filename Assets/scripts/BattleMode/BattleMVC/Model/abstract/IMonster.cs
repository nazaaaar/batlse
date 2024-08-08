namespace nazaaaar.platformBattle.mini.model
{
    public interface IMonster: IDamagable{
       slime.mini.model.MonterState MonterState {get;}

       float AttackSpeed {get;}

       int Damage {get;}

       float MoveSpeed {get;}

       float AgroRange {get;}
       float Speed {get;}
    }
}
