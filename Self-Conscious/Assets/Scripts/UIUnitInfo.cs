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
        [SerializeField] private Image battlePosition;
        [SerializeField] private List<Sprite> battlePositionIcons;

        [Header("Health & Willpower")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text willpowerText;
        [SerializeField] private RectTransform healthBar;
        [SerializeField] private RectTransform willpowerBar;
        [SerializeField] private TMP_Text shieldText;
        [SerializeField] private TMP_Text shieldPlusText;
        [SerializeField] protected List<GameObject> shieldIcons = new List<GameObject>();

        [Header("Resource Mods Over Time")]
        [SerializeField] protected GameObject damageOverTimeIcon;
        [SerializeField] protected GameObject healOverTimeIcon;
        [SerializeField] protected GameObject drainOverTimeIcon;
        [SerializeField] protected GameObject replenishOverTimeIcon;
        [SerializeField] protected TMP_Text damageOverTimeText;
        [SerializeField] protected TMP_Text healOverTimeText;
        [SerializeField] protected TMP_Text drainOverTimeText;
        [SerializeField] protected TMP_Text replenishOverTimeText;

        [Header("Selection Animation")]
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
            shieldPlusText.text = "";
            shieldText.text = "";
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

            switch(referencedBattlePosition.GetBPKind())
            {
                case BattlePositionKind.ATTACKFRONT:
                    {
                        battlePosition.sprite = battlePositionIcons[0];
                        break;
                    }
                case BattlePositionKind.ATTACKBACK:
                    {
                        battlePosition.sprite = battlePositionIcons[1];
                        break;
                    }
                case BattlePositionKind.DEFENSE:
                    {
                        battlePosition.sprite = battlePositionIcons[2];
                        break;
                    }
                case BattlePositionKind.SUPPORT:
                    {
                        battlePosition.sprite = battlePositionIcons[3];
                        break;
                    }
            }

            UpdateUIBars();
            UpdateShieldIcons();
            UpdateResourceMods();
        }

        public void UpdateShieldIcons()
        {
            if (referencedUnit.GetShieldStacks().Count > 0)
            {
                shieldPlusText.text = "+";
                shieldText.text = referencedUnit.GetShieldStacks()[0].ToString();
            } else
            {
                shieldText.text = "";
                shieldPlusText.text = "";
            }

            for (int i = 0; i < shieldIcons.Count; i++)
            {
                if (i < referencedUnit.GetShieldStacks().Count)
                {
                    shieldIcons[i].SetActive(true);
                } else
                {
                    shieldIcons[i].SetActive(false);
                }
            }
        }

        public void UpdateResourceMods()
        {
            if (referencedUnit.GetRMOT(ResourceOverTime.DAMAGE).GetModLifetime() > 0)
            {
                damageOverTimeIcon.SetActive(true);
                damageOverTimeText.text = referencedUnit.GetRMOT(ResourceOverTime.DAMAGE).GetModLifetime().ToString();
            } else
            {
                damageOverTimeIcon.SetActive(false);
                damageOverTimeText.text = "";
            }

            if (referencedUnit.GetRMOT(ResourceOverTime.HEAL).GetModLifetime() > 0)
            {
                healOverTimeIcon.SetActive(true);
                healOverTimeText.text = referencedUnit.GetRMOT(ResourceOverTime.HEAL).GetModLifetime().ToString();
            } else
            {
                healOverTimeIcon.SetActive(false);
                healOverTimeText.text = "";
            }

            if (referencedUnit.GetRMOT(ResourceOverTime.DRAIN).GetModLifetime() > 0)
            {
                drainOverTimeIcon.SetActive(true);
                drainOverTimeText.text = referencedUnit.GetRMOT(ResourceOverTime.DRAIN).GetModLifetime().ToString();
            } else
            {
                drainOverTimeIcon.SetActive(false);
                drainOverTimeText.text = "";
            }

            if (referencedUnit.GetRMOT(ResourceOverTime.REPLENISH).GetModLifetime() > 0)
            {
                replenishOverTimeIcon.SetActive(true);
                replenishOverTimeText.text = referencedUnit.GetRMOT(ResourceOverTime.REPLENISH).GetModLifetime().ToString();
            } else
            {
                replenishOverTimeIcon.SetActive(false);
                replenishOverTimeText.text = "";
            }
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
