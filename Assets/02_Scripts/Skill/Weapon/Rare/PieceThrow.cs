using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class PieceThrow : MagicSkill
    {
        protected override bool UseAtkRatio => false;

        protected override bool UseGroggyRatio => false;
        private ISkillActive iActive;
        protected override ActiveEnums _activeType => ActiveEnums.Continuous;

        private float curTime;
        
        [TitleGroup("스탯값")][LabelText("투척 주기")] public float frequency;
        [TitleGroup("스탯값")][LabelText("석고 조각 데미지")] public float dmg1;
        [TitleGroup("스탯값")][LabelText("석고 조각 그로기계수")] public float groggy1;
        [TitleGroup("스탯값")][LabelText("폭발 반경")] public float radius;
        [TitleGroup("스탯값")][LabelText("폭발 데미지")] public float expDmg;
        [TitleGroup("스탯값")][LabelText("나무 조각 데미지")] public float dmg2;
        [TitleGroup("스탯값")][LabelText("나무 조각 확률")] public float prob;
        [TitleGroup("스탯값")][LabelText("나무 조각 그로기계수")] public float groggy2;

        private Tweener tween;
        public override void Active()
        {
            base.Active();
            curTime = 0;
            Fire();
            eventUser?.EventManager.AddEvent(EventType.OnUpdate,UpdateInvoke);
        }

        void UpdateInvoke(EventParameters _)
        {
            if (curTime < frequency)
            {
                curTime += Time.deltaTime;
            }
            else
            {
                curTime = 0;
                Fire();
            }
        }
        void Fire()
        {
            float rand = Random.Range(0, 100f);
            type = rand <= prob ? ProjType.Fire : ProjType.Stone;
            string n = type == ProjType.Fire ? "FirePiece" : "StonePiece";

            Projectile proj = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, n,
                user.Position);
            float d = type == ProjType.Fire ? dmg2 : dmg1;
            
            proj.Init(attacker,new AtkBase(attacker,d));
            proj.Init((int)(type == ProjType.Fire ? groggy2 : groggy1));
            proj.AddEventUntilInitOrDestroy(AddCC);
            proj.Fire();
        }

        enum  ProjType
        {
            Stone,Fire
        }

        private ProjType type;
        void AddCC(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                switch (type)
                {
                    case ProjType.Stone:
                        var exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                            Define.DummyEffects.Explosion, parameters.user.Position);
                        exp.Init(attacker,new AtkItemCalculation(attacker as Actor, this, expDmg),1);
                        exp.Init(GameManager.instance.Player.atkInfo);
                        exp.transform.localScale = Vector3.one * (radius * 2);
                        break;
                    case ProjType.Fire:
                        target.AddSubBuff(eventUser,SubBuffType.Debuff_Burn);
                        break;
                }
            }
        }
        
        public override void Cancel()
        {
            base.Cancel();
            eventUser?.EventManager.RemoveEvent(EventType.OnUpdate,UpdateInvoke);
            EndMotion();
        }
    }
}