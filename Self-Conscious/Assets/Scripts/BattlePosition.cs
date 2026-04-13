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
        [SerializeField] private UIUnitInfo unitInfo;
        [SerializeField] private SpriteRenderer activeIndicator;

        private void Awake()
        {
            SetInactive();
        }

        void Start()
        {

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

        public void UpdateUI()
        {
            unitInfo.UpdateUI();
        }

        public PlayerControlledUnit GetUnit() { return currentUnit; }

        public void SetUnit(PlayerControlledUnit unit)
        {
            currentUnit = unit;
            unit.transform.position = transform.position;
            UpdateUI();
        }

        public bool Occupied() { return currentUnit != null; }

        public void SwapUnit(BattlePosition otherBP)
        {
            if (Occupied() && otherBP.Occupied())
            {
                PlayerControlledUnit temp = currentUnit;
                SetUnit(otherBP.GetUnit());
                otherBP.SetUnit(temp);
            } else if (Occupied())
            {
                otherBP.SetUnit(currentUnit);
                currentUnit = null;
            } else if (otherBP.Occupied())
            {
                SetUnit(otherBP.GetUnit());
                otherBP.SetUnit(null);
            } else
            {
                // Both are null why is this even running?
            }
        }
    }
}
