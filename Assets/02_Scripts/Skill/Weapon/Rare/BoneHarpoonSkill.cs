using Sirenix.OdinInspector;

namespace Apis

{
    public class BoneHarpoonSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        private BoneHarpoon harpoon;

        [TitleGroup("스탯값")][LabelText("돌아오는 속도")]
        public float speed;
        
        public override void Init(Weapon weapon)
        {
            base.Init(weapon);
            harpoon = weapon  as BoneHarpoon;
        }

        public override void Active()
        {
            base.Active();
            while (harpoon.harpoons.Count > 0)
            {
                var h = harpoon.harpoons.Dequeue();
                h.speed2 = speed;
                h.DmgRatio = Atk;
                h.Init(_weapon.CalculateGroggy(BaseGroggyPower));
                h.ReturnToActor();
            }
        }
    }
}