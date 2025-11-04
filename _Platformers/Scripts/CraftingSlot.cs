//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Platformers
//{
//    public class CraftingSlot : CardManager
//    {
//        public CraftingManager craftingManager;
//        public Vector2Int positionInGrid;


//        public override bool SetItem(Item item)
//        {
//            if (isOccupied || item == null) return false;

//            this.item = item;
//            itemImage.sprite = item.image;

//            this.item.text = item.itemType.ToString();
//            this.isOccupied = true;

//            RefreshDisplay();

//            craftingManager.AddToCrafting(positionInGrid,item,this,false);

//            return true;
//        }

//        public override void UnsetItem()
//        {
//            item = null;
//            this.isOccupied = false;
//            craftingManager.AddToCrafting(positionInGrid, null, this, true);
//            RefreshDisplay();

//        }
//    }
//}
