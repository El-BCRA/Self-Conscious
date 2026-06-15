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
