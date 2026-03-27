using TMPro;
using UnityEngine;

namespace SelfConscious
{
    public class UIAbilityButton : UIButton
    {
        [SerializeField] private AbilityData ability;
        [SerializeField] private TMP_Text abilityNameText;
        [SerializeField] private TMP_Text abilityDescriptionText;

        private void Awake()
        {
            BattleManager.instance.AddToAbilitiesUIList(this);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void ReplaceUIText()
        {
            abilityNameText.text = ability.abilityName;
            abilityDescriptionText.text = ability.abilityDescription;
        }

        public void SetAbility(AbilityData newAbility)
        {
            ability = newAbility;
        }
    }
}
