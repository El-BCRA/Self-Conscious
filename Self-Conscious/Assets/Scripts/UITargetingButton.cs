using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    public class UITargetingButton : UIButton
    {
        [SerializeField] private List<Unit> targets;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            ShowName();
            StartCoroutine(SelectionPulse());
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            targets[0].HideName();
        }

        public void ShowName()
        {
            if (targets.Count == 1)
            {
                targets[0].ShowName();
            }
        }

        public void OnTargetLock()
        {
            BattleManager.Instance.CacheTargets(targets);
            BattleManager.Instance.OnTargetConfirm();
        }

        public void AddToTargets(Unit unit)
        {
            targets.Add(unit);
        }

        public void RemoveFromTargets(Unit unit)
        {
            if (targets.Contains(unit)) 
            {  
                targets.Remove(unit); 
            }
        }

        public void ClearTargets()
        {
            targets.Clear();
        }
    }
}
