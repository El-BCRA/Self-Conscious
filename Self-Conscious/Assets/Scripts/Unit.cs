using TMPro;
using UnityEngine;

namespace SelfConscious
{
    public class Unit : MonoBehaviour
    {
        public string unitName;
        public int unitLevel;

        public int damage;

        public int maxHP;
        public int currentHP;

        public int maxWP;
        public int currentWP;

        [SerializeField] private TMP_Text nameText;

        void Start()
        {
            nameText.text = unitName;
        }
    }
}
