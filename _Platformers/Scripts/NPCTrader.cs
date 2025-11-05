using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformers
{
    

    public class NPCTrader : MonoBehaviour
    {
        public List<Item> itemsForSale = new List<Item>();

        // This could be triggered by player interaction (e.g., clicking on the NPC)
        public void BeginTrading()
        {
            Debug.Log("Starting trade with " + gameObject.name);
            // This is where you would open the trading UI
            // For now, we'll just log the items.
            foreach (var item in itemsForSale)
            {
                Debug.Log(item + " is for sale for " + item+ " gold.");
            }
        }

        public void BeginTradings()
        {
            // Find the UI Manager in the scene and call its method
            var tradeUIManager = FindFirstObjectByType<TradeUIManager>();
            if (tradeUIManager != null)
            {
                tradeUIManager.OpenTradeWindow(itemsForSale);
            }
            else
            {
                Debug.LogWarning("TradeUIManager not found in the scene.");
            }
        }
    }
}
