using chamwhy.DataType;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Apis

{
    public class ThornWhip : MagicSkill
    {
        protected override bool UseAtkRatio => false;
        protected override bool UseGroggyRatio => false;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("채찍 사정거리")] public float distance;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("채찍 발사시간")] public float fireTime;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("채찍 지속시간")] public float d;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("채찍 y크기")] public float ySize;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("채찍 발사 Ease")] public Ease ease;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("채찍 공격설정")] public ProjectileInfo whipInfo;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("채찍 그로기 계수")] public float groggy1;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("묶임 지속시간")] public float tiedDuration;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("묶임 데미지")] public float tiedDmg;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("묶임 데미지 범위")] public float size;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("묶임 그로기 계수")] public float groggy2;
        private Buff _tiedBuff;

        private WhipEffect whip;

        public override void Init()
        {
            base.Init();
            actionList.Clear();
            actionList.Add(SpawnWhip);
        }

        void SpawnWhip()
        {
            if (_tiedBuff == null)
            {
                BuffDataType data = new(SubBuffType.Debuff_Tied)
                {
                    buffPower = new [] {tiedDmg,size,groggy2}, buffCategory = 1,
                    buffDuration = tiedDuration, buffDispellType = 1, buffMaxStack = 1, showIcon = false
                };
                _tiedBuff = new(data,eventUser);
            }

            whip = GameManager.Factory.Get<WhipEffect>(FactoryManager.FactoryType.AttackObject,
                "SpellbookWhip", user.Position);

            whip.ease = ease;
            whip.distance = distance;
            whip.ySize = ySize;
            whip.fireTime = fireTime;
            whip.Init(attacker,new AtkBase(attacker,whipInfo.dmg),d);
            whip.Init(whipInfo);
            whip.Init((int)groggy1);
            whip.Fire();
            whip.AddEvent(EventType.OnAttackSuccess,AddTied);
        }
        void AddTied(EventParameters parameters)
        {
            if (parameters?.target is Actor target)
            {
                _tiedBuff.AddSubBuff(target,null);
            }
        }
    }
}