using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class HourGlass : MagicSkill
    {
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("스킬 반경")] public float radius;
        [PropertyOrder(2)][TitleGroup("스탯값")][LabelText("폭발 반경")] public float radius2;

        protected override ActiveEnums _activeType => ActiveEnums.Casting;

        private Queue<GameObject> _gears;
        private Queue<GameObject> gears => _gears ??= new();
        
        public override void Active()
        {
            base.Active();
        }

        public override void Init()
        {
            base.Init();
            
            actionList.Clear();
            actionList.Add(Explode);
        }

        void Explode()
        {
            GameManager.Factory.Return(gears.Dequeue());

            while (gears.Count > 0)
            {
                GameObject gear = gears.Dequeue();
                AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                    "GearExplosion", gear.transform.position);
                
                exp.AddEventUntilInitOrDestroy(ApplyBurn);
                exp.transform.localScale = Vector3.one * (radius2 * 0.3f * 2);
                exp.Init(attacker,new AtkBase(attacker,Atk),1);
                exp.Init((int)BaseGroggyPower);
                GameManager.Factory.Return(gear);
            }
        }
        void ApplyBurn(EventParameters parameters)
        {
            if (parameters?.target is Actor t)
            {
                t.AddSubBuff(eventUser,SubBuffType.Debuff_Burn);
            }
        }

        public override void StartCharge()
        {
            base.StartCharge();
            GameObject giantGear = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "GearSprite", user.Position);
            giantGear.transform.localScale = Vector3.one * 5;

            gears.Enqueue(giantGear);
            var col = Physics2D.OverlapCircleAll(user.Position, radius, LayerMasks.Enemy);

            List<Actor> targets = col.Select(x => x.transform.GetComponentInParentAndChild<Actor>())
                .Where(x => x != null).Distinct().ToList();

            targets.ForEach(x =>
            {
                GameObject gear = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "GearSprite", x.Position);
                SpineUtils.AddBoneFollower(x.Mecanim,"center",gear);
                gears.Enqueue(gear);
            });
        }

        public override void Cancel()
        {
            base.Cancel();

            while (gears.Count > 0)
            {
                GameManager.Factory.Return(gears.Dequeue());
            }
        }
    }
}