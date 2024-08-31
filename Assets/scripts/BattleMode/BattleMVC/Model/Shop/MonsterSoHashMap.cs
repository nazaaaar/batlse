using System.Collections.Generic;

namespace nazaaaar.platformBattle.mini.model
{
    public class MonsterSOHashMap
    {
        private Dictionary<string, MonsterSO> _data;

        public MonsterSOHashMap()
        {
            _data = new Dictionary<string, MonsterSO>();
        }

         public MonsterSOHashMap(MonsterSO[] cards)
    {
        _data = new Dictionary<string, MonsterSO>();
        foreach (var card in cards)
        {
            AddCard(card);
        }
    }

        public Dictionary<string, MonsterSO> Data { get => _data; set => _data = value; }

        public void AddCard(MonsterSO card)
        {
            string id = card.Id;
            _data[id] = card;
        }

        public MonsterSO SearchByID(string id)
        {
            if (_data.ContainsKey(id))
            {
                return _data[id];
            }
            else
            {
                return null;
            }
        }
    }
}
