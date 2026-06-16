using UnityEngine;

namespace SelfConscious
{
    public enum ResourceOverTime
    {
        DAMAGE,
        HEAL,
        DRAIN,
        REPLENISH
    }

    public struct ResourceModOverTime
    {
        [SerializeField] private float modAmount;
        [SerializeField] private uint modLifetime;
        [SerializeField] PercentScaleBase percentScaleBase;
        [SerializeField] ResourceOverTime resourceType;

        public ResourceModOverTime(float amount, uint lifetime, PercentScaleBase baseType, ResourceOverTime type)
        {
            modAmount = amount;
            modLifetime = lifetime;
            percentScaleBase = baseType;
            resourceType = type;
        }

        public int GetScaledAmount(Unit target)
        {
            int scaledAmount = 0;
            switch (percentScaleBase)
            {
                case PercentScaleBase.CURRENTTARGET:
                    {
                        if (resourceType == ResourceOverTime.DAMAGE || resourceType == ResourceOverTime.HEAL)
                        {
                            scaledAmount = (int)(target.GetCurrentHP() * modAmount);
                        } else if (resourceType == ResourceOverTime.DRAIN || resourceType == ResourceOverTime.REPLENISH)
                        {
                            scaledAmount = (int)(target.GetCurrentWP() * modAmount);
                        }
                        break;   
                    }
                case PercentScaleBase.CURRENTSOURCE:
                    {
                        if (resourceType == ResourceOverTime.DAMAGE || resourceType == ResourceOverTime.HEAL)
                        {
                            scaledAmount = (int)(target.GetCurrentHP() * modAmount);
                        } else if (resourceType == ResourceOverTime.DRAIN || resourceType == ResourceOverTime.REPLENISH)
                        {
                            scaledAmount = (int)(target.GetCurrentWP() * modAmount);
                        }
                        break;   
                    }
                case PercentScaleBase.MAXTARGET:
                    {
                        if (resourceType == ResourceOverTime.DAMAGE || resourceType == ResourceOverTime.HEAL)
                        {
                            scaledAmount = (int)(target.GetMaxHP() * modAmount);
                        } else if (resourceType == ResourceOverTime.DRAIN || resourceType == ResourceOverTime.REPLENISH)
                        {
                            scaledAmount = (int)(target.GetMaxWP() * modAmount);
                        }
                        break;   
                    }
                case PercentScaleBase.MAXSOURCE:
                    {
                        if (resourceType == ResourceOverTime.DAMAGE || resourceType == ResourceOverTime.HEAL)
                        {
                            scaledAmount = (int)(target.GetMaxHP() * modAmount);
                        } else if (resourceType == ResourceOverTime.DRAIN || resourceType == ResourceOverTime.REPLENISH)
                        {
                            scaledAmount = (int)(target.GetMaxWP() * modAmount);
                        }
                        break;   
                    }   
                case PercentScaleBase.MISSINGTARGET:
                    {
                        if (resourceType == ResourceOverTime.DAMAGE || resourceType == ResourceOverTime.HEAL)
                        {
                            scaledAmount = (int)((target.GetMaxHP() - target.GetCurrentHP()) * modAmount);
                        } else if (resourceType == ResourceOverTime.DRAIN || resourceType == ResourceOverTime.REPLENISH)
                        {
                            scaledAmount = (int)((target.GetMaxWP() - target.GetCurrentWP()) * modAmount);
                        }
                        break;
                    }
                case PercentScaleBase.MISSINGSOURCE:
                    {
                        if (resourceType == ResourceOverTime.DAMAGE || resourceType == ResourceOverTime.HEAL)
                        {
                            scaledAmount = (int)((target.GetMaxHP() - target.GetCurrentHP()) * modAmount);
                        } else if (resourceType == ResourceOverTime.DRAIN || resourceType == ResourceOverTime.REPLENISH)
                        {
                            scaledAmount = (int)((target.GetMaxWP() - target.GetCurrentWP()) * modAmount);
                        }
                        break;
                    }
                case PercentScaleBase.NONE:
                    {
                        scaledAmount = (int)modAmount;
                        break;
                    }
            }
            return scaledAmount;
        }

        public float GetModAmount()
        {
            return modAmount;
        }

        public void SetModAmount(float amount)
        {
            modAmount = amount;
        }

        public uint GetModLifetime()
        {
            return modLifetime;
        }

        public void SetModLifetime(uint lifetime)
        {
            modLifetime = lifetime;
        }

        public PercentScaleBase GetPercentScaleBase()
        {
            return percentScaleBase;
        }

        public void SetPercentScaleBase(PercentScaleBase baseType)
        {
            percentScaleBase = baseType;
        }

        public ResourceOverTime GetResourceType()
        {
            return resourceType;
        }

        public void SetResourceType(ResourceOverTime type)
        {
            resourceType = type;
        }
    }
   
}