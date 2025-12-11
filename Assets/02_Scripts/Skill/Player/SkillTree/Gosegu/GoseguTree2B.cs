using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class GoseguTree2B : SkillTree
    {
        private GoseguActiveSkill skill;
        [LabelText("데미지 계수")] public float dmg;
        [LabelText("그로기 수치")] public int groggy;
        [LabelText("상호작용 반경")] public float radius;
        public override void Activate(PlayerActiveSkill active, int level)
        {
            base.Activate(active,level);
            skill = active as GoseguActiveSkill;

            if (skill == null) return;
            skill.OnMechaSpawn.RemoveListener(MakeInteractable);
            skill.OnMechaDie.RemoveListener(RemoveInteractable);
            skill.OnMechaSpawn.AddListener(MakeInteractable);
            skill.OnMechaDie.AddListener(RemoveInteractable);

            if (skill.Mecha != null)
            {
                MakeInteractable(skill.Mecha);
            }
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnMechaSpawn.RemoveListener(MakeInteractable);
            skill?.OnMechaDie.RemoveListener(RemoveInteractable);
            if (skill?.Mecha != null)
            {
                RemoveInteractable(skill.Mecha);
            }
        }

        void MakeInteractable(SeguMecha mecha)
        {
            FrozenObject.InteractEvent.RemoveListener(DestroyFrozenObject);
            FrozenObject.InteractEvent.AddListener(DestroyFrozenObject);
            FrozenObject.isEnabled = true;
            Monster.MakeInteractable(radius);
            Monster.CheckInteractable -= CheckInteractable;
            Monster.CheckInteractable += CheckInteractable;
            Monster.InteractEvent.RemoveListener(PileBunker);
            Monster.InteractEvent.AddListener(PileBunker);
        }

        void RemoveInteractable(SeguMecha mecha)
        {
            FrozenObject.isEnabled = false;
            Monster.RemoveInteractable();
            Monster.CheckInteractable -= CheckInteractable;
            Monster.InteractEvent.RemoveListener(PileBunker);
            FrozenObject.InteractEvent.RemoveListener(DestroyFrozenObject);
        }

        bool CheckInteractable(Monster monster)
        {
            return monster.Contains(SubBuffType.Debuff_Frozen);
        }
        void PileBunker(Monster monster)
        {
            if (skill?.Mecha == null) return;

            EventParameters temp = new EventParameters(skill.eventUser, monster)
            {
                atkData = new(){
                groggyAmount = groggy,
                atkStrategy =  new AtkItemCalculation(skill.user as Actor , skill ,dmg),
                attackType = Define.AttackType.Extra
                }
            };
            skill.Mecha.Attack(temp);
            skill.Mecha.animator.SetInteger("AttackType",4);
            skill.Mecha.animator.SetTrigger("Attack");
            monster.RemoveType(SubBuffType.Debuff_Frozen);
            GameObject explosion =
                GameManager.Factory.Get(FactoryManager.FactoryType.Effect, Define.DummyEffects.ExplosionNoAttack,monster.Position);
            GameManager.Factory.Return(explosion, 1);
        }

        void DestroyFrozenObject(GameObject obj)
        {
            GameObject explosion =
                GameManager.Factory.Get(FactoryManager.FactoryType.Effect, Define.DummyEffects.ExplosionNoAttack,obj.transform.position);
            GameManager.Factory.Return(explosion, 1);
        }
    }
}