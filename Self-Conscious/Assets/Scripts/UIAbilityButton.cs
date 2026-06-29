using NUnit.Framework.Constraints;
using System.Collections;
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
        [SerializeField] private GameObject notEnoughResourcesIndicator; 

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            notEnoughResourcesIndicator.SetActive(false);
        }

        public void ReplaceUIText()
        {
            notEnoughResourcesIndicator.SetActive(false);
            abilityNameText.text = ability.abilityName;
            abilityDescriptionText.text = ability.abilityDescription;
            switch(ability.resourceCost)
            {
                case ResourceCost.HEALTHFLAT:
                    {
                        abilityCost.text = ability.cost + " HP";
                        break;
                    }
                case ResourceCost.HEALTHPERCENT:
                    {
                        // TODO: REVIEW: Implement percentage-based HP cost display (e.g. "20% HP")
                        abilityCost.text = (ability.cost * 100) + "% HP";
                        break;
                    }
                case ResourceCost.WILLPOWERFLAT:
                    {
                        abilityCost.text = ability.cost + " WP";
                        break;
                    }
                case ResourceCost.WILLPOWERPERCENT:
                    {
                        // TODO: REVIEW: Implement percentage-based WP cost display (e.g. "15% WP")
                        abilityCost.text = (ability.cost * 100) + "% WP";
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
            notEnoughResourcesIndicator.SetActive(false);
            StartCoroutine(TextJitter());
            StartCoroutine(SelectionPulse());
            ReplaceUIText();
        }

        public void TriggerTargetingUI()
        {
            StopAllCoroutines();
            switch (ability.resourceCost)
            {
                case ResourceCost.HEALTHFLAT:
                    {
                        if (BattleManager.Instance.GetActiveBattlePosition().GetUnit().GetCurrentHP() > ability.cost)
                        {
                            BattleManager.Instance.CacheAbility(ability);
                            BattleManager.Instance.OnAbilitySelect();
                        }
                        else
                        {
                            StartCoroutine(FlashNotEnough());
                        }
                        break;
                    }
                case ResourceCost.HEALTHPERCENT:
                    {
                        if (BattleManager.Instance.GetActiveBattlePosition().GetUnit().GetCurrentHP() > 
                            ability.cost * BattleManager.Instance.GetActiveBattlePosition().GetUnit().GetCurrentHP())
                        {
                            BattleManager.Instance.CacheAbility(ability);
                            BattleManager.Instance.OnAbilitySelect();
                        }
                        else
                        {
                            StartCoroutine(FlashNotEnough());
                        }
                        break;
                    }
                case ResourceCost.WILLPOWERFLAT:
                    {
                        if (BattleManager.Instance.GetActiveBattlePosition().GetUnit().GetCurrentWP() >= ability.cost)
                        {
                            BattleManager.Instance.CacheAbility(ability);
                            BattleManager.Instance.OnAbilitySelect();
                        }
                        else
                        {
                            StartCoroutine(FlashNotEnough());
                        }
                        break;
                    }
                case ResourceCost.WILLPOWERPERCENT:
                    {
                        if (BattleManager.Instance.GetActiveBattlePosition().GetUnit().GetCurrentHP() >= 
                            ability.cost * BattleManager.Instance.GetActiveBattlePosition().GetUnit().GetCurrentWP())
                        {
                            BattleManager.Instance.CacheAbility(ability);
                            BattleManager.Instance.OnAbilitySelect();
                        }
                        else
                        {
                            StartCoroutine(FlashNotEnough());
                        }
                        break;
                    }
                case ResourceCost.NONE:
                    {
                        BattleManager.Instance.CacheAbility(ability);
                        BattleManager.Instance.OnAbilitySelect();
                        break;
                    }
            }
        }

        public IEnumerator FlashNotEnough()
        {
            for (int i = 0; i < 3; i++)
            {
                notEnoughResourcesIndicator.SetActive(true);
                yield return new WaitForSeconds(.25f);
                notEnoughResourcesIndicator.SetActive(false);
                yield return new WaitForSeconds(.25f);
            }
        }
    }
}
