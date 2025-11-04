using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

namespace Platformers
{
    public class CraftButton : MonoBehaviour
    {
        // Start is called before the first frame update
        public CraftingManager craftingManager;

        public void ToggleCraftButton()
        {
            craftingManager.UpdateCrafting();
        }
    }
}
