using Apis;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

public class MagicSticker : Projectile
{
    [Title("마법 스티커 설정")] [LabelText("효과 발동 확률")]
    public float prob;

    [LabelText("코끼리 반경")] public float radius1;
    [LabelText("코끼리 데미지")] public float dmg1;
    [LabelText("뱀 반경")] public float radius2;
    [LabelText("뱀 데미지")] public float dmg2;
    [LabelText("기절 지속시간")] public float stunDuraiton;
    [LabelText("물고기 반경")] public float radius3;
    [LabelText("물고기 데미지")] public float dmg3;

    protected override void AttackInvoke(EventParameters parameters)
    {
        base.AttackInvoke(parameters);
        float rand = Random.Range(0, 100f);

        if (rand <= prob)
        {
            int type = Random.Range(1, 4);
            AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect, "BaseExplosion",
                transform.position);
            switch (type)
            {
                case 1:
                    exp.transform.localScale = Vector3.one * (0.3f * radius1 * 2);
                    exp.Init(_attacker,new AtkBase(_attacker,dmg1),1);
                    break;
                case 2:
                    exp.transform.localScale = Vector3.one * (0.3f * radius2 * 2);
                    exp.Init(_attacker,new AtkBase(_attacker,dmg2),1);
                    exp.AddEventUntilInitOrDestroy(AddStun);
                    break;
                case 3:
                    exp.transform.localScale = Vector3.one * (0.3f * radius3 * 2);
                    exp.Init(_attacker,new AtkBase(_attacker,dmg3),1);
                    exp.AddEventUntilInitOrDestroy(AddChill);
                    break;
            }
        }

        void AddChill(EventParameters inf)
        {
            if (inf?.target is Actor t)
            {
                t.AddSubBuff(inf.user,SubBuffType.Debuff_Chill);
            }
        }

        void AddStun(EventParameters inf)
        {
            if (inf?.target is Actor t)
            {
                t.StartStun(_eventUser, stunDuraiton);
            }
        }
    }
}
