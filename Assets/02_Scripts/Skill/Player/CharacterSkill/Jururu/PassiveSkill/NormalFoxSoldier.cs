public class NormalFoxSoldier : FoxSoldier
{
    private NormalAttack atkStrategy;
    private AtkInHorizontal atkCondition;
    private AtkFirstTargetInRadius atkInRadius;
    
    protected override IAtkStrategy AtkStrategy => atkStrategy ??= new NormalAttack(this);

    protected override IAtkCondition AtkCondition
    {
        get
        {
            switch (currentPattern)
            {
                case PatternType.Normal:
                    return atkCondition ??= new AtkInHorizontal(this);
                case PatternType.Following:
                    return atkInRadius ??= new AtkFirstTargetInRadius(this);
            }
            return null;
        }
    }

    private StayStill stayStill;
    private FollowMaster followMaster;

    private OrdinaryAttack ordinaryAttack;
    private RushAttack rushAttack;
    
    private AtkOnce atkOnce;
    private AtkUntilDie atkUntilDie;
    protected override IAtkPattern atkPattern{
        get
        {
            switch (currentPattern)
            {
                case PatternType.Normal:
                    return atkOnce ??= new(this);
                case PatternType.Following:
                    return atkUntilDie ??= new(this);
            }
            return atkOnce;
        }
    }
    protected override IIdlePattern idlePattern
    {
        get
        {
            switch (currentPattern)
            {
                case PatternType.Normal:
                    return stayStill ??= new();
                case PatternType.Following:
                    return followMaster ??= new(this);
            }
            return stayStill;
        }
    }

    protected override IAttackMethod atkMethod {
        get
        {
            switch (currentPattern)
            {
                case PatternType.Normal:
                    return ordinaryAttack ??= new(this);
                case PatternType.Following:
                    return rushAttack ??= new(this);
            }
            return ordinaryAttack;
        }
    }
}
