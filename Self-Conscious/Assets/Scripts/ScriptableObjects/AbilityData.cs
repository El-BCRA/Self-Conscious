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
        ALLUNITS,
        SELF,
        NONE,
    }

    public enum AbilityType
    {
        HPLOSE,
        EXTENDEDHPLOSE,
        MAXHPLOSE,
        HPGAIN,
        EXTENDEDHPGAIN,
        MAXHPGAIN,
        WPLOSE,
        EXTENDEDWHPLOSE,
        MAXWPLOSE,
        WPGAIN,
        EXTENDEDWPGAIN,
        MAXWPGAIN,
        RESOURCESWAP,
        ADDSHIELD,
        BATTLESWAP,
        APPLYCONDITION
    }

    public enum ResourceCost
    {
        WILLPOWERFLAT,
        WILLPOWERPERCENT,
        HEALTHFLAT,
        HEALTPERCENT,
        NONE
    }

    public enum PercentScaleBase
    {
        NONE,
        MAX,
        CURRENT,
        MISSING
    }

    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        public TargetingType targetingType;
        public AbilityType abilityEffect;
        public string abilityName = "Ability";

        [TextArea(2,5)]
        public string abilityDescription;
        public ResourceCost resourceCost;

        [Tooltip("Cost of the ability. In case of percentage-based costs, this is the percent-value.")]
        public int cost;
        public PercentScaleBase percentScaleBase;
        
        [Tooltip("Should remain null unless this ability applies a condition.")]
        public ConditionData condition;
    }
}
