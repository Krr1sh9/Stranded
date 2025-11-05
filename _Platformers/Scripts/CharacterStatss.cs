using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Platformers
{
    public class CharacterStatss : MonoBehaviour
    {
        [SerializeField] int StaminaToHealthConversion = 10;
        [SerializeField] int BaseStaminaPerLevel = 5;
        [SerializeField] int BaseStaminaOffset = 10;

        [SerializeField] int BaseDefensePerLevel = 3;
        [SerializeField] int BaseDefenseOffset = 2;

        [SerializeField] int BaseHealthPerLevel = 20;
        [SerializeField] int BaseHealthOffset = 100;

        [SerializeField] int BaseStrengthPerLevel = 2;
        [SerializeField] int BaseStrengthOffset = 5;
        [SerializeField] public TextMeshProUGUI StaminaText;
        [SerializeField] public TextMeshProUGUI HealthText;
        [SerializeField] public TextMeshProUGUI StrengthText;
        [SerializeField] public TextMeshProUGUI DefenseText;
        [SerializeField] EquipmentSlot[] EquipmentSlots;
        public int BaseStamina { get;  set; } = 0;
        public int BaseHealth { get; set; } = 0;

        public int BaseStrength { get; set; } = 0;

        public int BaseDefense { get; set; } = 0;

        public int Stamina
        {
            get
            {
                return BaseStamina;
            }

        }

        public int MaxHealth {
        
            get {
                return Stamina * StaminaToHealthConversion;
            }

        }

        public int Strength
        {
            get
            {
                return BaseStrength;
            }
        }

        public int Defense
        {
            get
            {
                return BaseDefense;
            }
        }

        public void OnUpdateLevel(int previousLevel,int currentLevel)
        {
            BaseStamina = BaseStaminaPerLevel * currentLevel + BaseStaminaOffset;
            StaminaText.text = $"Stamina: {Stamina}";
            HealthText.text = $"Health: {MaxHealth}";
            BaseStrength = BaseStrengthPerLevel * currentLevel + BaseStrengthOffset;
            StrengthText.text = $"Strength: {BaseDefense}";
            BaseDefense = BaseDefensePerLevel * currentLevel + BaseDefenseOffset;
            DefenseText.text = $"Defense: {BaseDefense}";
            Debug.Log($"Stats Updated! Current Stamina: {Stamina}, Current Health: {MaxHealth}, Current Strength: {Strength}, Current Defense: {Defense}");



        }
    }
}
