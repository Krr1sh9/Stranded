using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class FloatingHealthbar : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Slider slider;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform targets;


        public void UpdateHealthbar(float currentValue,float maxValue) {

            slider.value = currentValue / maxValue;


        }
  
        // Update is called once per frame
        void Update()
        {
            
            transform.parent.rotation = mainCamera.transform.rotation;
            transform.position = targets.position + new Vector3(0, 0.8f, 0);

        }
    }
}
