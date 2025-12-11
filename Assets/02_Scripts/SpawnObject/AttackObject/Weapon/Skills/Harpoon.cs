using chamwhy;
using UnityEngine;

namespace Apis
{
    public class Harpoon : Boomerang
    {
        private BoneHarpoon weapon;
        public GameObject extraCol;
        
        public void Init(BoneHarpoon _weapon)
        {
            weapon = _weapon;
            AddEvent(EventType.OnDestroy,Remove);
        }

        void Remove(EventParameters _)
        {
            weapon.harpoons.Remove(this);
        }
        protected override void FirstAttackInvoke(EventParameters parameters)
        {
            base.FirstAttackInvoke(parameters);
            if (parameters.target is Actor actor)
            {
                actor.AddSubBuff(_eventUser,SubBuffType.Debuff_Bleed);
            }
        }

        protected override void ReturnAttackInvoke(EventParameters parameters)
        {
            base.ReturnAttackInvoke(parameters);
            if (parameters.target is Actor actor)
            {
                actor.AddSubBuff(_eventUser,SubBuffType.Debuff_Bleed);
            }
        }

        public override void Stop()
        {
            base.Stop();
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            extraCol.SetActive(true);
        }

        public override void Init(AttackObjectInfo atkObjectInfo)
        {
            base.Init(atkObjectInfo);
            extraCol.SetActive(false);
        }

        public override void Fire(bool rotateWithPlayerX = true)
        {
            base.Fire(rotateWithPlayerX);
            extraCol.SetActive(false);
        }

        public override void ReturnToActor()
        {
            base.ReturnToActor();
            extraCol.SetActive(false);
        }

        private float distance;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isStop || isReturn) return;
            
            distance = Vector2.Distance(_attacker.Position, transform.position);

            if (distance > weapon.maxDistance)
            {
                Destroy();
            }
        }
    }
}