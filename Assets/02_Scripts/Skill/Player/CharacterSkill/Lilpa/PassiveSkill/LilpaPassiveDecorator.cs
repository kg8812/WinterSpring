public interface ILilpaPassive : ISkill
{
    public LilpaPassiveStat LilpaStat { get; }
}

public class LilpaPassiveStat : SkillStat
{
    public float heal;
    public float healRatio;

    public float atk;
    public float atkRatio;
    
    public LilpaPassiveStat()
    {
        
    }
    public LilpaPassiveStat(SkillStat stat)
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
    public override SkillStat Combine(SkillStat other)
    {
        if (other is LilpaPassiveStat stat)
        {
            LilpaPassiveStat c = new()
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
                
                heal = heal + stat.heal,
                healRatio = heal + stat.healRatio,
                atk = atk + stat.atk,
                atkRatio = atkRatio + stat.atkRatio,
            };

            return c;
        }
        return new LilpaPassiveStat(base.Combine(other));
    }

    public override SkillStat Subtract(SkillStat other)
    {
        if (other is LilpaPassiveStat stat)
        {
            LilpaPassiveStat c = new()
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
                
                heal = heal - stat.heal,
                healRatio = heal - stat.healRatio,
                atk = atk - stat.atk,
                atkRatio = atkRatio - stat.atkRatio,
            };

            return c;
        }
        return new LilpaPassiveStat(base.Subtract(other));
    }

    public override void Reset()
    {
        base.Reset();
        heal = 0;
        healRatio = 0;
    }

    public static LilpaPassiveStat operator +(LilpaPassiveStat a, LilpaPassiveStat b)
    {
        return a.Combine(b) as LilpaPassiveStat;
    }

    public static LilpaPassiveStat operator -(LilpaPassiveStat a, LilpaPassiveStat b)
    {
        return a.Subtract(b) as LilpaPassiveStat;
    }
}

public class LilpaPassiveAttachment : SkillAttachment, ILilpaPassive
{
    public LilpaPassiveAttachment(SkillStat stat) : base(stat)
    {
        LilpaStat = stat as LilpaPassiveStat;
    }

    public virtual LilpaPassiveStat LilpaStat { get; }
}

public class LilpaPassiveConfig : SkillConfig, ILilpaPassive
{
    public LilpaPassiveConfig(SkillStat stat) : base(stat)
    {
        LilpaStat = stat as LilpaPassiveStat;
    }

    public LilpaPassiveStat LilpaStat { get; }
}

public class LilpaPassiveDecorator : SkillDecorator, ILilpaPassive 
{
    public LilpaPassiveDecorator(ISkill skill, ISkill attachment) : base(skill, attachment)
    {
        config = skill as ILilpaPassive;
        this.attachment = attachment as ILilpaPassive;
    }

    private ILilpaPassive config;
    private ILilpaPassive attachment;
    
    public LilpaPassiveStat LilpaStat => config?.LilpaStat + attachment?.LilpaStat;
}

