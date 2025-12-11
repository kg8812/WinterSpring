using System;
using Sirenix.OdinInspector;

namespace Apis
{
    public partial class JingburgerPassiveSkill 
    {
        [Serializable]
        public struct BirdInfo
        {
            [LabelText("총 데미지")] public float dmg;
            [LabelText("그로기 수치")] public int groggy;
            [LabelText("필요 게이지")] public int count;
        }

        [Serializable]
        public struct DogInfo
        {
            [LabelText("데미지")] public float dmg;
            [LabelText("그로기 수치")] public int groggy;
            [LabelText("공격 횟수")] public int atkCount;
            [LabelText("필요 게이지")] public int count;
        }

        [Serializable]
        public struct WhaleInfo
        {
            [LabelText("데미지")] public float dmg;
            [LabelText("그로기 수치")] public int groggy;
            [LabelText("필요 게이지")] public int count;
        }
    }
}