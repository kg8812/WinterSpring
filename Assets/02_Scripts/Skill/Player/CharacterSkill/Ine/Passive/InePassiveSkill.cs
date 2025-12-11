using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Apis
{
    [CreateAssetMenu(fileName = "InePassive", menuName = "Scriptable/Skill/InePassive")]
    public class InePassiveSkill : PlayerPassiveSkill
    {
        protected override bool UseGroggyRatio => false;
        
        [TitleGroup("스탯값")][LabelText("깃털 그로기수치")] [SerializeField] int featherGroggy;
        [TitleGroup("스탯값")][SerializeField] int maxFeather;
        [TitleGroup("스탯값")][SerializeField] float frequency;
        [TitleGroup("스탯값")][SerializeField] float radius;
        [TitleGroup("스탯값")][SerializeField] ProjectileInfo info;

        private IInePassive IneStat => stats as IInePassive;

        UnityEvent<GameObject,IOnHit> _onFire;
        UnityEvent _onAfterFire;
        public UnityEvent<GameObject,IOnHit> OnFire => _onFire ??= new();
        public UnityEvent OnAfterFire => _onAfterFire ??= new();

        private IPassiveFire _fireStrategy;

        private EventType fireEventType;

        private ISkillUse useStrategy;

        public void ChangeUseStrategy(ISkillUse _useStrategy)
        {
            useStrategy = _useStrategy;
        }
        
        public void ChangeFireStrategy(IPassiveFire fire)
        {
            _fireStrategy = fire;

            eventUser?.EventManager.RemoveEvent(fireEventType,Activate);

            if (fire is MoonSlash)
            {
                fireEventType = EventType.OnWeaponSlash;
            }
            else if (fire is FireFeather)
            {
                fireEventType = EventType.OnBasicAttack;
            }
            eventUser?.EventManager.AddEvent(fireEventType,Activate);
        }
        #region 스탯값

        public float FeatherAtk => Atk;

        public int FeatherGroggy
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return Mathf.RoundToInt((featherGroggy + IneStat.IneStat.featherGroggy) *
                           (1 + IneStat.IneStat.featherGroggyRatio / 100));
                }

                return featherGroggy;
            }
        }

        public int MaxFeather
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return Mathf.RoundToInt((maxFeather + IneStat.IneStat.maxFeather) *
                                            (1 + IneStat.IneStat.maxFeatherRatio / 100));
                }

                return maxFeather;
            }
        }

        public float Frequency
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (frequency + IneStat.IneStat.frequency) * (1 + IneStat.IneStat.frequencyRatio / 100);
                }

                return frequency;
            }
        }

        public float Radius
        {
            get
            {
                if (IneStat?.IneStat != null)
                {
                    return (radius + IneStat.IneStat.radius) * (1 + IneStat.IneStat.radiusRatio / 100);
                }

                return radius;
            }
        }
        #endregion

        protected override void SetConfig()
        {
            base.SetConfig();
            baseConfig = new InePassiveConfig(new InePassiveStat());
        }

        public override void Decorate()
        {
            stats = baseConfig;
            attachments.ForEach(x => { stats = new InePassiveDecorator(stats, x); });
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            isEnabled = true;
            ChangeFireStrategy(new FireFeather(this));
            ChangeUseStrategy(new InePassiveFireOnce(this));
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            eventUser?.EventManager.RemoveEvent(fireEventType,Activate);
            featherTime = 0;
            while (queue.Count > 0)
            {
                queue.Dequeue().Destroy();
            }
        }

        public override void Active()
        {
            base.Active();
            CreateFeather();
        }

        void Activate(EventParameters _)
        {
            useStrategy.UseSkill();
        }
        public void Fire(float angle,float move,float scale)
        {
            if (queue.Count == 0) return;
            if (!isEnabled) return;
            
            var enemy = user.gameObject.GetTargetsInCircle(Radius, LayerMasks.Enemy);
            
            if (enemy.Count == 0) return;

            float minDistance = 0;

            var list = enemy;
            if (direction != null)
            {
                List<IOnHit> left = enemy.Where(x => x.Position.x < user.Position.x).ToList();
                List<IOnHit> right = enemy.Where(x => x.Position.x > user.Position.x).ToList();


                switch (direction.Direction)
                {
                    case EActorDirection.Left:
                        list = left.Count > 0 ? left : right;
                        break;
                    case EActorDirection.Right:
                        list = right.Count > 0 ? right : left;
                        break;
                }
            }

            IOnHit target = null;
            list.ForEach(x =>
            {
                Vector2 ePos = x.Position;
                
                float distance = Vector2.Distance(user.Position, ePos);

                
                if (Mathf.Approximately(minDistance, 0))
                {
                    minDistance = distance;
                    target = x;
                }
                else if (minDistance > distance)
                {
                    minDistance = distance;
                    target = x;
                }
            });

            if (ReferenceEquals(target, null)) return;
            
            if (queue.TryDequeue(out var feather))
            {
                feather.transform.SetParent(null);
                feather.LookAtTarget(target);
                OnFire.Invoke(feather.gameObject, target);
                if (feather.TryGetComponent(out ParticleDestroyer destroyer))
                {
                    destroyer.disappearWhenHide = false;
                }

                _fireStrategy.Fire(feather,angle,move,scale);
                OnAfterFire.Invoke();
            }
        }

        public void RemoveFeather(EventParameters parameters)
        {
            var feather = parameters.user as IneFeather;
            
            if (queue.Contains(feather))
            {
                queue.Remove(feather);
            }
        }
        private float featherTime;

        private CustomQueue<IneFeather> _queue = new();
        private CustomQueue<IneFeather> queue => _queue ??= new();

        private bool isEnabled = true;

        private UnityEvent _onFeatherAtk;
        public UnityEvent OnFeatherAtk => _onFeatherAtk??= new();
        
        public void Enable()
        {
            isEnabled = true;
            foreach (var feather in queue)
            {
                feather.gameObject.SetActive(true);
                feather.Collider.enabled = false;
            }
        }
        public void Disable()
        {
            isEnabled = false;
            foreach (var feather in queue)
            {
                feather.gameObject.SetActive(false);
            }
        }

        protected override void UpdateEvent(EventParameters parameters)
        {
            base.UpdateEvent(parameters);
            if (!isEnabled) return;
            if (queue.Count >= MaxFeather) return;

            featherTime += Time.deltaTime;
            if (featherTime >= Frequency)
            {
                Use();
                featherTime = 0;
            }
        }

        private IneFeather last;

        public void CreateUntilMax()
        {
            while (queue.Count < MaxFeather)
            {
                CreateFeather();
            }
        }
        public void CreateFeather()
        {
            IneFeather feather = SpawnEffect(Define.PlayerEffect.IneFeather, 0.5f,user.transform.position,true).GetComponent<IneFeather>();
            
            feather.Init(attacker,new AtkItemCalculation(statUser as Actor, this,info.dmg));
            feather.Init(info);
            feather.Init(FeatherGroggy);
            feather.move.Degree = 0;
            feather.AddEventUntilInitOrDestroy(RemoveFeather,EventType.OnDestroy);
            queue.Enqueue(feather);

            if (!ReferenceEquals(last,null))
            {
                feather.move.Degree = last.move.Degree - 360f / MaxFeather;
            }
            else
            {
                feather.move.Degree = 0;
            }

            feather.move.Update();

            last = feather;

            var appear = SpawnEffect(Define.PlayerEffect.IneFeatherAppear, 0.5f, feather.transform.position,false);
            appear.transform.SetParent(feather.transform);
        }

        public void ResetFeatherPositions()
        {
            if (queue.Count > MaxFeather)
            {
                queue.Dequeue().Destroy();
            }
            int count = 0;

            queue.ForEach(x =>
            {
                x.move.Degree = 360 - 360f / MaxFeather * count;
                count++;
                last = x;
            });
        }
        private bool isFire;
        
        public IEnumerator FireAll()
        {
            if (isFire) yield break;

            isFire = true;
            int count = queue.Count;
            float angle = 0;
            
            for (int i = 0; i < count ; i++)
            {
                float rand = Random.Range(-0.3f, 0.3f);
                float scale = Random.Range(1, 1.2f);
                Fire(angle,rand,scale);
                angle += 30;
                yield return new WaitForSeconds(0.05f);
            }

            isFire = false;
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Feather;

        protected override float TagIncrement => GameManager.Tag.Data.FeatherIncrement;
    }
}