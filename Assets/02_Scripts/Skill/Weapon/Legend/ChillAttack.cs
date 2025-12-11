using chamwhy;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace Apis
{
    public class ChillAttack : LeafAttack
    {
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 반경")] public float radius1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("냉기기운 반경")] public float radius2;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("냉기기운 공격설정")] public ProjectileInfo atkInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("냉기기운 지속시간")] public float duration2;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("냉기기운 그로기계수")] public float poisonGroggy;

        protected override void GroundSkill()
        {
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "PlagueExplosion", user.transform.position);
            obj.transform.localScale = Vector2.one * radius1 * 2 * 0.3f;
            obj.Init(attacker,new AtkBase(attacker,groundDmg),1);
            obj.Init((int)(groundGroggy));
            obj.AddEventUntilInitOrDestroy(AddChill);
            SpawnPoison();
        }

        protected override void AirSkill()
        {
            AttackObject obj = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "PlagueExplosion", user.transform.position);
            obj.transform.localScale = Vector2.one * radius1 * 2 * 0.3f;
            obj.Init(attacker,new AtkBase(attacker,landDmg),1);
            obj.Init((int)(landgroggy));
            obj.AddEventUntilInitOrDestroy(AddChill);
            SpawnPoison();
        }

        void SpawnPoison()
        {
            AttackObject poison = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                "ChillAtmosphere", user.Position);
            SpineUtils.AddBoneFollower(skeleton?.Mecanim,"center",poison.gameObject);
            poison.AddEventUntilInitOrDestroy(_ =>
            {
                Destroy(poison.GetComponent<BoneFollower>());
            },EventType.OnDestroy);
            poison.transform.localScale = Vector2.one * radius2 * 2;
            poison.Init(attacker,new AtkBase(attacker,atkInfo.dmg),duration2);
            poison.Init(atkInfo);
            poison.Init((int)(poisonGroggy));
            poison.AddEventUntilInitOrDestroy(AddChill);
        }
        void AddChill(EventParameters parameters)
        {
            if (parameters?.target is Actor t)
            {
                t.AddSubBuff(eventUser, SubBuffType.Debuff_Chill);
            }
        }
    }
}