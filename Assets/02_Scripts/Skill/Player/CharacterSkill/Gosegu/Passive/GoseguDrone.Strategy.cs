using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Default;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

namespace Apis
{
    public partial class GoseguDrone
    {
        #region  공격 전략
        
        public interface IAtkStrategy
        {
            public bool Attack(CustomQueue<IOnHit> targets);
        }

        public class FireProjectile : IAtkStrategy
        {
            private GoseguDrone drone;
            private GoseguMainDrone.MainDroneInfo info;
            private int atkCount;

            public FireProjectile(GoseguDrone drone,GoseguMainDrone.MainDroneInfo info,int atkCount)
            {
                this.drone = drone;
                this.info = info;
                this.atkCount = atkCount;
            }
            public bool Attack(CustomQueue<IOnHit> targets)
            {
                while (targets.Count > 0)
                {
                    Sequence seq = DOTween.Sequence();
                    var enemy = targets.Dequeue();
                    for (int i = 0; i < atkCount; i++)
                    {
                        seq.AppendCallback(() =>
                        {
                            drone.EffectSpawner.Spawn(Define.PlayerEffect.GoseguDroneIcicleShoot, drone.Position,
                                false);
                            Projectile proj = GameManager.Factory
                                .Get <Projectile>(FactoryManager.FactoryType.Effect, 
                                    Define.PlayerEffect.GoseguDroneIcicle, drone.Position);
                            proj.transform.localScale = Vector2.one * (info.radius * 2);
                            proj.Init(drone, new AtkBase(drone, info.projInfo.dmg));
                            proj.Init(info.projInfo);
                            proj.Init(info.groggy);
                            proj.LookAtTarget(enemy);
                            proj.Fire(false);
                            proj.hitEffect = Define.PlayerEffect.GoseguDroneIcicleHit;
                            drone.OnAttack?.Invoke(proj);

                        });
                        seq.AppendInterval(0.1f);
                    }
                }

                return true;
            }
        }
        
        public class FireMissile : IAtkStrategy
        {
            private GoseguDrone drone;
            private GoseguMissileDrone.MissileDroneInfo missileInfo;
            public FireMissile(GoseguDrone drone,GoseguMissileDrone.MissileDroneInfo missileInfo)
            {
                this.drone = drone;
                this.missileInfo = missileInfo;
            }
            public bool Attack(CustomQueue<IOnHit> targets)
            {
                if (targets == null || targets.Count == 0) return false;
                
                for (int i = 0; i < missileInfo.maxCount; i++)
                {
                    float j = Mathf.Pow(-1, i);
                    var target = targets.Dequeue();
                    targets.Enqueue(target);
                    Projectile missile = GameManager.Factory.Get<Projectile>(
                        FactoryManager.FactoryType.AttackObject, Define.PlayerSkillObjects.GoseguMissile,
                        // ReSharper disable once PossibleLossOfFraction
                        drone.Position + Vector3.up * (0.5f * Mathf.Pow(-1, i) * (i / 2 + i % 2)));

                    missile.Init(drone,new AtkBase(drone,missileInfo.projInfo.dmg),10);
                    missile.Init(missileInfo.projInfo);
                    missile.Init(missileInfo.groggy);
                    missile.LookAtTarget(target);
                    missile.Fire();
                    missile.Rotate(30 * j);
                    drone.OnAttack?.Invoke(missile);
                    missile.hitEffect = Define.PlayerEffect.GoseguMechaAtkEffect;
                }
                targets.Clear();
                return true;
            }
        }

        public class FireLaser : IAtkStrategy
        {
            private GoseguLaserDrone.LaserDroneInfo info;
            private GoseguDrone drone;
            public FireLaser(GoseguDrone drone, GoseguLaserDrone.LaserDroneInfo info)
            {
                this.drone = drone;
                this.info = info;
                
            }
            public bool Attack(CustomQueue<IOnHit> targets)
            {
                if (targets == null || targets.Count == 0) return false;

                while (targets.Count > 0)
                {
                    var target = targets.Dequeue();
                    BeamEffect laser = GameManager.Factory.Get<BeamEffect>(FactoryManager.FactoryType.AttackObject,
                        Define.PlayerSkillObjects.GoseguLaser, drone.Position);
                    drone.PetFollower.moveOn = false;
                    laser.Init(drone, new AtkBase(drone, info.atkInfo.dmg),
                        info.beamInfo.fireTime * 2);
                    laser.Init(info.atkInfo);
                    laser.Init(info.beamInfo);
                    laser.Init(info.groggy);
                    laser.LookAtTarget(target);
                    laser.Fire();
                    
                    laser.AddEventUntilInitOrDestroy(_ =>
                    {
                        drone.PetFollower.moveOn = true;
                    },EventType.OnDestroy);
                    drone.OnAttack?.Invoke(laser);

                }

                return true;
            }
        }
        public class Heal : IAtkStrategy
        {
            private GoseguDrone drone;
            private GoseguHealingDrone.HealingDroneInfo healInfo;

            private bool isHeal;
            
            public Heal(GoseguDrone drone,GoseguHealingDrone.HealingDroneInfo healInfo)
            {
                this.drone = drone;
                this.healInfo = healInfo;
                isHeal = false;
            }
            public bool Attack(CustomQueue<IOnHit> targets)
            {
                if (isHeal || targets.Count == 0) return false;

                List<IOnHit> list = targets.ToList();
                list.ForEach(x =>
                {
                    x.CurHp += x.MaxHp / 100 * healInfo.instantHeal;
                });
                GameManager.instance.StartCoroutineWrapper(HealCoroutine(list));
                return true;
            }

            
            IEnumerator HealCoroutine(List<IOnHit> targets)
            {
                if (isHeal) yield break;
                isHeal = true;

                drone.EffectSpawner.Spawn(Define.EtcEffects.Recovery, drone.Position, false);
                
                for (int i = 0; i < healInfo.count; i++)
                {
                    yield return new WaitForSeconds(healInfo.frequency);
                    targets.ForEach(x => x.CurHp += healInfo.heal);
                }

                isHeal = false;
            }
        }

        public class FireFreezeBullet : IAtkStrategy
        {
            private GoseguDrone drone;
            private GoseguFreezeDrone.FreezeDroneInfo info;
            
            public FireFreezeBullet(GoseguDrone drone,GoseguFreezeDrone.FreezeDroneInfo info)
            {
                this.drone = drone;
                this.info = info;
            }
            public bool Attack(CustomQueue<IOnHit> targets)
            {
                if (targets == null || targets.Count == 0) return false;

                while (targets.Count > 0)
                {
                    var target = targets.Dequeue();
                    Projectile bullet = GameManager.Factory.Get<Projectile>(
                        FactoryManager.FactoryType.AttackObject, Define.PlayerSkillObjects.GoseguFreezeBullet,
                        drone.Position);

                    bullet.Init(drone, new AtkBase(drone, info.projInfo.dmg), 10);
                    bullet.Init(info.projInfo);
                    bullet.AddEventUntilInitOrDestroy(SpawnExplosion);
                    bullet.LookAtTarget(target);
                    bullet.Fire();
                    
                    drone.OnAttack?.Invoke(bullet);

                }

                return true;
            }

            void SpawnExplosion(EventParameters eventParameters)
            {
                if (eventParameters?.user == null) return;
                
                AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                    Define.PlayerSkillObjects.GoseguFreezeExplosion, eventParameters.user.transform.position);

                exp.transform.localScale = Vector3.one * (info.expRadius * 2);
                exp.Init(drone, new AtkBase(drone, info.dmg),1);
                exp.Init(drone.player.atkInfo);
                exp.Init(info.groggy);
                exp.AddEventUntilInitOrDestroy(ApplyFreeze);
            }

            void ApplyFreeze(EventParameters eventParameters)
            {
                if (eventParameters?.target is Actor target)
                {
                    target.SubBuffManager.AddCC(drone.player, SubBuffType.Debuff_Frozen, info.duration);
                }
            }
        }
        
        #endregion
        
        #region 타겟 서칭 전략

        public interface ISearchStrategy
        {
            public CustomQueue<IOnHit> GetTargets(EventParameters parameters);
            public void Update();

            public void OnDisable();
        }
        
        public class AttackedTarget : ISearchStrategy // 플레이어가 공격한 타겟
        {
            public CustomQueue<IOnHit> GetTargets(EventParameters parameters)
            {
                CustomQueue<IOnHit> queue = new();
                if (parameters?.target != null)
                {
                    queue.Enqueue(parameters.target);
                }

                return queue;
            }

            public void Update()
            {
            }

            public void OnDisable()
            {
            }
        }

        public class LockedOnTargets : ISearchStrategy // 락온된 타겟 (주변 서칭)
        {
            private CustomQueue<IOnHit> targets = new();
            private int count;
            private GoseguDrone drone;
            private float radius;

            private readonly Dictionary<IOnHit, ParticleDestroyer> particles = new();
            public LockedOnTargets(GoseguDrone drone, int _count,float radius)
            {
                count = _count;
                this.drone = drone;
                this.radius = radius;
            }
            public CustomQueue<IOnHit> GetTargets(EventParameters parameters)
            {
                Sequence seq = DOTween.Sequence();
                var temp = targets.ToList();
                seq.SetDelay(0.5f);
                seq.AppendCallback(() =>
                {
                    temp.ForEach(LockOff);
                });
                return targets;
            }

            public void Update()
            {
                var cols = drone.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy);
                cols = cols.OrderByDistance(drone.Position);
                var temp = targets.ToList();

                if (cols.Count > count)
                {
                    cols = cols.Take(count).ToList();
                }

                temp.ForEach(x =>
                {
                    if (!cols.Contains(x))
                    {
                        targets.Remove(x);
                        LockOff(x);
                    }
                });
                
                cols.ForEach(x =>
                {
                    if (!targets.Contains(x))
                    {
                        LockOn(x);
                        targets.Enqueue(x);
                    }
                });
            }

            public void OnDisable()
            {
                targets.ForEach(LockOff);
                targets.Clear();
            }

            void LockOn(IOnHit target)
            {
                if (target == null) return;

                var startEffect =
                    drone.EffectSpawner.Spawn(Define.PlayerEffect.GoseguLockOnStart, target.Position, false);
                startEffect.transform.SetParent(target.transform);
                startEffect.name = "LockOnEffectStart";

                if (target is Actor a)
                {
                    SpineUtils.AddBoneFollower(a.Mecanim, "center", startEffect.gameObject);
                }

                var loopEffect =
                    drone.EffectSpawner.Spawn(Define.PlayerEffect.GoseguLockOn, target.Position, false);
                loopEffect.transform.SetParent(target.transform);
                loopEffect.name = "LockOnEffect";

                if (target is Actor b)
                {
                    SpineUtils.AddBoneFollower(b.Mecanim, "center", loopEffect.gameObject);
                }

                particles.TryAdd(target, loopEffect);
            }

            void LockOff(IOnHit target)
            {
                if (target == null) return;


                if (particles.ContainsKey(target))
                {
                    drone.EffectSpawner.Remove(particles[target]);
                    particles.Remove(target);
                }

                var f = drone.EffectSpawner.Spawn(Define.PlayerEffect.GoseguLockOnEnd, target.Position, false);
                if (target is Actor b)
                {
                    SpineUtils.AddBoneFollower(b.Mecanim, "center", f.gameObject);
                }
            }
        }

        public class ClosestTarget : ISearchStrategy // 가장 가까운 타겟
        {
            private GoseguDrone drone;
            private float radius;
            public ClosestTarget(GoseguDrone drone, float radius)
            {
                this.drone = drone;
                this.radius = radius;
            }
            public CustomQueue<IOnHit> GetTargets(EventParameters parameters)
            {
                CustomQueue<IOnHit> queue = new();

                var targets = drone.gameObject.GetTargetsInCircle(radius, LayerMasks.Enemy);
                targets = targets.OrderByDistance(drone.Position);
                
                if(targets.Count > 0) queue.Enqueue(targets[0]);
                
                return queue;
            }

            public void Update()
            {
            }

            public void OnDisable()
            {
            }
        }
        
        public class SelfTarget : ISearchStrategy // 본인에게 적용
        {
            public CustomQueue<IOnHit> GetTargets(EventParameters parameters)
            {
                CustomQueue<IOnHit> targets = new();

                if (parameters?.user is IOnHit target)
                {
                    targets.Enqueue(target);
                }

                return targets;
            }

            public void Update()
            {
            }

            public void OnDisable()
            {
            }
        }

        
        #endregion
    }
}
