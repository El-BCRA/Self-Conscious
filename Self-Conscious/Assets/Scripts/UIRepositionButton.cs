using UnityEngine;

namespace SelfConscious
{
    public class UIRepositionButton : UIButton
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private AbilityClass abilityClass;

        public void OnTargetLock()
        {
            BattleManager.Instance.CacheSwap(abilityClass);
            BattleManager.Instance.OnRepositionConfirm();
        }

        public AbilityClass GetAbilityClass()
        {
            return abilityClass;
        }

        public CanvasGroup GetCanvasGroup()
        {
            return canvasGroup;
        }
    }
}
