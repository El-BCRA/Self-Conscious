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
        [SerializeField] protected List<uint> shieldStacks = new List<uint>();
        [SerializeField] protected float hitJitter = 0.3f;
        [SerializeField] protected float hitOffset = 0.5f;

        [Header("Resource Mod Over Time")]
        protected ResourceModOverTime damageOverTime;
        protected ResourceModOverTime healOverTime;
        protected ResourceModOverTime drainOverTime;
        protected ResourceModOverTime replenishOverTime;

        void Awake()
        {
            damageOverTime = new ResourceModOverTime(0, 0, PercentScaleBase.NONE, ResourceOverTime.DAMAGE);
            healOverTime = new ResourceModOverTime(0, 0, PercentScaleBase.NONE, ResourceOverTime.HEAL);
            drainOverTime = new ResourceModOverTime(0, 0, PercentScaleBase.NONE, ResourceOverTime.DRAIN);
            replenishOverTime = new ResourceModOverTime(0, 0, PercentScaleBase.NONE, ResourceOverTime.REPLENISH);
        }
        
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

        public int GetMissingHP()
        {
            return maxHP - currentHP;
        }

        public float GetHPRatio()
        {
            return (float)currentHP / maxHP;
        }

        public float GetMissingHPRatio()
        {
            return (float)(maxHP - currentHP) / maxHP;
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

        public int GetMissingWP()
        {
            return maxWP - currentWP;
        }

        public float GetWPRatio()
        {
            return (float)currentWP / maxWP;
        }

        public float GetMissingWPRatio()
        {
            return (float)(maxWP - currentWP) / maxWP;
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
        
        public ResourceModOverTime GetRMOT(ResourceOverTime resourceType)
        {
            switch (resourceType)
            {
                case ResourceOverTime.DAMAGE:
                    return damageOverTime;
                case ResourceOverTime.HEAL:
                    return healOverTime;
                case ResourceOverTime.DRAIN:
                    return drainOverTime;
                case ResourceOverTime.REPLENISH:
                    return replenishOverTime;
                default:
                    return new ResourceModOverTime(0, 0, PercentScaleBase.NONE, resourceType);
            }
        }

        public float GetHitAnimationTime()
        {
            return hitJitter * 2;
        }
        #endregion

        #region ABILITY FLOW
        public void UseAbility(AbilityData ability)
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
                                    cost = (int)(ability.cost * GetMissingHP());
                                    break;
                                }
                            case PercentScaleBase.MISSINGTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using MISSINGTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * GetMissingHP());
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
                                    cost = (int)(ability.cost * GetMissingWP());
                                    break;
                                }
                            case PercentScaleBase.MISSINGTARGET:
                                {
                                    // SHOULD NOT BE USED, THIS IS A FAILSAFE
                                    Debug.Log("An ability, " + ability + ", is using MISSINGTARGET as its ability cost PercentScaleBase. " +
                                        "This does not behave as expected and should be changed.");
                                    cost = (int)(ability.cost * GetMissingWP());
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
                            Debug.Log("Calling ApplyShieldStacks on a value of " + dmgModifier);
                            dmgModifier = ApplyShieldStacks();
                            Debug.Log("Returned a value of " + dmgModifier);
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
                                        dmg = (int)((effect.value * source.GetMissingHP()) - dmgModifier);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        dmg = (int)((effect.value * GetMissingHP()) - dmgModifier);
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
                            damageOverTime.SetModAmount(effect.value);
                            damageOverTime.SetPercentScaleBase(effect.percentScaleBase);
                            damageOverTime.SetModLifetime(damageOverTime.GetModLifetime() + 1);
                            UpdateResourceModUI();
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
                                        healAmount = (int)(source.GetMissingHP() * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        healAmount = (int)(GetMissingHP() * effect.value);
                                        break;
                                    }
                            }
                            SetCurrentHP(currentHP + healAmount);
                            break;
                        }
                    case EffectType.EXTENDEDHPGAIN:
                        {
                            healOverTime.SetModAmount(effect.value);
                            healOverTime.SetPercentScaleBase(effect.percentScaleBase);
                            healOverTime.SetModLifetime(healOverTime.GetModLifetime() + 1);
                            UpdateResourceModUI();
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
                                        drainAmount = (int)(source.GetMissingWP() * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        drainAmount = (int)(GetMissingWP() * effect.value);
                                        break;
                                    }
                            }
                            SetCurrentWP(currentWP - drainAmount);
                            break;
                        }
                    case EffectType.EXTENDEDWPLOSE:
                        {
                            drainOverTime.SetModAmount(effect.value);
                            drainOverTime.SetPercentScaleBase(effect.percentScaleBase);
                            drainOverTime.SetModLifetime(drainOverTime.GetModLifetime() + 1);
                            UpdateResourceModUI();
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
                                        replenishAmount = (int)(source.GetMissingWP() * effect.value);
                                        break;
                                    }
                                case PercentScaleBase.MISSINGTARGET:
                                    {
                                        replenishAmount = (int)(source.GetMissingWP() * effect.value);
                                        break;
                                    }
                            }
                            SetCurrentWP(currentWP + replenishAmount);
                            break;
                        }
                    case EffectType.EXTENDEDWPGAIN:
                        {
                            replenishOverTime.SetModAmount(effect.value);
                            replenishOverTime.SetPercentScaleBase(effect.percentScaleBase);
                            replenishOverTime.SetModLifetime(replenishOverTime.GetModLifetime() + 1);
                            UpdateResourceModUI();
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

        #region BATTLE FLOW
        public void StartTurn()
        {
            StartCoroutine(TurnStart());
        }
        #endregion

        #region SHIELD LOGIC
        public int ApplyShieldStacks()
        {
            int returnMod = 0;
            if (shieldStacks.Count > 0)
            {
                returnMod = (int)shieldStacks[0];
                shieldStacks.RemoveAt(0);
            }
            UpdateShieldIcons();
            return returnMod;
        }

        public virtual void UpdateShieldIcons()
        {
            // Implemented on children since the way shield icons are displayed 
            // may differ between player-controlled units and enemies
        }
        #endregion
        
        #region RESOURCE MOD OVER TIME LOGIC 
        public void TickResourceMods()
        {
            // Debug.Log("Ticking resource mods for " + unitName);
            if (damageOverTime.GetModLifetime() > 0)
            {
                // Debug.Log("Damage over time mod amount: " + damageOverTime.GetModAmount() + ", lifetime: " + damageOverTime.GetModLifetime());
                // Get base damage amount from the damage over time mod, scaled appropriately
                int dmg = (int)damageOverTime.GetScaledAmount(this);

                // Apply shield stacks to damage before applying damage over time
                if (shieldStacks.Count > 0)
                {
                    int dmgModifier = ApplyShieldStacks();
                    dmg = (int)damageOverTime.GetModAmount() - dmgModifier;
                }

                // Update current HP based on damage over time, and reduce mod lifetime by 1
                if (dmg > 0)
                {
                    SetCurrentHP(currentHP - dmg);
                }
                damageOverTime.SetModLifetime(damageOverTime.GetModLifetime() - 1);

                // If damage over time mod lifetime has reached 0, reset mod amount and percent scale base to defaults
                if (damageOverTime.GetModLifetime() == 0)
                {
                    damageOverTime.SetModAmount(0);
                    damageOverTime.SetPercentScaleBase(PercentScaleBase.NONE);
                }
            }

            if (healOverTime.GetModLifetime() > 0)
            {
                // Get base heal amount from the heal over time mod, scaled appropriately
                int heal = (int)healOverTime.GetScaledAmount(this);

                // Update current HP based on heal over time, and reduce mod lifetime by 1
                if (heal > 0)
                {
                    SetCurrentHP(currentHP + heal);
                }
                healOverTime.SetModLifetime(healOverTime.GetModLifetime() - 1);

                // If heal over time mod lifetime has reached 0, reset mod amount and percent scale base to defaults
                if (healOverTime.GetModLifetime() == 0)
                {
                    healOverTime.SetModAmount(0);
                    healOverTime.SetPercentScaleBase(PercentScaleBase.NONE);
                }
            }

            if (drainOverTime.GetModLifetime() > 0)
            {
                // Get base drain amount from the drain over time mod, scaled appropriately
                int drain = (int)drainOverTime.GetScaledAmount(this);
                
                // Update current WP based on drain over time, and reduce mod lifetime by 1
                if (drain > 0)
                {
                    SetCurrentWP(currentWP - drain);
                }
                drainOverTime.SetModLifetime(drainOverTime.GetModLifetime() - 1);
                
                // If drain over time mod lifetime has reached 0, reset mod amount and percent scale base to defaults
                if (drainOverTime.GetModLifetime() == 0)
                {
                    drainOverTime.SetModAmount(0);
                    drainOverTime.SetPercentScaleBase(PercentScaleBase.NONE);
                }
            }

            if (replenishOverTime.GetModLifetime() > 0)
            {                
                // Get base replenish amount from the replenish over time mod, scaled appropriately
                int replenish = (int)replenishOverTime.GetScaledAmount(this);

                // Update current WP based on replenish over time, and reduce mod lifetime by 1
                if (replenish > 0)
                {
                    SetCurrentWP(currentWP + replenish);
                }
                replenishOverTime.SetModLifetime(replenishOverTime.GetModLifetime() - 1);

                // If replenish over time mod lifetime has reached 0, reset mod amount and percent scale base to defaults
                if (replenishOverTime.GetModLifetime() == 0)
                {
                    replenishOverTime.SetModAmount(0);
                    replenishOverTime.SetPercentScaleBase(PercentScaleBase.NONE);
                }
            }
        }
        
        public virtual void UpdateResourceModUI()
        {
            // Implemented on children since the way resource mod over time UI is displayed 
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

        public virtual IEnumerator TurnStart()
        {
            TickResourceMods();
            UpdateResourceModUI();
            yield return null;
        }
    }
}
