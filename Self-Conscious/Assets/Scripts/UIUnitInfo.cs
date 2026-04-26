using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelfConscious
{
    public class UIUnitInfo : MonoBehaviour
    {
        [Header("Name, Position & Portrait")]
        [SerializeField] private TMP_Text battlePositionText;
        [SerializeField] private BattlePosition referencedBattlePosition;
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private Image battlePortrait;

        [Header("Health & Willpower")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text willpowerText;
        [SerializeField] private RectTransform healthBar;
        [SerializeField] private RectTransform willpowerBar;
        [SerializeField] private float barFullWidth = 0.78f;

        [Header("SelectionAnimation")]
        [SerializeField] private float startScale = 1.1f;
        [SerializeField] private float selectScale = 1.2f;
        [SerializeField] private float jitterAngle = 2.5f;
        [SerializeField] private float jitterDelay = 0.2f;
        [SerializeField] private float minJitterDelta = 0.5f;
        private bool selected = false;

        private PlayerControlledUnit referencedUnit;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            battlePositionText.text = referencedBattlePosition.GetBPKind().ToString();
            referencedUnit = referencedBattlePosition.GetUnit();
            battlePortrait.sprite = referencedUnit.GetPortrait();
            selected = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetActive()
        {
            transform.localScale = new Vector3(selectScale, selectScale, selectScale);
            selected = true;
            StartCoroutine(Jitter());
        }

        public void SetInactive()
        {
            transform.localScale = new Vector3(startScale, startScale, startScale);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            selected = false;
        }

        public void UpdateUIBars()
        {
            float ratio = (float) referencedUnit.GetCurrentHP() / referencedUnit.GetMaxHP();
            healthBar.localScale = new Vector3(ratio, 1, 1);
            ratio = (float) referencedUnit.GetCurrentWP() / referencedUnit.GetMaxWP();
            willpowerBar.localScale = new Vector3(ratio, 1, 1);
        }

        public void UpdateUI()
        {
            referencedUnit = referencedBattlePosition.GetUnit();
            battlePortrait.sprite = referencedUnit.GetPortrait();
            unitNameText.text = referencedUnit.GetName();
            healthText.text = "" + referencedUnit.GetCurrentHP() + "/" + referencedUnit.GetMaxHP();
            willpowerText.text = "" + referencedUnit.GetCurrentWP() + "/" + referencedUnit.GetMaxWP();

            UpdateUIBars();
        }

        public IEnumerator Jitter()
        {
            while (selected)
            {
                float newAngle = Random.Range(-jitterAngle, jitterAngle);
                if (Mathf.Abs(newAngle - transform.localEulerAngles.z) < minJitterDelta)
                {
                    newAngle += minJitterDelta;
                }
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newAngle);
                yield return new WaitForSeconds(jitterDelay);
            }
        }
    }
}
