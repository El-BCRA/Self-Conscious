using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    public class UIAbilityButton : UIButton
    {
        [SerializeField] private AbilityData ability;
        [SerializeField] private TMP_Text abilityNameText;
        [SerializeField] private TMP_Text abilityDescriptionText;
        [SerializeField] private TMP_Text abilityCost;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void ReplaceUIText()
        {
            abilityNameText.text = ability.abilityName;
            abilityDescriptionText.text = ability.abilityDescription;
            switch(ability.resourceCost)
            {
                case ResourceCost.HEALTH:
                    {

                        abilityCost.text = ability.cost + " HP";
                        break;
                    }
                case ResourceCost.WILLPOWER:
                    {

                        abilityCost.text = ability.cost + " WP";
                        break;
                    }
                case ResourceCost.NONE:
                    {
                        abilityCost.text = "";
                        break;
                    }
            }
        }

        public void SetAbility(AbilityData newAbility)
        {
            ability = newAbility;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            StartCoroutine(TextJitter());
            StartCoroutine(SelectionPulse());
            ReplaceUIText();
        }

        public void TriggerTargetingUI()
        {
            BattleManager.Instance.CacheAbility(ability);
            BattleManager.Instance.OnAbilitySelect();
        }
    }
}
