using UnityEngine;

namespace SelfConscious
{
    public class PlayerControlledUnit : Unit
    {
        [SerializeField] private AbilityClass abilityClass;

        [Header("Abilities")]
        [SerializeField] private AbilityData[] attackAbilities;
        [SerializeField] private AbilityData[] defenseAbilities;
        [SerializeField] private AbilityData[] supportAbilities;

        [Header("Sprites")]
        [SerializeField] private SpriteRenderer idleSprite;
        [SerializeField] private SpriteRenderer activeSprite;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BattleManager.Instance.AddToContextualUI(targetingSelection);
            nameText.text = unitName;
            SetIdle();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetActive()
        {
            activeSprite.color = new Color(1f, 1f, 1f, 1f);
            idleSprite.color = new Color(1f, 1f, 1f, 0f);
        }

        public void SetIdle()
        {
            activeSprite.color = new Color(1f, 1f, 1f, 0f);
            idleSprite.color = new Color(1f, 1f, 1f, 1f);
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
    }
}
