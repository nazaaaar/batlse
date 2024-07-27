using System;

namespace nazaaaar.platformBattle.mini.model
{
    public class ShopCard
    {
        public ShopCardSO shopCardSO;

        private bool couldBeChanged;

        public bool CouldBeChanged { get => couldBeChanged; set { couldBeChanged = value;}}
    }
}
