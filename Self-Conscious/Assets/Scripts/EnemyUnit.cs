using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace SelfConscious
{
    public class EnemyUnit : Unit
    {
        [Header("Local UI")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private RectTransform healthBar;
        [SerializeField] protected List<GameObject> shieldIcons = new List<GameObject>();
        [SerializeField] protected TMP_Text shieldText;
        [SerializeField] protected TMP_Text shieldPlusText;
        [SerializeField] protected GameObject damageOverTimeIcon;
        [SerializeField] protected GameObject healOverTimeIcon;
        [SerializeField] protected GameObject drainOverTimeIcon;
        [SerializeField] protected GameObject replenishOverTimeIcon;
        [SerializeField] protected TMP_Text damageOverTimeText;
        [SerializeField] protected TMP_Text healOverTimeText;
        [SerializeField] protected TMP_Text drainOverTimeText;
        [SerializeField] protected TMP_Text replenishOverTimeText;

        [Header("Abilities")]
        [SerializeField] private AbilityData[] abilities;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BattleManager.Instance.AddToContextualUI(targetingSelection);
            nameText.text = unitName;
            shieldText.text = "";
            shieldPlusText.text = "";
            nameText.gameObject.SetActive(false);
            healthText.text = GetMaxHP().ToString();
            UpdateShieldIcons();
            UpdateResourceModUI();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #region UI
        public void UpdateUIBars()
        {
            float ratio = (float)GetCurrentHP() / GetMaxHP();
            healthBar.localScale = new Vector3(ratio, 1, 1);
        }

        public override void UpdateShieldIcons()
        {
            if (shieldStacks.Count > 0)
            {
                shieldText.text = shieldStacks[0].ToString();
                shieldPlusText.text = "+";
            }
            else
            {
                shieldText.text = "";
                shieldPlusText.text = "";
            }
            for (int i = 0; i < shieldIcons.Count; i++)
            {
                if (i < shieldStacks.Count)
                {
                    shieldIcons[i].SetActive(true);
                } else
                {
                    shieldIcons[i].SetActive(false);
                }
            }
        }

        public override void UpdateResourceModUI()
        {
            if (damageOverTime.GetModLifetime() > 0)
            {
                damageOverTimeIcon.SetActive(true);
                damageOverTimeText.text = damageOverTime.GetModLifetime().ToString();
            } else {
                damageOverTimeIcon.SetActive(false);
                damageOverTimeText.text = "";
            }

            if (healOverTime.GetModLifetime() > 0)
            {
                healOverTimeIcon.SetActive(true);
                healOverTimeText.text = healOverTime.GetModLifetime().ToString();
            } else {
                healOverTimeIcon.SetActive(false);
                healOverTimeText.text = "";
            }

            if (drainOverTime.GetModLifetime() > 0)
            {
                drainOverTimeIcon.SetActive(true);
                drainOverTimeText.text = drainOverTime.GetModLifetime().ToString();
            } else {
                drainOverTimeIcon.SetActive(false);
                drainOverTimeText.text = "";
            }

            if (replenishOverTime.GetModLifetime() > 0)
            {
                replenishOverTimeIcon.SetActive(true);
                replenishOverTimeText.text = replenishOverTime.GetModLifetime().ToString();
            } else {
                replenishOverTimeIcon.SetActive(false);
                replenishOverTimeText.text = "";
            }
        }
        #endregion

        #region BATTLE FLOW
        public override void SetCurrentHP(int val)
        {
            healthText.text = val.ToString();

            currentHP = val;
            UpdateUIBars();
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            else if (currentHP <= 0)
            {
                currentHP = 0;
                OnDefeat();
            }
        }

        public AbilityData GetPureRandomAbility()
        {
            if (abilities.Length > 0)
            {
                return abilities[Random.Range(0, abilities.Length)];
            }
            else
            {
                return null;
            }
        }

        public AbilityData GetWeightedRandomAbility()
        {
            if (abilities.Length > 0)
            {
                float totalWeight = 0;
                foreach (AbilityData ability in abilities)
                {
                    totalWeight += ability.cost;
                }

                float randomPoint = Random.Range(0f, totalWeight);
                float currentPoint = 0;
                foreach (AbilityData ability in abilities)
                {
                    currentPoint += ability.cost;
                    if (randomPoint <= currentPoint)
                    {
                        return ability;
                    }
                }

                // Fallback in case of rounding errors
                return abilities[abilities.Length - 1];
            }
            else
            {
                return null;
            }
        }

        public Unit SelectTarget(List<PlayerControlledUnit> possibleTargets)
        {
            if (possibleTargets.Count > 0)
            {
                // Get the total missing HP ratio of all possible targets
                float totalPartyHealthRatio = 0;
                foreach (Unit unit in possibleTargets)                
                {
                    totalPartyHealthRatio += unit.GetMissingHPRatio();
                }

                // Select a random point within that total ratio
                float randomPoint = Random.Range(0f, totalPartyHealthRatio);

                // Iterate through the possible targets and select the one that corresponds to the random point
                float currentPoint = 0;
                foreach (Unit unit in possibleTargets)
                {
                    currentPoint += unit.GetMissingHPRatio();
                    if (randomPoint <= currentPoint)                    
                    {
                        return unit;
                    }
                }

                // Fallback in case of rounding errors
                return possibleTargets[possibleTargets.Count - 1];
            }
            else
            {
                return null;
            }
        }
        #endregion

        public void OnDefeat()
        {
            BattleManager.Instance.EnemyDefeat(this);
            Destroy(this.gameObject);
        }
    }
}
