using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private GameObject selectionHighlight;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            button = GetComponent<Button>();
            selectionHighlight.SetActive(false);
        }

        public void OnSelect(BaseEventData eventData)
        {
            selectionHighlight.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selectionHighlight.SetActive(false);
        }
    }
}
