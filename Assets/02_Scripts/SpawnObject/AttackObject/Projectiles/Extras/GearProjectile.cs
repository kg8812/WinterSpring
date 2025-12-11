using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class GearProjectile : Projectile
    {
        [LabelText("폭발 반경")] public float size;
        [LabelText("폭발 데미지")] public float expDmg;
        [LabelText("냉기 부여확률")] public float prob;
        
        public override void Destroy()
        {
            AttackObject exp =
                GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject, "GearExplosion",transform.position);

            exp.transform.localScale = Vector3.one * (size * 0.3f);
            exp.Init(_attacker,new AtkBase(_attacker,expDmg),1);
            exp.Init(groggy);
            exp.AddEventUntilInitOrDestroy(AddChill);
            base.Destroy();
        }

        void AddChill(EventParameters parameters)
        {
            if (parameters?.target is Actor t)
            {
                float rand = Random.Range(0f, 100f);
                if (rand <= prob)
                {
                    t.AddSubBuff(_eventUser, SubBuffType.Debuff_Chill);
                }
            }
        }
    }
}