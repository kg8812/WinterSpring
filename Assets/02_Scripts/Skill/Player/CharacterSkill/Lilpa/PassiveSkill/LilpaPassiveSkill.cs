using Sirenix.OdinInspector;
using Spine;
using UnityEngine;
using UnityEngine.Events;

namespace Apis
{
    [CreateAssetMenu(fileName = "LilpaPassive", menuName = "Scriptable/Skill/LilpaPassive")]
    public class LilpaPassiveSkill : PlayerPassiveSkill
    {
        protected override bool UseGroggyRatio => false;

        [TitleGroup("스탯값")] [LabelText("회복량(%)")] [SerializeField] float amount;
        
        private UnityEvent<int> _onHeal; // 흡혈 발동시 실행되는 이벤트, 매개변수는 흡혈 스택
        public UnityEvent<int> OnHeal => _onHeal ??= new();
        ILilpaPassive lilpaStat => stats as ILilpaPassive;

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            eventUser?.EventManager.AddEvent(EventType.OnKill,Heal);
            eventUser?.EventManager.AddEvent(EventType.OnColliderAttack,AddStack);
            isEnhanced = false;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
           eventUser?.EventManager.RemoveEvent(EventType.OnKill,Heal);
           eventUser?.EventManager.RemoveEvent(EventType.OnColliderAttack,AddStack);
        }

        void AddStack(EventParameters parameters)
        {
            if (!TryUse()) return;
            
            if (parameters?.target is Actor target)
            {
                target.AddSubBuff(eventUser,SubBuffType.HunterStack);
            }
        }
        protected override void SetConfig()
        {
            base.SetConfig();
            baseConfig = new LilpaPassiveConfig(new LilpaPassiveStat());
        }

        public override void Decorate()
        {
            stats = baseConfig;
            attachments.ForEach(x => { stats = new LilpaPassiveDecorator(stats, x);});
        }

        private Bone bone;
        [HideInInspector] public bool isEnhanced;
        public void Heal(EventParameters parameters)
        {
            if (!(parameters?.target is Actor t)) return;

            int count = t.SubBuffCount(SubBuffType.HunterStack);

            if (count <= 0) return;
            if (hit == null) return;
           
            hit.CurHp += (hit.MaxHp - hit.CurHp) * ((amount * count + (lilpaStat.LilpaStat.heal +
                    GameManager.Tag.Data.HuntIncrement * GameManager.Tag.GetTagCount(TagManager.SkillTreeTag.Hunt))) *
                (1 + lilpaStat.LilpaStat.healRatio / 100f)) / 100;
            
            OnHeal.Invoke(count);

            SpawnEffect(Define.PlayerEffect.LilpaDrain,0.5f , false);

            GameManager.Sound.Play("lilpaSkill_absorb");
            isEnhanced = false;
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Hunt;

        protected override float TagIncrement => GameManager.Tag.Data.HuntDmgIncrement;
    }
}