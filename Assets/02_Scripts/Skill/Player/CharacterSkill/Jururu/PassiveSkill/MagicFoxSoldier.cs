public class MagicFoxSoldier : FoxSoldier
{
    private MagicAttack atkStrategy;
    private AtkInRadius atkCondition;
    
    protected override IAtkStrategy AtkStrategy => atkStrategy ??= new(this);

    protected override IAtkCondition AtkCondition => atkCondition ??= new(this);
    private StayStill stayStill;
    private FollowMaster followMaster;

    private OrdinaryAttack ordinaryAttack;
    private RushAttack rushAttack;
    
    private AtkOnce atkOnce;
    protected override IAtkPattern atkPattern => atkOnce ??= new(this);

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
            return ordinaryAttack ??= new(this);
        }
    }
}
