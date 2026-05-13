using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("Text Jitter")]
        [SerializeField] private TMP_Text jitterText;
        [SerializeField] private float jitterAngle = 5f;
        [SerializeField] private float jitterDelay = 0.2f;

        [Header("Selection Highlight")]
        [SerializeField] private GameObject selectionHighlight;
        [SerializeField] private float startingScale;
        [SerializeField] private float scaleLeeway = .25f;
        [SerializeField] private float scaleMultiplier = 1f;
        protected bool selected = false;

        [Header("Pencil Scratch SFX")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private PencilScribble scribble;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            selectionHighlight.SetActive(false);
            startingScale = selectionHighlight.GetComponent<RectTransform>().localScale.x;
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

        public virtual void OnDeselect(BaseEventData eventData)
        {
            PlayPencilScribble();
            selectionHighlight.SetActive(false);
            selected = false;
            if (jitterText != null)
            {
                jitterText.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (selectionHighlight != null)
            {
                selectionHighlight.GetComponent<RectTransform>().localScale = new Vector3(startingScale, startingScale, startingScale);
            }
        }

        public void PlayPencilScribble()
        {
            audioSource.clip = scribble.GetClip();
            audioSource.Play();
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

        public IEnumerator SelectionPulse()
        {
            float timer = 0;
            while (selected)
            {
                timer += Time.deltaTime;
                float newScale = startingScale + (Mathf.Sin(timer * scaleMultiplier) * scaleLeeway);
                selectionHighlight.GetComponent<RectTransform>().localScale = new Vector3 (newScale, newScale, newScale);
                yield return null;
            }
        }
    }
}
