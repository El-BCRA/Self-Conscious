using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    public class UIRepositionButton : UIButton
    {
        [Header("Reposition Details")]
        [SerializeField] private TMP_Text repostionLabel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private AbilityClass abilityClass;

        public void Start()
        {
            repostionLabel.gameObject.SetActive(false);
        }

        public void OnTargetLock()
        {
            BattleManager.Instance.CacheSwap(abilityClass);
            BattleManager.Instance.OnRepositionConfirm();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            repostionLabel.gameObject.SetActive(true);
            StartCoroutine(TextJitter());
            StartCoroutine(SelectionPulse());
        }

        public new void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            repostionLabel.gameObject.SetActive(false);
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
