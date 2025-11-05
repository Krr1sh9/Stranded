using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public class CraftingSystem : MonoBehaviour
    {

        public int GRID_SIZE = 3;

        public Item[,] items;
        // Start is called before the first frame update
        public CraftingSystem() {
            items = new Item[GRID_SIZE,GRID_SIZE];
        }
        
        private bool IsEmpty(int x,int y) {
            return items[x,y] == null;
        }

        private Item GetItem(int x, int y) {
            return items[x, y];
        }

        public void SetItem(Item item, int x, int y) {
            items[x, y] = item;
        
        }

        //public void IncreaseItemAmount(int x, int y) {
        //    GetItem(x, y).count++;
        //}

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
