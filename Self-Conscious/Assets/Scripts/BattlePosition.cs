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
        [SerializeField] private Unit currentUnit;
        [SerializeField] private BattlePositionKind kind;

        public Unit GetUnit() { return currentUnit; }

        public void SetUnit(Unit unit)
        {
            currentUnit = unit;
        }

        public bool Occupied() { return currentUnit != null; }

        public void swapUnit(BattlePosition otherBP)
        {
            if (Occupied() && otherBP.Occupied())
            {
                Unit temp = currentUnit;
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
