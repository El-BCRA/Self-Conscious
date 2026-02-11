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
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text willpowerText;

        void Start()
        {
            nameText.text = unitName;
            healthText.text = "HP: " + currentHP + "/" + maxHP;
            willpowerText.text = "WP: " + currentWP + "/" + maxWP;
        }

        void Update()
        {
            
        }
    }
}
