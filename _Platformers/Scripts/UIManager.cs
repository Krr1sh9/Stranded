//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Platformers
//{
//    public class UIManager : MonoBehaviour
//    {
//        // A reference to the panel we will be controlling
//        public DropItUI itemDetailsPanel;
        

//        // OnEnable is called when the object becomes active
//        public void OnEnable()
//        {
//            // Start listening for the broadcast from ANY InventoryItem
//            InventoryItem.OnItemClicked += HandleItemClicked;
//        }

//        // OnDisable is called when the object becomes inactive or is destroyed
//        private void OnDisable()
//        {
//            // IMPORTANT: Always unsubscribe from static events to prevent memory leaks and errors
//            InventoryItem.OnItemClicked -= HandleItemClicked;
//        }

//        /// <summary>
//        /// This is our "listener" function. It will be executed whenever the OnItemClicked event is broadcast.
//        /// </summary>
//        /// <param name="item">The item data that was sent with the broadcast.</param>
//        private void HandleItemClicked(Item item)
//        {
//            // We received the broadcast! Now, tell the details panel to show itself
//            // and display the data we received.
//            itemDetailsPanel.Show(item);
//        }
//    }
//}
