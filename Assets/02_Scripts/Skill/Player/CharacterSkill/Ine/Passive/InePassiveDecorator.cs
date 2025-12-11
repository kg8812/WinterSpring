public interface IInePassive : ISkill
{
    public InePassiveStat IneStat { get; }
}

public class InePassiveConfig : SkillConfig, IInePassive
{
    public InePassiveConfig(SkillStat stat) : base(stat)
    {
        IneStat = stat as InePassiveStat;
    }

    public InePassiveStat IneStat { get; }
}

public class InePassiveAttachment : SkillAttachment, IInePassive
{
    public InePassiveAttachment(SkillStat stat) : base(stat)
    {
        if (stat is InePassiveStat ineStat)
        {
            IneStat = ineStat;
        }
        else
        {
            IneStat = null;
        }
    }

    public InePassiveStat IneStat { get; }
}

public class InePassiveDecorator : SkillDecorator,IInePassive
{
    public InePassiveStat IneStat => config?.IneStat + attachment?.IneStat;

    private readonly IInePassive config;
    private readonly IInePassive attachment;

    public InePassiveDecorator(ISkill skill, ISkill attachment) : base(skill, attachment)
    {
        config = skill as IInePassive;
        this.attachment = attachment as IInePassive;
    }
}

public class InePassiveStat : SkillStat
{
    public int featherGroggy;
    public int maxFeather;
    public float frequency;
    public float radius;

    public float featherGroggyRatio;
    public float maxFeatherRatio;
    public float frequencyRatio;
    public float radiusRatio;

    public InePassiveStat()
    {
    }

    public InePassiveStat(SkillStat stat)
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
        featherGroggy = 0;
        maxFeather = 0;
        frequency = 0;
        radius = 0;
        featherGroggyRatio = 0;
        maxFeatherRatio = 0;
        frequencyRatio = 0;
        radiusRatio = 0;
    }

    public override SkillStat Combine(SkillStat other)
    {
        if (other is InePassiveStat stat)
        {
            InePassiveStat c = new()
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
                
                featherGroggy = featherGroggy + stat.featherGroggy,
                maxFeather = maxFeather + stat.maxFeather,
                frequency = frequency + stat.frequency,
                radius = radius + stat.radius,
                featherGroggyRatio = featherGroggyRatio + stat.featherGroggyRatio,
                maxFeatherRatio = maxFeatherRatio + stat.maxFeatherRatio,
                frequencyRatio = frequencyRatio + stat.frequencyRatio,
                radiusRatio = radiusRatio + stat.radiusRatio,
            };

            return c;
        }
        return new InePassiveStat(base.Combine(other));
    }

    public override SkillStat Subtract(SkillStat other)
    {
        if (other is InePassiveStat stat)
        {
            InePassiveStat c = new()
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
                
                featherGroggy = featherGroggy - stat.featherGroggy,
                maxFeather = maxFeather - stat.maxFeather,
                frequency = frequency - stat.frequency,
                radius = radius - stat.radius,
                featherGroggyRatio = featherGroggyRatio - stat.featherGroggyRatio,
                maxFeatherRatio = maxFeatherRatio - stat.maxFeatherRatio,
                frequencyRatio = frequencyRatio - stat.frequencyRatio,
                radiusRatio = radiusRatio - stat.radiusRatio,
            };

            return c;
        }
        return new InePassiveStat(base.Subtract(other));
    }
    public static InePassiveStat operator +(InePassiveStat a, InePassiveStat b)
    {
        return a.Combine(b) as InePassiveStat;
    }

    public static InePassiveStat operator -(InePassiveStat a, InePassiveStat b)
    {
        return a.Subtract(b) as InePassiveStat;
    }
}