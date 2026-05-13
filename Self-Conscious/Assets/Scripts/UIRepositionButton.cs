using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SelfConscious
{
    public class UIRepositionButton : UIButton
    {
        [Header("Reposition Details")]
        [SerializeField] private TMP_Text repostionLabel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private PlayerControlledUnit unit;
        [SerializeField] private SpriteRenderer battlePosition;
        [SerializeField] private List<Sprite> battlePositionIcons;

        public void Start()
        {
            battlePosition.sprite = null;
            repostionLabel.gameObject.SetActive(false);
        }

        public void OnTargetLock()
        {
            BattleManager.Instance.CacheSwap(unit.GetUnitClass());
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

        public void UpdateBattleStationUI()
        {
            if (unit == null || unit.GetBattlePosition() == null)
            {
                battlePosition.sprite = null;
            } else
            {
                switch (unit.GetBattlePosition().GetBPKind())
                {
                    case BattlePositionKind.ATTACKFRONT:
                        {
                            battlePosition.sprite = battlePositionIcons[0];
                            break;
                        }
                    case BattlePositionKind.ATTACKBACK:
                        {
                            battlePosition.sprite = battlePositionIcons[1];
                            break;
                        }
                    case BattlePositionKind.DEFENSE:
                        {
                            battlePosition.sprite = battlePositionIcons[2];
                            break;
                        }
                    case BattlePositionKind.SUPPORT:
                        {
                            battlePosition.sprite = battlePositionIcons[3];
                            break;
                        }
                }
            }
        }

        public AbilityClass GetAbilityClass()
        {
            return unit.GetUnitClass();
        }

        public CanvasGroup GetCanvasGroup()
        {
            return canvasGroup;
        }
    }
}
