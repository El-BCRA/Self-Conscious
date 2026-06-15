using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace SelfConscious
{
    public class Unit : MonoBehaviour
    {
        [Header("Local UI")]
        [SerializeField] protected TMP_Text nameText;
        [SerializeField] protected Sprite UIPortrait;
        [SerializeField] protected GameObject characterSprites;
        [SerializeField] protected CanvasGroup targetingSelection;
        [SerializeField] protected UIButton targetingButton;

        [Header("Unit Data Fields")]
        [SerializeField] protected string unitName;
        [SerializeField] protected int maxHP;
        [SerializeField] protected int currentHP;
        [SerializeField] protected int maxWP;
        [SerializeField] protected int currentWP;

        [Header("Battle Data Fields")]
        protected List<uint> shieldStacks = new List<uint>();
        [SerializeField] protected float hitJitter = 0.3f;
        [SerializeField] protected float hitOffset = 0.5f;

        #region GETTERS & SETTERS
        public Sprite GetPortrait()
        {
            return UIPortrait;
        }

        public CanvasGroup GetTargetingSelection()
        {
            return targetingSelection;
        }

        public GameObject GetSelectionHighlight()
        {
            return targetingButton.gameObject;
        }

        public GameObject GetNameText()
        {
            return nameText.gameObject;
        }

        public virtual void ShowName()
        {
            nameText.gameObject.SetActive(true);
        }

        public virtual void HideName()
        {
            if (nameText != null)
            {
                nameText.gameObject.SetActive(false);
            }
        }

        public string GetName()
        {
            return unitName;
        }

        public int GetMaxHP()
        {
            return maxHP;
        }

        public int GetCurrentHP()
        {
            return currentHP;
        }

        public virtual void SetCurrentHP(int val)
        {
            // Implement on children
        }

        public int GetMaxWP()
        {
            return maxWP;
        }

        public int GetCurrentWP()
        {
            return currentWP;
        }

        public void SetCurrentWP(int val)
        {
            currentWP = val;
            if (currentWP > maxWP)
            {
                currentWP = maxWP;
            } else if (currentWP < 0)
            {
                currentWP = 0;
            }
        }

        public List<uint> GetShieldStacks()
        {
            return shieldStacks;
        }
        
        public float GetHitAnimationTime()
        {
            return hitJitter * 2;
        }
        #endregion

        public void UseAbilty(AbilityData ability)
        {
            switch (ability.resourceCost)
            {
                case ResourceCost.HEALTHFLAT:
                    {
                        SetCurrentHP(currentHP - (int)ability.cost);
                        break;
                    }
                case ResourceCost.HEALTHPERCENT:
                    {
                        int cost = 0;
                        switch (ability.percentScaleBase)
                        {
                            case PercentScaleBase.NONE:
                                {
                                    cost = (int) ability.cost;
                                    break;
                                }
                            case PercentScaleBase.MAXSOURCE:
                                {
                                    cost = (int)(ability.cost * maxHP);
                                    break;
                                }
                            case PercentScaleBase.MAXTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using MAXTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * maxHP);
                                    break;
                                }
                            case PercentScaleBase.CURRENTSOURCE:
                                {
                                    cost = (int)(ability.cost * currentHP);
                                    break;
                                }
                            case PercentScaleBase.CURRENTTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using CURRENTTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * currentHP);
                                    break;
                                }
                            case PercentScaleBase.MISSINGSOURCE:
                                {
                                    cost = (int)(ability.cost * (maxHP - currentHP));
                                    break;
                                }
                            case PercentScaleBase.MISSINGTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using MISSINGTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * (maxHP - currentHP));
                                    break;
                                }
                        }
                        SetCurrentHP(currentHP - cost);
                        break;
                    }
                case ResourceCost.WILLPOWERFLAT:
                    {
                        SetCurrentWP(currentWP - (int)ability.cost);
                        break;
                    }
                case ResourceCost.WILLPOWERPERCENT:
                    {
                        int cost = 0;
                        switch (ability.percentScaleBase)
                        {
                            case PercentScaleBase.NONE:
                                {
                                    cost = (int)ability.cost;
                                    break;
                                }
                            case PercentScaleBase.MAXSOURCE:
                                {
                                    cost = (int)(ability.cost * maxWP);
                                    break;
                                }
                            case PercentScaleBase.MAXTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using MAXTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * maxWP);
                                    break;
                                }
                            case PercentScaleBase.CURRENTSOURCE:
                                {
                                    cost = (int)(ability.cost * currentWP);
                                    break;
                                }
                            case PercentScaleBase.CURRENTTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using CURRENTTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * currentWP);
                                    break;
                                }
                            case PercentScaleBase.MISSINGSOURCE:
                                {
                                    cost = (int)(ability.cost * (maxWP - currentWP));
                                    break;
                                }
                            case PercentScaleBase.MISSINGTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using MISSINGTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * (maxWP - currentWP));
                                    break;
                                }
                        }
                        SetCurrentWP(currentWP - cost);
                        break;
                    }
                case ResourceCost.NONE:
                    {
                        break;
                    }
            }
        }

        #region  ABILITY FLOW
        public void ApplyAbility(AbilityData ability, Unit source)
        {
            foreach (AbilityEffectData effect in ability.effectList)
            {
                switch (effect.abilityEffect)
                {
                    case EffectType.HPLOSE:
                        {
                            int dmgModifier = 0;
                            int dmg = 0;
                            ApplyShieldStacks(dmgModifier);
                            switch (effect.percentScaleBase)
                            {
                                case PercentScaleBase.NONE:
                                    {
                                        dmg = (int)effect.value - dmgModifier;
                                        break;
                                    }
                                case PercentScaleBase.MAXSOURCE:
                                    {
                                        dmg = (int)((effect.value * source.maxHP) - dmgModifier);
                                        break;
                                    }
                                case PercentScaleBase.MAXTARGET:
                                    {
                                        dmg = (int)((effect.value * maxHP) - dmgModifier);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTSOURCE:
                                    {
                                        dmg = (int)((effect.value * source.currentHP) - dmgModifier);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTTARGET:
                                    {
                                        dmg = (int)((effect.value * currentHP) - dmgModifier);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGSOURCE:
                                    {
                                        dmg = (int)((effect.value * (source.maxHP - source.currentHP)) - dmgModifier);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        dmg = (int)((effect.value * (maxHP - currentHP)) - dmgModifier);
                                        break;
                                    }
                            }
                            if (dmg > 0)
                            {
                                SetCurrentHP(currentHP - dmg);
                            }
                            break;
                        }
                    case EffectType.EXTENDEDHPLOSE:
                        {
                            // TODO: Implement extended HP loss (e.g. damage over time)
                            break;
                        }
                    case EffectType.MAXHPLOSE:
                        {
                            // TODO: Implement MAXHPLOSE (e.g. reduce max HP and current HP accordingly)
                            break;
                        }
                    case EffectType.HPGAIN:
                        {
                            int healAmount = 0;
                            switch (effect.percentScaleBase)
                            {
                                case PercentScaleBase.NONE:
                                    {
                                        healAmount = (int)effect.value;
                                        break;
                                    }
                                case PercentScaleBase.MAXSOURCE:
                                    {
                                        healAmount = (int)(source.maxHP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MAXTARGET:
                                    {
                                        healAmount = (int)(maxHP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTSOURCE:
                                    {
                                        healAmount = (int)(source.currentHP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTTARGET:
                                    {
                                        healAmount = (int)(currentHP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGSOURCE:
                                    {
                                        healAmount = (int)((source.maxHP - source.currentHP) * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        healAmount = (int)((maxHP - currentHP) * effect.value);
                                        break;
                                    }
                            }
                            SetCurrentHP(currentHP + healAmount);
                            break;
                        }
                    case EffectType.EXTENDEDHPGAIN:
                        {
                            // TODO: Implement extended HP gain (e.g. heal over time)
                            break;
                        }
                    case EffectType.MAXHPGAIN:
                        {
                            // TODO: Implement MAXHPGAIN (e.g. increase max HP and current HP accordingly)
                            break;
                        }
                    case EffectType.WPLOSE:
                        {
                            int drainAmount = 0;
                            switch (effect.percentScaleBase)
                            {
                                case PercentScaleBase.NONE:
                                    {
                                        drainAmount = (int)effect.value;
                                        break;
                                    }
                                case PercentScaleBase.MAXSOURCE:
                                    {
                                        drainAmount = (int)(source.maxWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MAXTARGET:
                                    {
                                        drainAmount = (int)(maxWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTSOURCE:
                                    {
                                        drainAmount = (int)(source.currentWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTTARGET:
                                    {
                                        drainAmount = (int)(currentWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGSOURCE:
                                    {
                                        drainAmount = (int)((source.maxWP - source.currentWP) * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        drainAmount = (int)((maxWP - currentWP) * effect.value);
                                        break;
                                    }
                            }
                            SetCurrentWP(currentWP - drainAmount);
                            break;
                        }
                    case EffectType.EXTENDEDWHPLOSE:
                        {
                            // TODO: Implement extended WP loss (e.g. willpower drain over time)
                            break;
                        }
                    case EffectType.MAXWPLOSE:
                        {
                            // TODO: Implement MAXWPLOSE (e.g. reduce max WP and current WP accordingly)
                            break;                    
                        }
                    case EffectType.WPGAIN:
                        {
                            int replenishAmount = 0;
                            switch (effect.percentScaleBase)
                            {
                                case PercentScaleBase.NONE:
                                    {
                                        replenishAmount = (int)effect.value;
                                        break;
                                    }
                                case PercentScaleBase.MAXSOURCE:
                                    {
                                        replenishAmount = (int)(source.maxWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MAXTARGET:
                                    {
                                        replenishAmount = (int)(maxWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTSOURCE:
                                    {
                                        replenishAmount = (int)(source.currentWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.CURRENTTARGET:
                                    {
                                        replenishAmount = (int)(currentWP * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGSOURCE:
                                    {
                                        replenishAmount = (int)((source.maxWP - source.currentWP) * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        replenishAmount = (int)((maxWP - currentWP) * effect.value);
                                        break;
                                    }
                            }
                            SetCurrentWP(currentWP + replenishAmount);
                            break;
                        }
                    case EffectType.EXTENDEDWPGAIN:
                        {
                            // TODO: Implement extended WP gain (e.g. willpower regeneration over time)
                            break;
                        }
                    case EffectType.MAXWPGAIN:
                        {
                            // TODO: Implement MAXWPGAIN (e.g. increase max WP and current WP accordingly)
                            break;
                        }
                    case EffectType.RESOURCESWAP:
                        {
                            int tempHP = currentHP;
                            int tempWP = currentWP;
                            SetCurrentHP(tempWP);
                            SetCurrentWP(tempHP);
                            break;
                        }
                    case EffectType.ADDSHIELD:
                        {
                            shieldStacks.Add((uint)effect.value);
                            UpdateShieldIcons();
                            break;
                        }
                    case EffectType.BATTLESWAP:
                        {
                            // TODO: Implement BATTLESWAP (e.g. swap positions with another unit as a combat action)
                            break;                    
                        }
                    case EffectType.APPLYCONDITION:
                        {
                            // TODO: Implement APPLYCONDITION (e.g. apply a status effect)
                            break;
                        }
                }   
            }
            StartCoroutine(Impact());
        }
        #endregion

        #region SHIELD LOGIC
        public int ApplyShieldStacks(int dmgModifier)
        {
            if (shieldStacks.Count > 0)
            {
                dmgModifier -= (int)shieldStacks[0];
                shieldStacks.RemoveAt(0);
            }
            UpdateShieldIcons();
            return dmgModifier;
        }

        public virtual void UpdateShieldIcons()
        {
            // Implemented on children since the way shield icons are displayed 
            // may differ between player-controlled units and enemies
        }
        #endregion
        
        public virtual IEnumerator Impact()
        {
            characterSprites.transform.position = characterSprites.transform.position - new Vector3(hitOffset, 0, 0);
            yield return new WaitForSeconds(hitJitter/2);
            characterSprites.transform.position = characterSprites.transform.position + new Vector3(hitOffset, 0, 0);
            yield return new WaitForSeconds(hitJitter/2);
            characterSprites.transform.position = characterSprites.transform.position - new Vector3(hitOffset, 0, 0);
            yield return new WaitForSeconds(hitJitter);
            characterSprites.transform.position = characterSprites.transform.position + new Vector3(hitOffset, 0, 0);
        }
    }
}
