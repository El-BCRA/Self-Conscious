using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject selectionHighlight;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            selectionHighlight.SetActive(false);
        }

        public void ResetSelectionHighlight()
        {
            selectionHighlight.SetActive(false);
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            selectionHighlight.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selectionHighlight.SetActive(false);
        }

        public GameObject GetSelectionHighlight()
        {
            return selectionHighlight;
        }
    }
}
