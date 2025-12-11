public interface IJingPassive : ISkill
{
    public JingPassiveStat JingStat { get; }
}

public class JingPassiveStat : SkillStat
{
    public int _maxStack;
    public int count;
    public float radius;

    public float _maxStackRatio;
    public float countRatio;
    public float radiusRatio;

    public JingPassiveStat()
    {
        
    }
    public JingPassiveStat(SkillStat stat)
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
        _maxStack = 0;
        count = 0;
        radius = 0;
        _maxStackRatio = 0;
        countRatio = 0;
        radiusRatio = 0;
    }

    public override SkillStat Combine(SkillStat other)
    {
        if (other is JingPassiveStat stat)
        {
            JingPassiveStat c = new()
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
                
                _maxStack = _maxStack + stat._maxStack,
                count = count + stat.count,
                radius = radius + stat.radius,
                _maxStackRatio = _maxStackRatio + stat._maxStackRatio,
                countRatio = countRatio + stat.countRatio,
                radiusRatio = radiusRatio + stat.radiusRatio,
            };

            return c;
        }
        
        return new JingPassiveStat(base.Combine(other));
    }

    public override SkillStat Subtract(SkillStat other)
    {
        if (other is JingPassiveStat stat)
        {
            JingPassiveStat c = new()
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
                
                _maxStack = _maxStack - stat._maxStack,
                count = count - stat.count,
                radius = radius - stat.radius,
                _maxStackRatio = _maxStackRatio - stat._maxStackRatio,
                countRatio = countRatio - stat.countRatio,
                radiusRatio = radiusRatio - stat.radiusRatio,
            };

            return c;
        }
        return new JingPassiveStat(base.Subtract(other));
    }

    public static JingPassiveStat operator +(JingPassiveStat a, JingPassiveStat b)
    {
        SkillStat c = a.Combine(b);
        return c as JingPassiveStat;
    }
    
    public static JingPassiveStat operator -(JingPassiveStat a, JingPassiveStat b)
    {
        SkillStat c = a.Subtract(b);
        return c as JingPassiveStat;
    }
}

public class JingPassiveAttachment : SkillAttachment,IJingPassive
{
    public JingPassiveAttachment(SkillStat stat) : base(stat)
    {
        JingStat = stat as JingPassiveStat;
    }

    public JingPassiveStat JingStat { get; }
}
public class JingPassiveConfig : SkillConfig,IJingPassive
{
    public JingPassiveConfig(SkillStat stat) : base(stat)
    {
        JingStat = stat as JingPassiveStat;
    }

    public JingPassiveStat JingStat { get; }
}
public class JingPassiveDecorator : SkillDecorator,IJingPassive
{
    public JingPassiveDecorator(ISkill skill, ISkill attachment) : base(skill, attachment)
    {
        config = skill as IJingPassive;
        this.attachment = attachment as IJingPassive;
    }
    
    private readonly IJingPassive config;
    private readonly IJingPassive attachment;
    public JingPassiveStat JingStat => config?.JingStat + attachment?.JingStat;

}