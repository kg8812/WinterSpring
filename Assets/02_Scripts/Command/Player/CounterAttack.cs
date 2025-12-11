using Apis;
using Apis.SkillTree;
using chamwhy;
using UnityEngine;

namespace Command
{
    [CreateAssetMenu(fileName = "CounterAttack",menuName = "ActorCommand/Player/ViichanCounterAtk")]
    public class CounterAttack : PlayerCommand
    {
        private ViichanTree2C tree;

        public void Init(ViichanTree2C tree)
        {
            this.tree = tree;
        }

        protected override void Invoke(Player go)
        {
            if (go.ActiveSkill is ViichanActiveSkill skill)
            {
                AttackObject atk = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                    Define.PlayerEffect.ViichanCounter, go.Position);
                atk.transform.localScale = new Vector3((int)go.Direction * tree.atkArea.x, tree.atkArea.y, 1);
                atk.Init(go, new FixedAmount(skill.MaxGauge * tree.dmg / 100), atk.projectileInfo.duration);
                atk.Init(tree.groggy);
                tree?.Resume();
            }
        }

        public override bool InvokeCondition(Player go)
        {
            return true;
        }
    }
}