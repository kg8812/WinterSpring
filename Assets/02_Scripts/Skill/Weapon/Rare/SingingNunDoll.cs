using System.Collections;
using System.Collections.Generic;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class SingingNunDoll : MagicSkill
    {

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        [TitleGroup("스탯값")][LabelText("범위")] public Vector2 size;
        
        [TitleGroup("스탯값")][LabelText("공격 설정")] public ProjectileInfo atkInfo;

        [TitleGroup("스탯값")] [LabelText("공격 쿨타임")]
        public float atkCd;
        
        private AttackObject area;

        private bool isCd;
        
        public override void Active()
        {
            base.Active();

            isCd = false;
            area = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject, "GoatSeguArea",
                user.transform.position);
            area.transform.localScale = size / 2;
            area.Collider.enabled = false;
            eventUser.EventManager.AddEvent(EventType.OnAttack,Attack);
        }
        public override void AfterDuration()
        {
            base.AfterDuration();
            area.Destroy();
            eventUser.EventManager.RemoveEvent(EventType.OnAttack,Attack);
        }

        public override void Cancel()
        {
            base.Cancel();
            eventUser.EventManager.RemoveEvent(EventType.OnAttack,Attack);
        }

        void Attack(EventParameters param)
        {
            if (isCd) return;
            GameManager.instance.StartCoroutine(CdCoroutine());
            var atk = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.DummyEffects.Explosion, area.Position);
            atk.Init(attacker, new AtkItemCalculation(attacker as Actor, this,atkInfo.dmg),1);
            atk.Init(atkInfo);
            atk.Init((int)BaseGroggyPower);
            atk.transform.localScale = size * 0.6f;
        }

        IEnumerator CdCoroutine()
        {
            if (isCd) yield break;
            isCd = true;
            yield return new WaitForSeconds(atkCd);
            isCd = false;
        }
    }
}