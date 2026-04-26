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
        [SerializeField] private float jitterAngle = 5f;
        [SerializeField] private float jitterDelay = 0.2f;
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
            if (jitterText is not null)
            {
                jitterText.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        public GameObject GetSelectionHighlight()
        {
            return selectionHighlight;
        }

        public IEnumerator TextJitter()
        {
            while (selected)
            {
                float newAngle = Random.Range(-1 * jitterAngle, jitterAngle);
                if (Mathf.Abs(newAngle - jitterText.transform.localEulerAngles.z) < 1f)
                {
                    newAngle += 1f;
                }
                jitterText.transform.localEulerAngles = new Vector3(0, 0, newAngle);
                yield return new WaitForSeconds(jitterDelay);
            }
        }
    }
}
