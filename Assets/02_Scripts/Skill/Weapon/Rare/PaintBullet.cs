using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Apis
{
    public class PaintBullet : MoonLightPenetrationBullet
    {
        [ValidateInput("ValidateSize", "차징과 개수를 맞춰주세요")]
        [TabGroup("사용 관련/Charge/설정","차징 설정")] [LabelText("차징별 폭발 크기")] public List<float> sizes;
        
        private float size;

        public override void Init()
        {
            base.Init();
            size = 1;
        }

        bool ValidateSize(List<float> _sizes)
        {
            return _sizes.Count == chargeInfos.Count;
        }
        protected override void Action1()
        {
            global::PaintBullet proj =
                GameManager.Factory.Get<global::PaintBullet>(FactoryManager.FactoryType.AttackObject, "paintBullet", user.Position);
            proj.Init(attacker,new AtkBase(attacker,Atk));
            proj.Init(projectileInfo);

            proj.size = size;
            proj.Fire();
        }

        protected override void ChargeInvoke(int idx)
        {
            base.ChargeInvoke(idx);
            size = sizes[idx];
        }
    }
}