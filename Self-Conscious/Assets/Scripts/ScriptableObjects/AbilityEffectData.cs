using UnityEngine;

namespace SelfConscious
{
    public enum EffectType
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

    [CreateAssetMenu(fileName = "AbilityEffectData", menuName = "Scriptable Objects/AbilityEffectData")]
    public class AbilityEffectData : ScriptableObject
    {
        public TargetingType areaOfEffect;
        public EffectType abilityEffect;
        public PercentScaleBase percentScaleBase;
        [Tooltip("For any effects which scale with some numerical value. If the PercentScaleBase " +
            "is any value besides NONE, this value should be treated as a percentage")]
        public float value;

        [Tooltip("Should remain null unless this ability applies a condition.")]
        public ConditionData condition;
    }
}
