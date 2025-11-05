using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Platformers
{
    public class XPTracker : MonoBehaviour
    {


        [SerializeField] BaseXPTranslations XPTranslationType;
        BaseXPTranslations XPTranslation;
        [SerializeField] TextMeshProUGUI CurrentLevelText;
        [SerializeField] TextMeshProUGUI CurrentXPText;
        [SerializeField] TextMeshProUGUI XPToNextLevelText;
        [SerializeField] UnityEvent<int,int> OnLevelChanged = new UnityEvent<int, int> ();

        // Start is called before the first frame update

        private void Awake()
        {
            XPTranslation = ScriptableObject.Instantiate(XPTranslationType);
        }
        
        public void AddXP(int amount)
        {
            int previousLevel = XPTranslation.CurrentLevel;
            if (XPTranslation.AddXP(amount)) {
                OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
            }
            UpdateUI();
        }

        public void SetLevel(int level) {
            int previousLevel = XPTranslation.CurrentLevel;

            XPTranslation.SetLevel(level);
            if (previousLevel != XPTranslation.CurrentLevel) {
                OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
            }
            UpdateUI();
        }
        void Start()
        {
            UpdateUI();
            OnLevelChanged.Invoke(0, XPTranslation.CurrentLevel);
        }

        private void UpdateUI()
        {
            CurrentLevelText.text = "Level: " + XPTranslation.CurrentLevel;
            CurrentXPText.text = "XP: " + XPTranslation.CurrentXP;
            if (!XPTranslation.IsMaxLevel)
            {
                XPToNextLevelText.text = "XP to next level: " + XPTranslation.XPToNextLevel;
              }
            else
            {
                XPToNextLevelText.text = "XP to next level: MAX LEVEL";
            }
        }
    }
}
