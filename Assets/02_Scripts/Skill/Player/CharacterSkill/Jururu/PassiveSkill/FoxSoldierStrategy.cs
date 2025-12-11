using System.Collections;
using System.Linq;
using Apis;
using chamwhy;
using UnityEngine;
using Default;

public partial class FoxSoldier
{
    #region 공격 전략
    public interface IAtkStrategy
    {
        public void Attack();
    }

    public class NormalAttack : IAtkStrategy
    {
        private FoxSoldier soldier;

        public NormalAttack(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Attack()
        {
            soldier.pokdoAttackCollider.Init(soldier,new AtkBase(soldier,soldier.info.atkInfo.dmg));
            soldier.pokdoAttackCollider.Init(soldier.info.groggy);
        }
    }

    public class ShieldAttack : IAtkStrategy
    {
        private FoxSoldier soldier;
        
        public ShieldAttack(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Attack()
        {
            soldier.pokdoAttackCollider.Init(soldier,new AtkBase(soldier,soldier.info.atkInfo.dmg));
            soldier.pokdoAttackCollider.Init(soldier.info.groggy);
        }
    }
    public class GunAttack : IAtkStrategy
    {
        private FoxSoldier soldier;

        public GunAttack(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Attack()
        {
            soldier.EffectSpawner.Spawn(soldier.GetEffectAddress(EffectType.GunnerShoot), soldier.firePos.position,
                false);
            Projectile bullet = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                soldier.GetEffectAddress(EffectType.GunnerBullet), soldier.firePos.position);
            bullet.Init(soldier,new AtkBase(soldier,soldier.info.atkInfo.dmg),5);
            bullet.Init(soldier.info.atkInfo);
            bullet.Init(soldier.info.groggy);
            bullet.Fire();
        }
    }

    public class MagicAttack : IAtkStrategy
    {
        private FoxSoldier soldier;

        public MagicAttack(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Attack()
        {
            soldier.EffectSpawner.Remove(soldier.GetEffectAddress(EffectType.MagicianReady));
            soldier.EffectSpawner.Spawn(soldier.GetEffectAddress(EffectType.MagicianShoot), soldier.firePos.position,false);
            Projectile magic = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.JururuSoulKnightFireball, soldier.firePos.position);
            magic.Init(soldier,new AtkBase(soldier,soldier.info.atkInfo.dmg),5);
            magic.Init(soldier.info.atkInfo);
            magic.Init(soldier.info.groggy);
            magic.Fire();
        }
    }
    #endregion

    #region 공격 조건
    
    public interface IAtkCondition
    {
        public void Update();
    }

    public class AtkInHorizontal : IAtkCondition
    {
        private FoxSoldier soldier;

        public AtkInHorizontal(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Update()
        {
            var left = Physics2D.Raycast(soldier.Position, Vector2.left,soldier.info.atkRange,LayerMasks.Enemy);
            var right = Physics2D.Raycast(soldier.Position, Vector2.right,soldier.info.atkRange,LayerMasks.Enemy);
            
            if (left.collider != null && right.collider != null)
            {
                soldier.DoAttack(soldier.Direction == EActorDirection.Left
                    ? left.collider.transform
                    : right.collider.transform);
            }
            else if (left.collider != null)
            {
                soldier.DoAttack(left.collider.transform);
            }
            else if (right.collider != null)
            {
                soldier.DoAttack(right.collider.transform);
            }
        }
    }
    
    public class AtkInRadius : IAtkCondition
    {
        private FoxSoldier soldier;

        public AtkInRadius(FoxSoldier soldier)
        {
            this.soldier = soldier;
        }
        
        public void Update()
        {
            var enemies = soldier.gameObject.GetTargetsInCircle(soldier.info.findRange, LayerMasks.Enemy);
            enemies = enemies.OrderByDistance(soldier.Position);
            if (enemies.Count > 0)
            {
                soldier.DoAttack(enemies[0].transform);
            }
        }
    }

    public class AtkFirstTargetInRadius : IAtkCondition
    {
        private FoxSoldier soldier;

        public AtkFirstTargetInRadius(FoxSoldier soldier)
        {
            this.soldier = soldier;
        }
        
        public void Update()
        {
            if (soldier.target != null && !soldier.target.IsDead)
            {
                soldier.DoAttack(soldier.target.transform);
                return;
            }
            
            var enemies = soldier.gameObject.GetTargetsInCircle(soldier.info.findRange, LayerMasks.Enemy);
            enemies = enemies.OrderByDistance(soldier.Position);
            if (enemies.Count > 0)
            {
                soldier.target = enemies[0] as Actor;
                soldier.DoAttack(enemies[0].transform);
            }
        }
    }
    #endregion

    #region 공격 사용 방식
    
    public interface IAttackMethod
    {
        public void Attack(Transform target);
    }

    public class OrdinaryAttack : IAttackMethod
    {
        private FoxSoldier soldier;

        public OrdinaryAttack(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Attack(Transform target)
        {
            soldier.Attack(target);
        }
    }

    public class RushAttack : IAttackMethod
    {
        private FoxSoldier soldier;
        
        public RushAttack(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Attack(Transform target)
        {
            soldier.StartCoroutine(MoveToEnemy(target));
        }

        private Vector2 velocity;
        
        IEnumerator MoveToEnemy(Transform target)
        {
            float distance = Vector2.Distance(target.position, soldier.Position);

            while (distance > soldier.info.atkRange)
            {
                soldier.Position = Vector2.MoveTowards(soldier.Position, target.position, 8 * Time.deltaTime);
                soldier.SetDirection(target.position.x > soldier.Position.x
                    ? EActorDirection.Right
                    : EActorDirection.Left);
                distance = Vector2.Distance(target.position, soldier.Position);
                yield return null;
            }

            soldier.Attack(target);
        }
    }
    #endregion

    #region Idle 패턴

    public interface IIdlePattern
    {
        public void OnEnter();
        public void Update();
        public void OnExit();
    }

    public class StayStill : IIdlePattern
    {
        public void OnEnter()
        {
        }

        public void Update()
        {
        }

        public void OnExit()
        {
        }
    }

    public class FollowMaster : IIdlePattern
    {
        private FoxSoldier soldier;
        public FollowMaster(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }

        private Vector2 velocity;
        private CustomQueue<FoxSoldier> _queue;
        private CustomQueue<FoxSoldier> queue => soldier.skill.spawnedSoldiers;
        public void OnEnter()
        {
        }

        public void Update()
        {
            float offset = 0;
            if (queue.Contains(soldier))
            {
                offset = (-(queue.Count / 2) + queue.IndexOf(soldier)) * 0.5f;
            }
            float distance = Vector2.Distance(soldier.transform.position, soldier.Master.Position);
            if (distance > 0.02f)
            {
                soldier.transform.position = Vector2.SmoothDamp(soldier.transform.position, 
                    soldier.Master.Position + Vector3.right * offset, ref velocity, 0.5f,30);
            }
            else
            {
                //soldier.transform.position = soldier.Master.Position + Vector3.right * offset;
            }
            soldier.SetDirection(soldier.Master.Direction);
        }

        public void OnExit()
        {
        }
    }

    #endregion

    #region Attack 패턴

    public interface IAtkPattern
    {
        public void Update();
        public void AfterAtk();
    }

    public class AtkOnce : IAtkPattern
    {
        private FoxSoldier soldier;

        public AtkOnce(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Update()
        {
        }

        public void AfterAtk()
        {
            soldier.StartCoroutine(soldier.AttackCD());
            soldier.ChangeState(States.Idle);
        }
    }

    public class AtkUntilDie : IAtkPattern
    {
        private FoxSoldier soldier;

        public AtkUntilDie(FoxSoldier _soldier)
        {
            soldier = _soldier;
        }
        public void Update()
        {
            if (soldier.target == null || soldier.target.IsDead)
            {
                soldier.ChangeState(States.Idle);
            }
        }

        public void AfterAtk()
        {
            soldier.StartCoroutine(soldier.AttackCD());
        }
    }
    #endregion
}
