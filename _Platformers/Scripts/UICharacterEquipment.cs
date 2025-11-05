//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Platformers
//{
//    public class UICharacterEquipment : MonoBehaviour
//    {
//        public EquipmentSlot[] equipmentSlots;

//        // Add these fields to the class to fix CS0103 errors
//        public EquipmentSlot helmetSlot;
//        public EquipmentSlot chestPlateSlot;
//        public EquipmentSlot leggingsSlot;
//        public EquipmentSlot bootsSlot;
//        public CharacterEquipment characterEquipment;

//        // Start is called before the first frame update
//        private void Awake()
//        {
//            equipmentSlots = GetComponentsInChildren<EquipmentSlot>();
//        }
        
//        void Start()
//        {
        
//        }

//        // Update is called once per frame
//        void Update()
//        {
        
//        }

//        public void SetCharacterEquipment() {
//            if (helmetSlot != null) {
//                helmetSlot.SetItem(characterEquipment.helmet);
//            }
//            if (chestPlateSlot != null) {
//                chestPlateSlot.SetItem(characterEquipment.chestPlate);
//            }
//            if (leggingsSlot != null) {
//                leggingsSlot.SetItem(characterEquipment.leggings);
//            }
//            if (bootsSlot != null) {
//                bootsSlot.SetItem(characterEquipment.boots);
//            }   
//        }
//        public void SetItem(CharacterEquipment equipment) {
//            this.characterEquipment = equipment;
//        }


//    }
//}
