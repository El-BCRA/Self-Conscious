using TMPro;
using UnityEngine;
using System.Collections;

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
        [SerializeField] protected int unitLevel;
        [SerializeField] protected int maxHP;
        [SerializeField] protected int currentHP;
        [SerializeField] protected int maxWP;
        [SerializeField] protected int currentWP;

        [Header("Battle Data Fields")]
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
                        SetCurrentHP(currentHP - ability.cost);
                        break;
                    }
                case ResourceCost.HEALTPERCENT:
                    {
                        // TODO: Implement percentage-based HP cost
                        break;
                    }
                case ResourceCost.WILLPOWERFLAT:
                    {
                        SetCurrentWP(currentWP - ability.cost);
                        break;
                    }
                case ResourceCost.WILLPOWERPERCENT:
                    {
                        // TODO: Implement percentage-based WP cost
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
                        SetCurrentHP(currentHP - ability.cost);
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
                        SetCurrentHP(currentHP + ability.cost);
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
                        SetCurrentWP(currentWP - ability.cost);
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
                        SetCurrentWP(currentWP + ability.cost);
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
                        // TODO: Implement ADDSHIELD (e.g. add a damage reduction shield)
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
        }

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
