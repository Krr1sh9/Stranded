using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class ButtonInfo : MonoBehaviour
    {

        public int ItemID;
        public TextMeshProUGUI PriceTxt;
        public TextMeshProUGUI QuantityTxt;
        public GameObject ShopManager;
        public Item item;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            PriceTxt.text = ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();
            QuantityTxt.text = ShopManager.GetComponent<ShopManager>().shopItems[3, ItemID].ToString();

        }
    }
}
