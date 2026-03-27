using UnityEngine;

namespace SelfConscious
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        public string AbilityName;
        public string AbilityDescription;

    }
}
