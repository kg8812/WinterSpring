public interface IGoseguActive
{
    public GoseguActiveStat GoseguStat { get; }
}

public class GoseguActiveStat : SkillStat
{
    public float hp;
    public float maxGauge;
    public float finalDmgIncrement;
    public float gaugeGain;
    
    public float hpRatio;
    public float maxGaugeRatio;
    public float gaugeGainRatio;

    public GoseguActiveStat()
    {
        
    }
    public GoseguActiveStat(SkillStat stat)
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
        hp = 0;
        maxGauge = 0;
        hpRatio = 0;
        maxGaugeRatio = 0;
        finalDmgIncrement = 0;
        gaugeGain = 0;
        gaugeGainRatio = 0;
    }

    public override SkillStat Combine(SkillStat other)
    {
        if (other is GoseguActiveStat stat)
        {
            GoseguActiveStat c = new()
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

                hp = hp + stat.hp,
                maxGauge = maxGauge + stat.maxGauge,
                hpRatio = hpRatio + stat.hpRatio,
                maxGaugeRatio = maxGaugeRatio + stat.maxGaugeRatio,
                finalDmgIncrement = finalDmgIncrement + stat.finalDmgIncrement,
                gaugeGain = gaugeGain + stat.gaugeGain,
                gaugeGainRatio = gaugeGainRatio + stat.gaugeGainRatio,
            };

            return c;
        }
        return new GoseguActiveStat(base.Combine(other));
    }

    public override SkillStat Subtract(SkillStat other)
    {
        if (other is GoseguActiveStat stat)
        {
            GoseguActiveStat c = new()
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

                hp = hp - stat.hp,
                maxGauge = maxGauge - stat.maxGauge,
                hpRatio = hpRatio - stat.hpRatio,
                maxGaugeRatio = maxGaugeRatio - stat.maxGaugeRatio,
                finalDmgIncrement = finalDmgIncrement - stat.finalDmgIncrement,
                gaugeGain = gaugeGain - stat.gaugeGain,
                gaugeGainRatio = gaugeGainRatio - stat.gaugeGainRatio,
            };

            return c;
        }
        return new GoseguActiveStat(base.Subtract(other));
    }

    public static GoseguActiveStat operator +(GoseguActiveStat a, GoseguActiveStat b)
    {
        return a.Combine(b) as GoseguActiveStat;
    }
    
    public static GoseguActiveStat operator -(GoseguActiveStat a, GoseguActiveStat b)
    {
        return a.Subtract(b) as GoseguActiveStat;
    }
}

public class GoseguActiveAttachment : SkillAttachment, IGoseguActive
{
    public GoseguActiveAttachment(SkillStat stat) : base(stat)
    {
        GoseguStat = stat as GoseguActiveStat;
    }

    public GoseguActiveStat GoseguStat { get; }
}

public class GoseguActiveConfig : SkillConfig, IGoseguActive
{
    public GoseguActiveConfig(SkillStat stat) : base(stat)
    {
        GoseguStat = stat as GoseguActiveStat;
    }

    public GoseguActiveStat GoseguStat { get; }
}

public class GoseguActiveDecorator : SkillDecorator, IGoseguActive
{
    public GoseguActiveDecorator(ISkill skill, ISkill attachment) : base(skill, attachment)
    {
        config = skill as IGoseguActive;
        this.attachment = attachment as IGoseguActive;
    }

    private IGoseguActive config;
    private IGoseguActive attachment;
    public GoseguActiveStat GoseguStat => config?.GoseguStat + attachment?.GoseguStat;
}