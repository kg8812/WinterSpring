using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Apis
{
    [CreateAssetMenu(fileName = "DoorLockKeySkill", menuName = "Scriptable/Skill/Weapon/DoorLockKeySkill")]
    public class DoorLockKeySkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Charge;

        [TitleGroup("스탯값")] [LabelText("마법검 투사체 설정")] 
        public ProjectileInfo projInfo;

        private Queue<Projectile> blades = new();

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);
            Item = weapon;
        }

        public override void Active()
        {
            base.Active();
            blades.ForEach(x =>
            {
                Destroy(x.GetComponent<PetFollower>());
                x.transform.localScale = Vector3.one;
                x.Fire();
            });
            blades.Clear();
        }

        public override void Cancel()
        {
            base.Cancel();
            blades.ForEach(x =>
            {
                Destroy(x.GetComponent<PetFollower>());
                x.Destroy();
            });
            blades.Clear();
        }

        protected override void ChargeInvoke(int idx)
        {
            base.ChargeInvoke(idx);
            var dagger = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject,
                Define.PlayerSkillObjects.LilpaDagger,
                user.Position + Vector3.left * direction.DirectionScale + Vector3.up * 0.3f * idx);
            blades.Enqueue(dagger);
            var follower = Default.Utils.GetOrAddComponent<PetFollower>(dagger.gameObject);
            follower.Init(user.transform,user as Actor,Vector3.right + Vector3.up * 0.3f * idx);
            dagger.Init(attacker,new AtkItemCalculation(attacker as Actor, this));
            dagger.Init(projInfo);
            dagger.transform.localScale = new Vector3(direction.DirectionScale, 1,1);
        }
        
    }
}