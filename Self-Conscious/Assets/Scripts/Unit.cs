using TMPro;
using UnityEngine;

namespace SelfConscious
{
    public class Unit : MonoBehaviour
    {
        [Header("Local UI")]
        [SerializeField] protected TMP_Text nameText;
        [SerializeField] protected Sprite UIPortrait;
        [SerializeField] protected CanvasGroup targetingSelection;
        [SerializeField] protected UIButton targetingButton;

        [Header("Unit Data Fields")]
        [SerializeField] protected string unitName;
        [SerializeField] protected int unitLevel;
        [SerializeField] protected int maxHP;
        [SerializeField] protected int currentHP;
        [SerializeField] protected int maxWP;
        [SerializeField] protected int currentWP;

        #region GETTERS & SETTERS
        public Sprite GetPortrait()
        {
            return UIPortrait;
        }

        public CanvasGroup GetTargetingSelection()
        {
            return targetingSelection;
        }

        public GameObject GetSelectionHighlight()
        {
            return targetingButton.gameObject;
        }

        public string GetName()
        {
            return unitName;
        }

        public int GetMaxHP()
        {
            return maxHP;
        }

        public int GetCurrentHP()
        {
            return currentHP;
        }

        public virtual void SetCurrentHP(int val)
        {
            // Implement on children
        }

        public int GetMaxWP()
        {
            return maxWP;
        }

        public int GetCurrentWP()
        {
            return currentWP;
        }

        public void SetCurrentWP(int val)
        {
            currentWP = val;
            if (currentWP > maxWP)
            {
                currentWP = maxWP;
            } else if (currentWP < 0)
            {
                currentWP = 0;
            }
        }
        #endregion

        public void UseAbilty(AbilityData ability)
        {
            switch (ability.resourceCost)
            {
                case ResourceCost.HEALTH:
                    {
                        SetCurrentHP(currentHP - ability.cost);
                        break;
                    }
                case ResourceCost.WILLPOWER:
                    {
                        SetCurrentWP(currentWP - ability.cost);
                        break;
                    }
                case ResourceCost.NONE:
                    {
                        break;
                    }
            }
        }

        public void ApplyAbility(AbilityData ability, Unit source)
        {
            switch (ability.abilityEffect)
            {
                case AbilityType.HEALTHMOD:
                    {
                        SetCurrentHP(currentHP - ability.modAmount);
                        break;
                    }
                case AbilityType.HEALTHDRAIN:
                    {
                        SetCurrentHP(currentHP - ability.modAmount);
                        source.SetCurrentHP(source.GetCurrentHP() + (ability.cost * ability.drainMultiplier));
                        break;
                    }
                case AbilityType.HEALTHDONATE:
                    {
                        SetCurrentHP(currentHP + ability.modAmount);
                        break;
                    }
                case AbilityType.WILLMOD:
                    {
                        SetCurrentWP(currentWP - ability.modAmount);
                        break;
                    }
                case AbilityType.WILLDRAIN:
                    {
                        SetCurrentHP(currentHP - ability.modAmount);
                        source.SetCurrentWP(source.GetCurrentWP() + (ability.cost * ability.drainMultiplier));
                        break;
                    }
                case AbilityType.WILLDONATE:
                    {
                        SetCurrentWP(currentWP + ability.modAmount);
                        break;
                    }
                case AbilityType.STATMOD:
                    {
                        // TODO: COME BACK TO THIS AT SOME POINT
                        break;
                    }
                case AbilityType.CONDITION:
                    {
                        // TODO: COME BACK TO THIS AT SOME POINT TOO
                        break;
                    }
            }

            // Check for death
            if (currentHP < 0)
            {
                currentHP = 0;
                // TODO: Other shit which means "the unit is dead"
            }
        }
    }
}
