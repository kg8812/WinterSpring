using chamwhy;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class Jingburger3E : SkillTree
    {
        private JingburgerPassiveSkill passive;
        private JingburgerActiveSkill active;

        [System.Serializable]
        public struct DataStruct
        {
            
            [LabelText("액티브 쿨타임 감소량")] public float cdReduce;
            [LabelText("패시브 물통 증가량")] public int passiveStack;
        }

        public DataStruct[] datas;
        
        public override void Activate(PlayerPassiveSkill _passive, int level)
        {
            base.Activate(passive,level);
            passive = _passive as JingburgerPassiveSkill;
            if (passive == null) return;
            passive.OnFire -= MinusActiveCd;
            passive.OnFire += MinusActiveCd;
        }

        public override void Activate(PlayerActiveSkill _active, int level)
        {
            base.Activate(active,level);
            this.active = _active as JingburgerActiveSkill;
            if (active == null) return;
            active.OnActive.RemoveListener(AddStack);
            active.OnActive.AddListener(AddStack);
        }
        
        public override void DeActivate()
        {
            base.DeActivate();
            
            active?.OnActive.RemoveListener(AddStack);
            if (passive != null)
            {
                passive.OnFire -= MinusActiveCd;
            }
        }

        void AddStack()
        {
            if (passive == null) return;
            
            passive.FireStrategies.ForEach(x =>
            {
                x.CurStack += datas[level-1].passiveStack;
            });
        }

        void MinusActiveCd(AttackObject paint)
        {
            paint.AddEventUntilInitOrDestroy(x =>
            {
                if (active == null) return;
                active.CurCd -= datas[level-1].cdReduce;
            });
        }
    }
}