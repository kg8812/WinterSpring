
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public class Barbell : Weapon
    {
        [Title("바벨 설정")] [LabelText("넉백추가 콤보")] [Tooltip("입력한 타수에 적이 맞으면 날라감")] 
        public int comboCount;
        [LabelText("넉백 파워")] public float knockBackPower;
        [LabelText("넉백 시간")] public float knockBackTime;
        [LabelText("넉백 각도")] public float knockBackAngle;
        public override AttackCategory Category => AttackCategory.GreatSword;

        public override void SetGroundCollider(int idx, AttackObject[] attackObjs)
        {
            base.SetGroundCollider(idx, attackObjs);
            if (idx == comboCount - 1)
            {
                attackObjs.ForEach(x =>
                {
                    x.knockBackData.knockBackForce = knockBackPower;
                    x.knockBackData.knockBackAngle = knockBackAngle;
                    x.knockBackData.knockBackTime = knockBackTime;
                    x.groggyKnockBackData.knockBackForce = knockBackPower;
                    x.groggyKnockBackData.knockBackAngle = knockBackAngle;
                    x.groggyKnockBackData.knockBackTime = knockBackTime;
                });
            }
        }

        public override void SetAirCollider(int idx, AttackObject[] attackObjs)
        {
            base.SetAirCollider(idx, attackObjs);
            if (idx == comboCount - 1)
            {
                attackObjs.ForEach(x =>
                {
                    x.knockBackData.knockBackForce = knockBackPower;
                    x.knockBackData.knockBackAngle = knockBackAngle;
                    x.knockBackData.knockBackTime = knockBackTime;
                    x.groggyKnockBackData.knockBackForce = knockBackPower;
                    x.groggyKnockBackData.knockBackAngle = knockBackAngle;
                    x.groggyKnockBackData.knockBackTime = knockBackTime;
                });
            }
        }
    }
}