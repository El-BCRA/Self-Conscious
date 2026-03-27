using UnityEngine;

namespace SelfConscious
{
    public enum BattlePositionKind
    {
        ATTACKFRONT,
        ATTACKBACK,
        DEFENSE,
        SUPPORT
    }

    public class BattlePosition : MonoBehaviour
    {
        [SerializeField] private PlayerControlledUnit currentUnit;
        [SerializeField] private BattlePositionKind kind;
        [SerializeField] private SpriteRenderer activeIndicator;

        void Start()
        {
            SetInactive();
        }

        public BattlePositionKind GetBPKind()
        {
            return kind;
        }

        public void SetActive()
        {
            activeIndicator.enabled = true;
        }

        public void SetInactive()
        {
            activeIndicator.enabled = false;
        }

        public PlayerControlledUnit GetUnit() { return currentUnit; }

        public void SetUnit(PlayerControlledUnit unit)
        {
            currentUnit = unit;
        }

        public bool Occupied() { return currentUnit != null; }

        public void swapUnit(BattlePosition otherBP)
        {
            if (Occupied() && otherBP.Occupied())
            {
                PlayerControlledUnit temp = currentUnit;
                currentUnit = otherBP.GetUnit();
                otherBP.SetUnit(temp);
            } else if (Occupied())
            {
                otherBP.SetUnit(currentUnit);
                currentUnit = null;
            } else if (otherBP.Occupied())
            {
                currentUnit = otherBP.GetUnit();
                otherBP.SetUnit(null);
            } else
            {
                // Both are null why is this even running?
            }
        }
    }
}
