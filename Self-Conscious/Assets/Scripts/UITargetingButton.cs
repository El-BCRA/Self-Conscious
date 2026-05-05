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
            StartCoroutine(SelectionPulse());
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
