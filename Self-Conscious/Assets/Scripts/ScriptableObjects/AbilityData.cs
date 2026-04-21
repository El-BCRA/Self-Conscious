using UnityEngine;

namespace SelfConscious
{
    public enum AbilityClass
    {
        PROTAGONIST,
        GIANT,
        NERD,
        PRINCESS,
        PERFORMER,
        SCAREDYCAT,
        MASTERMIND,
        BULLY,
        ITEM
    }

    public enum TargetingType
    {
        ENEMYSINGLE,
        ENEMYALL,
        ALLYSINGLE,
        ALLYALL,
        NONE,
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
        NONE,
        WILLPOWER,
        HEALTH
    }

    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        public TargetingType targetingType;
        public AbilityType abilityEffect;
        public ResourceCost resourceCost;
        public string abilityName = "Ability";
        public string abilityDescription;

        public int cost;
        public int modAmount;
        public int drainMultiplier;
    }
}
