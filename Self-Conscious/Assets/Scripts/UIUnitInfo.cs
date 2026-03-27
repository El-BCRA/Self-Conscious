using TMPro;
using UnityEngine;

namespace SelfConscious
{
    public class UIUnitInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private TMP_Text battlePositionText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text willpowerText;
        [SerializeField] private BattlePosition referencedBattlePosition;
        private Unit referencedUnit;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            battlePositionText.text = referencedBattlePosition.GetBPKind().ToString();
            referencedUnit = referencedBattlePosition.GetUnit();
        }

        // Update is called once per frame
        void Update()
        {
            referencedUnit = referencedBattlePosition.GetUnit();
            unitNameText.text = referencedUnit.GetName();
            healthText.text = "" + referencedUnit.GetCurrentHP() + "/" + referencedUnit.GetMaxHP();
            willpowerText.text = "" + referencedUnit.GetCurrentWP() + "/" + referencedUnit.GetMaxWP();
        }
    }
}
