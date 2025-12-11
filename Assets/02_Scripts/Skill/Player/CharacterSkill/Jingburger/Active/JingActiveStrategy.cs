using Apis;
using Apis.SkillTree;
using chamwhy;
using DG.Tweening;
using UnityEngine;

public partial class JingburgerActiveSkill
{
    public interface ISpawnStrategy
    {
        public Projectile Spawn();
    }

    public class SpawnRasengan : ISpawnStrategy
    {
        private JingburgerActiveSkill skill;

        public SpawnRasengan(JingburgerActiveSkill active)
        {
            skill = active;
        }

        public Projectile Spawn()
        {
            skill.SpawnEffect(Define.PlayerEffect.JingRasenganShoot, skill.radius1, skill.user.Position, false);
            Projectile atk = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.JingRasengan,
                skill.user.Position + Vector3.right * (skill.radius1 * skill.direction?.DirectionScale ?? 1f));

            atk.transform.localScale = new Vector3(skill.radius1 * 2, skill.radius1 * 2);
            atk.Init(skill.attacker, new FixedAmount(0));
            atk.Init(skill.info1);
            
            return atk;
        }
    }

    public class SpawnRasenShuriken : ISpawnStrategy
    {
        private JingburgerActiveSkill skill;
        private Jingburger2A tree;

        public SpawnRasenShuriken(JingburgerActiveSkill skill, Jingburger2A tree)
        {
            this.skill = skill;
            this.tree = tree;
        }

        public Projectile Spawn()
        {
            skill.SpawnEffect(Define.PlayerEffect.JingRasenganShoot, skill.radius1, skill.user.Position, false);
            
            Projectile atk = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.JingShuriken,
                skill.user.Position + Vector3.right * (skill.radius1 * skill.direction?.DirectionScale ?? 1f));

            atk.transform.localScale = new Vector3(skill.radius1 * 2, skill.radius1 * 2);
            atk.Init(skill.attacker,new AtkItemCalculation(skill.attacker as Actor,skill,tree.projInfo.dmg ), tree.projInfo.duration);
            atk.Init(tree.projInfo);

            return atk;
        }
    }

    public interface IUseStrategy
    {
        public int MotionType { get; }
        public void Use(Projectile rasengan);
        public void Cancel();
    }

    public class FireProjectile : IUseStrategy
    {
        public int MotionType => 0;

        public void Use(Projectile rasengan)
        {
            rasengan?.Fire();
        }

        public void Cancel()
        {
        }
    }

    public class RushRasengan : IUseStrategy
    {
        private JingburgerActiveSkill skill;
        private Jingburger2C tree;
        private Player player => GameManager.instance.Player;

        public RushRasengan(JingburgerActiveSkill skill, Jingburger2C tree)
        {
            this.skill = skill;
            this.tree = tree;
        }

        private Tween tween;

        public int MotionType => 1;

        public void Use(Projectile rasengan)
        {
            rasengan.firstVelocity = Vector2.zero;
            rasengan.rigid.velocity = Vector2.zero;
            if (Mathf.Approximately(rasengan.duration, 0))
            {
                rasengan.Destroy(tree.distance / tree.speed);
            }

            var rushEffect = skill.SpawnEffect(Define.PlayerEffect.JingRasenganRush, skill.radius1, rasengan.Position,
                false);
            rushEffect.transform.SetParent(rasengan.transform);
            rasengan.Fire();

            player.AddEvent(EventType.OnHitReaction, EndSkill);
            player.Rb.gravityScale = 0;
            tween = player.ActorMovement.DashTemp(tree.distance / tree.speed, tree.distance, false).SetAutoKill(true)
                .SetSpeedBased().SetEase(Ease.Linear);

            tween.onUpdate += () =>
            {
                rasengan.transform.position = player.Position + Vector3.right * (int)player.Direction;
            };

            rasengan.AddEventUntilInitOrDestroy(_ =>
            {
                tween?.Kill();
                skill.EndMotion();
                player.Rb.gravityScale = player.ActorMovement.GravityScale;
                skill.RemoveEffect(Define.PlayerEffect.JingRasenganRush);
            }, EventType.OnDestroy);

            void EndSkill(EventParameters _)
            {
                rasengan?.Destroy();
                player.RemoveEvent(EventType.OnHitReaction, EndSkill);
            }
        }

        public void Cancel()
        {
            tween?.Kill();
        }
    }
}