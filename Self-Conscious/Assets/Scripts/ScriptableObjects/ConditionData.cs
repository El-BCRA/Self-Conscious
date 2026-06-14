using UnityEngine;

namespace SelfConscious
{
    public enum Condition
    {
        STRENGTHENED,
        TEMPSTRENGTHENED,
        WEAKENED,
        TEMPWEAKENED,
        GUARDED,
        TEMPGUARDED,
        VULNERABLE,
        TEMPVULNERABLE,
        IMMUNE,
        REDIRECT,
        STUNNED,
        ENTRANCED,
        LIGHTNINGROD,
        SPREAD,
        CONDENSE,
        OVERDRIVE,
        BRACED
    }

    public enum ConditionFamily
    {
        OFFENSE,
        DEFENSE,
        INTERRUPTION
    }

    [CreateAssetMenu(fileName = "ConditionData", menuName = "Scriptable Objects/ConditionData")]
    public class ConditionData : ScriptableObject
    {
        public Condition condition;
        public ConditionFamily conditionFamily;
    }
}
