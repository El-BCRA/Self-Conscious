using UnityEngine;

namespace SelfConscious
{
    public enum TargetingType
    {
        ENEMYSINGLE,
        ENEMYALL,
        ALLYSINGLE,
        ALLYALL,
        ALLUNITS,
        SELF
    }

    public enum AbilityType
    {
        HEALTHMOD,
        HEALTHDRAIN,
        HEALTHDONATE,
        WILLMOD,
        WILLDRAIN,
        WILLDONATE,
        STATMOD,
        CONDITION
    }

    public enum ResourceCost
    {
        HEALTH,
        WILLPOWER,
        NONE
    }

    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        public TargetingType targetingType;
        public AbilityType abilityEffect;
        public ResourceCost resourceCost;
        public string abilityName;
        public string abilityDescription;

        public int cost;
        public int modAmount;
    }
}
