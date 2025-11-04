using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemsSO : ScriptableObject 
    {

        new public string name = "New Item";    
        public Sprite icon = null;

        public virtual void Use()
        {
            // Use the item
            // Something may happen
            Debug.Log("Using " + name);
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
