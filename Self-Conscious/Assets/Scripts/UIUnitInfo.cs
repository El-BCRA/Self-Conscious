using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelfConscious
{
    public class UIUnitInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private TMP_Text battlePositionText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text willpowerText;
        [SerializeField] private Image battlePortrait;
        [SerializeField] private BattlePosition referencedBattlePosition;
        private PlayerControlledUnit referencedUnit;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            battlePositionText.text = referencedBattlePosition.GetBPKind().ToString();
            referencedUnit = referencedBattlePosition.GetUnit();
            battlePortrait.sprite = referencedUnit.GetPortrait();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void UpdateUI()
        {
            referencedUnit = referencedBattlePosition.GetUnit();
            battlePortrait.sprite = referencedUnit.GetPortrait();
            unitNameText.text = referencedUnit.GetName();
            healthText.text = "" + referencedUnit.GetCurrentHP() + "/" + referencedUnit.GetMaxHP();
            willpowerText.text = "" + referencedUnit.GetCurrentWP() + "/" + referencedUnit.GetMaxWP();
        }
    }
}
