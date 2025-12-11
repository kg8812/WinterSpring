using chamwhy.DataType;
using Sirenix.OdinInspector;

namespace Apis.SkillTree
{
    public class LilpaTree3D : SkillTree
    {
        [System.Serializable]
        public struct DataStruct
        {
            [LabelText("지속시간 증가량")] public float duration;
        }

        public DataStruct[] datas;

        private bool isActivated;
        private SubBuffOptionDataType data;

        public override void Init()
        {
            base.Init();
            isActivated = false;
        }

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            if (data != null && isActivated)
            {
                data.duration -= datas[level-1].duration;
            }
            base.Activate(passive,level);
            isActivated = true;
            if (BuffDatabase.DataLoad.TryGetSubBuffOption(SubBuffType.HunterStack, out data))
            {
                data.duration += datas[level-1].duration;
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();

            if (data != null && isActivated)
            {
                data.duration -= datas[level-1].duration;
            }
            isActivated = false;
        }
    }
}