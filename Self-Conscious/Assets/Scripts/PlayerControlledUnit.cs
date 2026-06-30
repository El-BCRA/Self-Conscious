using UnityEngine;
using System.Collections;

namespace SelfConscious
{
    public class PlayerControlledUnit : Unit
    {
        [Header("Battle Data")]
        [SerializeField] private AbilityClass abilityClass;
        [SerializeField] private BattlePosition battlePosition;

        [Header("Abilities")]
        [SerializeField] private AbilityData[] attackAbilities;
        [SerializeField] private AbilityData[] defenseAbilities;
        [SerializeField] private AbilityData[] supportAbilities;

        [Header("Sprites")]
        [SerializeField] private SpriteRenderer idleSprite;
        [SerializeField] private SpriteRenderer activeSprite;
        [SerializeField] private SpriteRenderer downedSprite;

        private bool downed = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BattleManager.Instance.AddToContextualUI(targetingSelection);
            nameText.text = unitName;
            HideName();
            SetIdle();
            if (currentHP == 0)
            {
                SetDowned();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void SetCurrentHP(int val)
        {
            currentHP = val;

            if (val != 0 && downed)
            {
                downed = false;
                SetIdle();
            }

            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            else if (currentHP <= 0)
            {
                SetDowned();
                currentHP = 0;
            }
        }

        public void SetActive()
        {
            if (!downed)
            {
                activeSprite.color = new Color(1f, 1f, 1f, 1f);
                idleSprite.color = new Color(1f, 1f, 1f, 0f);
                downedSprite.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        public void SetIdle()
        {
            if (!downed)
            {
                activeSprite.color = new Color(1f, 1f, 1f, 0f);
                idleSprite.color = new Color(1f, 1f, 1f, 1f);
                downedSprite.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        public void SetDowned()
        {
            downed = true;
            downedSprite.color = new Color(1f, 1f, 1f, 1f);
            activeSprite.color = new Color(1f, 1f, 1f, 0f);
            idleSprite.color = new Color(1f, 1f, 1f, 0f);
            BattleManager.Instance.CheckLose();
        }

        public BattlePosition GetBattlePosition()
        {
            return battlePosition;
        }

        public void SetBattlePosition(BattlePosition position)
        {
            battlePosition = position;
        }

        public bool GetDowned()
        {
            return downed;
        }

        public AbilityData[] GetAttackAbilities()
        {
            return attackAbilities;
        }

        public AbilityData[] GetDefenseAbilities()
        {
            return defenseAbilities;
        }

        public AbilityData[] GetSupportAbilities()
        {
            return supportAbilities;
        }

        public AbilityClass GetUnitClass()
        {
            return abilityClass;
        }

        public override void UpdateShieldIcons()
        {
            battlePosition.UpdateUI();
        }

        public override void UpdateResourceModUI()
        {
            battlePosition.UpdateUI();
        }

        public override IEnumerator AttackWindup()
        {
            homePosition = transform.position;
            float counter = 0;
            while (counter < 0.5)
            {
                transform.position = transform.position - new Vector3((float)(hitOffset / 0.5f) * Time.deltaTime, 0, 0);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(.5f);
            transform.position = homePosition;
        }
    }
}
