using TMPro;
using UnityEngine;
using System.Collections;

namespace SelfConscious
{
    public class EnemyUnit : Unit
    {
        [Header("Local UI")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private RectTransform healthBar;

        [Header("Abilities")]
        [SerializeField] private AbilityData[] abilities;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BattleManager.Instance.AddToContextualUI(targetingSelection);
            nameText.text = unitName;
            nameText.gameObject.SetActive(false);
            healthText.text = GetMaxHP().ToString();
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
