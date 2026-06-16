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

        public void OnDefeat()
        {
            BattleManager.Instance.EnemyDefeat(this);
            Destroy(this.gameObject);
        }
    }
}
