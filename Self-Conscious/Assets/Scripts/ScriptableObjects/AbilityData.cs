using UnityEngine;
using System.Collections.Generic;

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
        MAXSOURCE,
        MAXTARGET,
        CURRENTSOURCE,
        CURRENTTARGET,
        MISSINGSOURCE,
        MISSINGTARGET
    }

    [CreateAssetMenu(fileName = "AbilityEffectData", menuName = "Scriptable Objects/AbilityEffectData")]
    public class AbilityEffectData : ScriptableObject
    {
        public TargetingType areaOfEffect;
        public AbilityType abilityEffect;
        [Tooltip("Should remain null unless this ability applies a condition.")]
        public ConditionData condition;
    }

    public class AbilityData : ScriptableObject
    {
        public string abilityName = "Ability";
        public TargetingType targetingType;
        [TextArea(2,5)]
        public string abilityDescription;
        public ResourceCost resourceCost;

        [Tooltip("Cost of the ability. In case of percentage-based costs, this is the percent-value.")]
        public int cost;
        public PercentScaleBase percentScaleBase;
        public List<AbilityEffectData> effectList;
    }
}
