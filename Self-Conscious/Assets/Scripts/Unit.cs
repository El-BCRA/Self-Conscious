using TMPro;
using UnityEngine;

namespace SelfConscious
{
    public class Unit : MonoBehaviour
    {
        [Header("Unit Data Fields")]
        [SerializeField] protected string unitName;
        [SerializeField] protected int unitLevel;
        [SerializeField] protected int maxHP;
        [SerializeField] protected int currentHP;
        [SerializeField] protected int maxWP;
        [SerializeField] protected int currentWP;

        [Header("Local UI")]
        [SerializeField] protected TMP_Text nameText;

        void Start()
        {
            nameText.text = unitName;
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

        public int GetMaxWP()
        {
            return maxWP;
        }

        public int GetCurrentWP()
        {
            return currentWP;
        }
    }
}
