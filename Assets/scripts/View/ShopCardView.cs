using nazaaaar.platformBattle.mini.model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nazaaaar.platformBattle.mini.view
{
    public class ShopCardView: MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI descriotion;
        [SerializeField]
        private TextMeshProUGUI cost;
        [SerializeField]
        private Image image;
        [SerializeField]
        private Button button;

        
        public Image Image { get => image; set => image = value; }
        public TextMeshProUGUI Title { get => title; set => title = value; }
        public TextMeshProUGUI Descriotion { get => descriotion; set => descriotion = value; }
        public TextMeshProUGUI Cost { get => cost; set => cost = value; }
        public bool Interactable {get{
            return button.interactable;
        }
        set{
            button.interactable = value;
        }
        }

        public void ConfigWithShopCard(ShopCard shopCard){
            this.image.sprite = shopCard.shopCardSO.image;
            this.title.text = shopCard.shopCardSO.title;
            this.Descriotion.text = shopCard.shopCardSO.description;
            this.cost.text = shopCard.shopCardSO.cost.ToString();
            this.Interactable = shopCard.CouldBeChanged;
            Debug.Log(shopCard.shopCardSO.title + " " + shopCard.CouldBeChanged);
        }
    }
}