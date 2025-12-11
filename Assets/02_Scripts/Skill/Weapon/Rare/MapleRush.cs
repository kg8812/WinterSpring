using Sirenix.OdinInspector;

namespace Apis
{
    public class MapleRush : RushSlash
    {

        [InfoBox("다단히트는 각 히트마다 그로기가 적용됩니다")] [PropertyOrder(2)] [TitleGroup("스탯값")] [LabelText("마지막 공격 설정")]
        public ProjectileInfo atkinfo;
        
        public override void Active()
        {
            base.Active();
            tweener.onKill += () =>
            {
                foreach (var t in GameManager.instance.Player.attackColliders)
                {
                    SetAttackObject(t,atkinfo);
                }
            };
        }
    }
}