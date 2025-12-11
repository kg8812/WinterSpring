using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Apis
{
    [CreateAssetMenu(fileName = "JingburgerPassive", menuName = "Scriptable/Skill/JingburgerPassive")]

    public partial class JingburgerPassiveSkill : PlayerPassiveSkill
    {
        Player masterPlayer;

        protected override bool UseGroggyRatio => false;

        [TitleGroup("스탯값")][LabelText("물감 그로기")] [SerializeField] float paintGroggy;
        [FormerlySerializedAs("maxStack")]
        [TitleGroup("스탯값")][LabelText("필요 스택")] [SerializeField] int _maxStack;
        [TitleGroup("스탯값")][LabelText("투사체 설정")] [SerializeField] ProjectileInfo info;
        [TitleGroup("스탯값")][LabelText("발사 개수")] [SerializeField] int count;
        [TitleGroup("스탯값")][LabelText("발사 텀")] [SerializeField] float term;
        [TitleGroup("스탯값")][LabelText("투사체 반경")] [SerializeField] float radius;
        [TitleGroup("스탯값")][LabelText("똥똥 설정")] [SerializeField] public DogInfo dogInfo;
        
        private int stack;

        private IFireStrategy fireStrategy;
        public IFireStrategy FireStrategy => fireStrategy;
        IJingPassive jingStats => stats as IJingPassive;

        #region 스탯값

        public override float BaseGroggyPower => (paintGroggy + stats.Stat.groggy) * (1 + stats.Stat.groggyRatio / 100);

        public int JingMaxStack
        {
            get
            {
                if (jingStats?.JingStat != null)
                {
                    return Mathf.RoundToInt((_maxStack + jingStats.JingStat._maxStack) * (1 + jingStats.JingStat._maxStackRatio / 100));
                }
                
                return _maxStack;
            }
        }

        public int Count
        {
            get
            {
                if (jingStats?.JingStat != null)
                {
                    return Mathf.RoundToInt((count + jingStats.JingStat.count) * (1 + jingStats.JingStat.countRatio / 100));
                }
                
                return count;
            }
        }

        public float Radius
        {
            get
            {
                if (jingStats?.JingStat != null)
                {
                    return (radius + jingStats.JingStat.radius) * (1 + jingStats.JingStat.radiusRatio / 100);
                }
                
                return radius;
            }
        }

        #endregion

        public enum AnimalPaints
        {
            Dog,Bird,Whale
        }
        public List<IFireEvent> FireStrategies => _fireStrategy ??= new();

        List<IFireEvent> _fireStrategy;

        public IFireEvent GetAnimalPaint(AnimalPaints animal)
        {
            switch (animal)
            {
                case AnimalPaints.Dog:
                    return FireStrategies.Find(x => x is SpawnPuppy);
                case AnimalPaints.Bird :
                    return FireStrategies.Find(x => x is SpawnBird);
                case AnimalPaints.Whale:
                    return FireStrategies.Find(x => x is SpawnWhale);
            }

            return null;
        }

        [HideInInspector] public Action<JingPaint> OnFire;

        [HideInInspector] public Action OnAnimalChange;

        [HideInInspector] public Action OnAnimalFire;

        [HideInInspector] public Action OnAnimalStackChange;
        
        public void ExecuteFireEvent(JingPaint atk)
        {
            foreach (var x in FireStrategies)
            {
                if (x.Usable)
                {
                    for (int i = 0; i < x.OnUse.Count; i++)
                    {
                        x.OnUse[i]?.Invoke(atk);
                    }
                    OnAnimalFire?.Invoke();
                }
            }
            FireStrategies.ForEach(x =>
            {
                if (x.CurStack == x.MaxStack)
                {
                    x.CurStack = 0;
                }
                else
                {
                    x.CurStack++;
                }
            });
            
            OnFire?.Invoke(atk);
        }
        public void AddFireStrategy(IFireEvent fireEvent)
        {
            FireStrategies.Add(fireEvent);
            _fireStrategy = FireStrategies.OrderBy(x => x.Priority).ToList();
            OnAnimalChange?.Invoke();
        }

        public void RemoveFireStrategy(IFireEvent fireEvent)
        {
            FireStrategies.Remove(fireEvent);
            _fireStrategy = FireStrategies.OrderBy(x => x.Priority).ToList();
            OnAnimalChange?.Invoke();
        }
        protected override void SetConfig()
        {
            baseConfig = new JingPassiveConfig(new JingPassiveStat());
        }

        public override void Decorate()
        {
            stats = baseConfig;
            attachments.ForEach(x => { stats = new JingPassiveDecorator(stats, x); });
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Depiction;

        protected override float TagIncrement => GameManager.Tag.Data.DepictionIncrement;

        public override void Init()
        {
            base.Init();
            stack = 0;
            fireStrategy = new FireNormal(this);
            AddFireStrategy(new SpawnPuppy(this,dogInfo));
        }

        public void ChangeFireStrategy(IFireStrategy strategy)
        {
            if (strategy == null) return;
            
            fireStrategy = strategy;
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            stack = 0;
            eventUser?.EventManager.AddEvent(EventType.OnAttack,Use);
            fireStrategy = new FireNormal(this);
        }

        void Use(EventParameters _)
        {
            Use();
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            eventUser?.EventManager.RemoveEvent(EventType.OnAttack,Use);
        }

        public override void Active()
        {
            base.Active();
            stack++;
            if (stack < JingMaxStack) return;
            stack = 0;

            if (fireStrategy != null)
            {
                GameManager.instance.StartCoroutine(FireCoroutine());
            }
        }

        IEnumerator FireCoroutine()
        {
            int c = fireStrategy.Count;

            for (int i = 0; i < c; i++) 
            {
                float curTime = 0;
                
                fireStrategy.Fire();
                
                
                while (curTime < term)
                {
                    curTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}