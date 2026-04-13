using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject selectionHighlight;
        [SerializeField] private TMP_Text jitterText;
        protected bool selected = false;

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
            selected = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selectionHighlight.SetActive(false);
            selected = false;
        }

        public GameObject GetSelectionHighlight()
        {
            return selectionHighlight;
        }
        public IEnumerator TextJitter()
        {
            while (selected)
            {
                jitterText.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-2.5f, 2.5f));
                yield return new WaitForSeconds(.2f);
            }
        }
    }
}
