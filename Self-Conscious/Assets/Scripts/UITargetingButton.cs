using UnityEngine;
using System.Collections.Generic;

namespace SelfConscious
{
    public class UITargetingButton : UIButton
    {
        [SerializeField] private List<Unit> targets;

        public void OnTargetLock()
        {
            BattleManager.Instance.CacheTargets(targets);
        }
    }
}
