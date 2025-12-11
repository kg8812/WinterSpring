using Apis;
using chamwhy;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

public class HunterGunSkill : MagicSkill
{
    protected override ActiveEnums _activeType => ActiveEnums.Instant;

    [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("공격 반경")] public float size;
    [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("공격력 감소 지속시간")] public float duration2;
    [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("공격력 감소량")] public float amount;

    private Buff buff;

    private HunterGun hunterGun;

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);
        hunterGun = weapon as HunterGun;
    }
    public override void Active()
    {
        base.Active();
        if (buff == null)
        {
            var data = new BuffDataType(SubBuffType.Debuff_Atk)
            {
                buffPower = new[]{amount},buffCategory = 1,buffDuration = duration2,
                buffDispellType = 1,buffMaxStack = 1,valueType = ValueType.Value,showIcon = false
            };
            buff = new(data, eventUser);
        }

        AttackObject atk = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect, "HunterSkillExplosion",
            hunterGun.eagle.Position);
        atk.transform.localScale = Vector2.one * (size * 2);
        atk.Init(attacker,new AtkBase(attacker,Atk),1);
        atk.Init(_weapon.CalculateGroggy(BaseGroggyPower));
        atk.AddEvent(EventType.OnAttackSuccess,AddDebuff);
        hunterGun.eagle.ChangeState(HunterGunSummon.States.Idle);
        hunterGun.eagle.transform.position = user.Position;
    }

    void AddDebuff(EventParameters parameters)
    {
        if (parameters?.target is Actor act)
        {
            buff.AddSubBuff(act,null);
        }
    }
}
