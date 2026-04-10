using TMPro;
using UnityEngine;

namespace SelfConscious
{
    public class EnemyUnit : Unit
    {
        [Header("Local UI")]
        [SerializeField] private TMP_Text HPText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BattleManager.Instance.AddToContextualUI(targetingSelection);
            nameText.text = unitName;
        }

        // Update is called once per frame
        void Update()
        {
            HPText.text = "HP: " + currentHP + "/" + maxHP;
        }
    }
}
