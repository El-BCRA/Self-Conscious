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

    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        public TargetingType targetingType;
        public AbilityType abilityEffect;
        public string abilityName;
        public string abilityDescription;
    }
}
