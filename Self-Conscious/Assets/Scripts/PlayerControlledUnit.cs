using UnityEngine;

namespace SelfConscious
{
    public class PlayerControlledUnit : Unit
    {
        [SerializeField] private int maxWP;
        [SerializeField] private int currentWP;
        [SerializeField] private AbilityClass abilityClass;

        [Header("Abilities")]
        [SerializeField] private AbilityData[] attackAbilities;
        [SerializeField] private AbilityData[] defenseAbilities;
        [SerializeField] private AbilityData[] supportAbilities;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BattleManager.Instance.AddToContextualUI(targetingSelection);
            nameText.text = unitName;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public AbilityData[] GetAttackAbilities()
        {
            return attackAbilities;
        }

        public AbilityData[] GetDefenseAbilities()
        {
            return defenseAbilities;
        }

        public AbilityData[] GetSupportAbilities()
        {
            return supportAbilities;
        }

        public AbilityClass GetUnitClass()
        {
            return abilityClass;
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
        }
    }
}
