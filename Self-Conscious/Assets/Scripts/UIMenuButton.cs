using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace SelfConscious
{
    public class UIMenuButton : UIButton
    {
        [SerializeField] private string selectionDetailsText;
        [SerializeField] private TMP_Text choiceDescriptionText;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            StartCoroutine(TextJitter());
            StartCoroutine(SelectionPulse());
            if (choiceDescriptionText != null)
            {
                choiceDescriptionText.text = selectionDetailsText;
            }
        }
    }
}
