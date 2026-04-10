using TMPro;
using UnityEngine;

namespace SelfConscious
{
    public class Unit : MonoBehaviour
    {
        [Header("Local UI")]
        [SerializeField] protected TMP_Text nameText;
        [SerializeField] private Sprite UIPortrait;
        [SerializeField] private CanvasGroup targetingSelection;
        [SerializeField] private UIButton targetingButton;

        [Header("Unit Data Fields")]
        [SerializeField] protected string unitName;
        [SerializeField] protected int unitLevel;
        [SerializeField] protected int maxHP;
        [SerializeField] protected int currentHP;

        #region GETTERS & SETTERS
        public Sprite GetPortrait()
        {
            return UIPortrait;
        }

        public CanvasGroup GetTargetingSelection()
        {
            return targetingSelection;
        }

        public GameObject GetSelectionHighlight()
        {
            return targetingButton.GetSelectionHighlight();
        }

        public string GetName()
        {
            return unitName;
        }

        public int GetMaxHP()
        {
            return maxHP;
        }

        public int GetCurrentHP()
        {
            return currentHP;
        }

        public void SetCurrentHP(int val)
        {
            currentHP = val;
        }
        #endregion
    }
}
