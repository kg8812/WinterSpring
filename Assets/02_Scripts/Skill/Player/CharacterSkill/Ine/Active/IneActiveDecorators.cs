public interface IIneActive : ISkill
{ 
    public IneActiveStat IneStat { get; }
    
}

public class IneActiveConfig : SkillConfig, IIneActive
{
    public IneActiveConfig(SkillStat stat) : base(stat)
    {
        if (stat is IneActiveStat ineStat)
        {
            IneStat = ineStat;
        }
        else
        {
            IneStat = null;
        }
    }

    public IneActiveStat IneStat { get; }
}

public class IneActiveAttachment : SkillAttachment, IIneActive
{
    public IneActiveAttachment(SkillStat stat) : base(stat)
    {
        if (stat is IneActiveStat ineStat)
        {
            IneStat = ineStat;
        }
        else
        {
            IneStat = null;
        }
    }

    public IneActiveStat IneStat { get; }
}

public class IneActiveDecorator : SkillDecorator,IIneActive
{
    public IneActiveStat IneStat
    {
        get
        {
            return config?.IneStat + attachment?.IneStat;
        }
    }

    private readonly IIneActive config;
    private readonly IIneActive attachment;

    public IneActiveDecorator(ISkill skill, ISkill attachment) : base(skill, attachment)
    {
        config = skill as IIneActive;
        this.attachment = attachment as IIneActive;
    }
}
public class IneActiveStat : SkillStat
{
    public int maxMana;
    public float manaGain;
    public float manaGainInSecond;
    public int circle1Count;
    public float circle1Radius;
    public int circle1groggy;
    public float circle2Mana;
    public float circle2Duration;
    public float circle2Radius;
    public int circle2groggy;
    public float circle2AtkFrequency;
    public float circle3Mana;
    public float circle3Radius;
    public int circle3groggy;
    public float meteorRadius;
    public float circle3ExpRadius;
    public int circle3ExpGroggy;
    public float highCircleDmgIncrement;
    
    public float maxManaRatio;
    public float manaGainRatio;
    public int circle1CountRatio;
    public float circle1RadiusRatio;
    public int circle1groggyRatio;
    public float circle2ManaRatio;
    public float circle2DurationRatio;
    public float circle2RadiusRatio;
    public int circle2groggyRatio;
    public float circle2AtkFrequencyRatio;
    public float circle3ManaRatio;
    public float circle3RadiusRatio;
    public int circle3groggyRatio;
    public float meteorRadiusRatio;
    public float circle3ExpRadiusRatio;
    public int circle3ExpGroggyRatio;
    public float highCircleDmgIncrementRatio;


    public IneActiveStat()
    {
        
    }
    public IneActiveStat(SkillStat stat)
    {
        baseCd = stat.baseCd;
        maxStack = stat.maxStack;
        duration = stat.duration;
        dmg = stat.dmg;
        groggy = stat.groggy;
        stackGain = stat.stackGain;
        baseCdRatio = stat.baseCdRatio;
        maxStackRatio = stat.maxStackRatio;
        durationRatio = stat.durationRatio;
        dmgRatio = stat.dmgRatio;
        groggyRatio = stat.groggyRatio;
    }
    public override void Reset()
    {
        base.Reset();
        manaGain = 0;
        manaGainInSecond = 0;
        circle1Count = 0;
        circle1Radius = 0;
        circle1groggy = 0;
        circle2Mana = 0;
        circle2Duration = 0;
        circle2Radius = 0;
        circle2groggy = 0;
        circle2AtkFrequency = 0;
        circle3Mana = 0;
        circle3Radius = 0;
        circle3groggy = 0;
        meteorRadius = 0;
        circle3ExpRadius = 0;
        circle3ExpGroggy = 0;
        highCircleDmgIncrement = 0;
        
        manaGainRatio = 0;
        circle1CountRatio = 0;
        circle1RadiusRatio = 0;
        circle1groggyRatio = 0;
        circle2ManaRatio = 0;
        circle2DurationRatio = 0;
        circle2RadiusRatio = 0;
        circle2groggyRatio = 0;
        circle2AtkFrequencyRatio = 0;
        circle3ManaRatio = 0;
        circle3RadiusRatio = 0;
        circle3groggyRatio = 0;
        meteorRadiusRatio = 0;
        circle3ExpRadiusRatio = 0;
        circle3ExpGroggyRatio = 0;
        highCircleDmgIncrementRatio = 0;
    }

    public override SkillStat Combine(SkillStat other)
    {
        if (other is IneActiveStat b)
        {
            IneActiveStat c = new()
            {
                baseCd = baseCd + other.baseCd,
                maxStack = maxStack + other.maxStack,
                duration = duration + other.duration,
                dmg = dmg + other.dmg,
                groggy = groggy + other.groggy,
                stackGain = stackGain + other.stackGain,
                baseCdRatio = baseCdRatio + other.baseCdRatio,
                maxStackRatio = maxStackRatio + other.maxStackRatio,
                durationRatio = durationRatio + other.durationRatio,
                dmgRatio = dmgRatio + other.dmgRatio,
                groggyRatio = groggyRatio + other.groggyRatio,

                manaGain = manaGain + b.manaGain,
                manaGainInSecond = manaGainInSecond + b.manaGainInSecond,
                maxMana = maxMana + b.maxMana,
                circle1Count = circle1Count + b.circle1Count,
                circle1Radius = circle1Radius + b.circle1Radius,
                circle1groggy = circle1groggy + b.circle1groggy,
                circle2Mana = circle2Mana + b.circle2Mana,
                circle2Duration = circle2Duration + b.circle2Duration,
                circle2Radius = circle2Radius + b.circle2Radius,
                circle2groggy = circle2groggy + b.circle2groggy,
                circle2AtkFrequency = circle2AtkFrequency + b.circle2AtkFrequency,
                circle3Mana = circle3Mana + b.circle3Mana,
                circle3Radius = circle3Radius + b.circle3Radius,
                circle3groggy = circle3groggy + b.circle3groggy,
                meteorRadius = meteorRadius + b.meteorRadius,
                circle3ExpRadius = circle3ExpRadius + b.circle3ExpRadius,
                circle3ExpGroggy = circle3ExpGroggy + b.circle3ExpGroggy,
                highCircleDmgIncrement = highCircleDmgIncrement + b.highCircleDmgIncrement,
                
                manaGainRatio = manaGainRatio + b.manaGainRatio,
                maxManaRatio = maxManaRatio + b.maxManaRatio,
                circle1CountRatio = circle1CountRatio + b.circle1CountRatio,
                circle1RadiusRatio = circle1RadiusRatio + b.circle1RadiusRatio,
                circle1groggyRatio = circle1groggyRatio + b.circle1groggyRatio,
                circle2ManaRatio = circle2ManaRatio + b.circle2ManaRatio,
                circle2DurationRatio = circle2DurationRatio + b.circle2DurationRatio,
                circle2RadiusRatio = circle2RadiusRatio + b.circle2RadiusRatio,
                circle2groggyRatio = circle2groggyRatio + b.circle2groggyRatio,
                circle2AtkFrequencyRatio = circle2AtkFrequencyRatio + b.circle2AtkFrequencyRatio,
                circle3ManaRatio = circle3ManaRatio + b.circle3ManaRatio,
                circle3RadiusRatio = circle3RadiusRatio + b.circle3RadiusRatio,
                circle3groggyRatio = circle3groggyRatio + b.circle3groggyRatio,
                meteorRadiusRatio = meteorRadiusRatio + b.meteorRadiusRatio,
                circle3ExpRadiusRatio = circle3ExpRadiusRatio + b.circle3ExpRadiusRatio,
                circle3ExpGroggyRatio = circle3ExpGroggyRatio + b.circle3ExpGroggyRatio,
                highCircleDmgIncrementRatio = highCircleDmgIncrementRatio + b.highCircleDmgIncrementRatio,
            };
            return c;
        }

        return new IneActiveStat(base.Combine(other));
    }

    public override SkillStat Subtract(SkillStat other)
    {
        
        if (other is IneActiveStat b)
        {
            IneActiveStat c = new()
            {
                baseCd = baseCd - other.baseCd,
                maxStack = maxStack - other.maxStack,
                duration = duration - other.duration,
                dmg = dmg - other.dmg,
                groggy = groggy - other.groggy,
                stackGain = stackGain - other.stackGain,
                baseCdRatio = baseCdRatio - other.baseCdRatio,
                maxStackRatio = maxStackRatio - other.maxStackRatio,
                durationRatio = durationRatio - other.durationRatio,
                dmgRatio = dmgRatio - other.dmgRatio,
                groggyRatio = groggyRatio - other.groggyRatio,

                manaGain = manaGain - b.manaGain,
                manaGainInSecond = manaGainInSecond - b.manaGainInSecond,
                maxMana = maxMana - b.maxMana,
                circle1Count = circle1Count - b.circle1Count,
                circle1Radius = circle1Radius - b.circle1Radius,
                circle1groggy = circle1groggy - b.circle1groggy,
                circle2Mana = circle2Mana - b.circle2Mana,
                circle2Duration = circle2Duration - b.circle2Duration,
                circle2Radius = circle2Radius - b.circle2Radius,
                circle2groggy = circle2groggy - b.circle2groggy,
                circle2AtkFrequency = circle2AtkFrequency - b.circle2AtkFrequency,
                circle3Mana = circle3Mana - b.circle3Mana,
                circle3Radius = circle3Radius - b.circle3Radius,
                circle3groggy = circle3groggy - b.circle3groggy,
                meteorRadius = meteorRadius - b.meteorRadius,
                circle3ExpRadius = circle3ExpRadius - b.circle3ExpRadius,
                circle3ExpGroggy = circle3ExpGroggy - b.circle3ExpGroggy,
                highCircleDmgIncrement = highCircleDmgIncrement - b.highCircleDmgIncrement,

                manaGainRatio = manaGainRatio - b.manaGainRatio,
                maxManaRatio = maxManaRatio - b.maxManaRatio,
                circle1CountRatio = circle1CountRatio - b.circle1CountRatio,
                circle1RadiusRatio = circle1RadiusRatio - b.circle1RadiusRatio,
                circle1groggyRatio = circle1groggyRatio - b.circle1groggyRatio,
                circle2ManaRatio = circle2ManaRatio - b.circle2ManaRatio,
                circle2DurationRatio = circle2DurationRatio - b.circle2DurationRatio,
                circle2RadiusRatio = circle2RadiusRatio - b.circle2RadiusRatio,
                circle2groggyRatio = circle2groggyRatio - b.circle2groggyRatio,
                circle2AtkFrequencyRatio = circle2AtkFrequencyRatio - b.circle2AtkFrequencyRatio,
                circle3ManaRatio = circle3ManaRatio - b.circle3ManaRatio,
                circle3RadiusRatio = circle3RadiusRatio - b.circle3RadiusRatio,
                circle3groggyRatio = circle3groggyRatio - b.circle3groggyRatio,
                meteorRadiusRatio = meteorRadiusRatio - b.meteorRadiusRatio,
                circle3ExpRadiusRatio = circle3ExpRadiusRatio - b.circle3ExpRadiusRatio,
                circle3ExpGroggyRatio = circle3ExpGroggyRatio - b.circle3ExpGroggyRatio,
                highCircleDmgIncrementRatio = highCircleDmgIncrementRatio - b.highCircleDmgIncrementRatio,
            };
            return c;
        }
        return new IneActiveStat(base.Subtract(other));
    }

    public static IneActiveStat operator +(IneActiveStat a, IneActiveStat b)
    {
        return a.Combine(b) as IneActiveStat;
    }

    public static IneActiveStat operator -(IneActiveStat a, IneActiveStat b)
    {
        return a.Subtract(b) as IneActiveStat;
    }
}