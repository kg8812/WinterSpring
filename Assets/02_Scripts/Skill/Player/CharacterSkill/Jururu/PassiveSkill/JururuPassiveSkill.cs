using System.Collections.Generic;
using DG.Tweening;
using Default;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Apis
{
    [CreateAssetMenu(fileName = "JururuPassive", menuName = "Scriptable/Skill/JururuPassive")]
    public class JururuPassiveSkill : PlayerPassiveSkill
    {
        protected override bool UseGroggyRatio => false;

        private ICdActive _cdActive;
        public override ICdActive CDActive => _cdActive ??= new StackCd(this);
        protected override CDEnums _cdType => CDEnums.Stack;

        private Dictionary<SoldierTypes, string> _soldierList;

        [HideInInspector] public CustomQueue<FoxSoldier> spawnedSoldiers;

        private UnityEvent<FoxSoldier> _onSoldierSpawn;
        public UnityEvent<FoxSoldier> OnSoldierSpawn => _onSoldierSpawn ??= new();
        public enum SoldierTypes
        {
            Normal,Shield,Magic,Gun
        }

        
        
        private FoxSoldier.PatternType patternType;

        public enum StackUseType
        {
            Once,All
        }

        [HideInInspector] public StackUseType useType;
        
        Dictionary<SoldierTypes, string> SoldierList
        {
            get
            {
                if (_soldierList != null) return _soldierList;
                
                _soldierList = new();
                _soldierList.Add(SoldierTypes.Normal, Define.PlayerSkillObjects.JururuFoxSoldier);

                return _soldierList;
            }
        }
        
        public void AddSoldierType(SoldierTypes type)
        {
            if (SoldierList.ContainsKey(type)) return;
            
            switch (type)
            {
                case SoldierTypes.Shield:
                    SoldierList.Add(type,Define.PlayerSkillObjects.JururuShieldFoxSoldier);
                    break;
                case SoldierTypes.Gun:
                    SoldierList.Add(type,Define.PlayerSkillObjects.JururuGunFoxSoldier);
                    break;
                case SoldierTypes.Magic:
                    SoldierList.Add(type,Define.PlayerSkillObjects.JururuMagicFoxSoldier);
                    break;
            }
        }

        public void RemoveSoliderType(SoldierTypes type)
        {
            SoldierList.Remove(type);
        }

        void RemoveSoldier(EventParameters eventParameters)
        {
            spawnedSoldiers.Remove(eventParameters?.user as FoxSoldier);
        }
        void SpawnFoxSoldier()
        {
            spawnedSoldiers ??= new();

            FoxSoldier newFoxSoldier = GameManager.Factory.Get<FoxSoldier>(FactoryManager.FactoryType.Normal, SoldierList[SoldierList.Keys.GetRandom()]);
            spawnedSoldiers.Enqueue(newFoxSoldier);
            newFoxSoldier.AddEvent(EventType.OnDeath,RemoveSoldier);
            newFoxSoldier.SetMaster(GameManager.instance.Player);
            newFoxSoldier.Init(Duration,patternType,this);
            switch (patternType)
            {
                case FoxSoldier.PatternType.Normal:
                    int rand = Random.Range(0, 2);
                    newFoxSoldier.transform.position =
                        _parameters.target.transform.position +
                        new Vector3(rand == 0
                                ? Random.Range(-newFoxSoldier.info.minSummonRadius, -newFoxSoldier.info.maxSummonRadius)
                                : Random.Range(newFoxSoldier.info.minSummonRadius, newFoxSoldier.info.maxSummonRadius),
                            0, 0);
                    break;
                case FoxSoldier.PatternType.Following:
                    newFoxSoldier.transform.position = user.Position;
                    break;
            }
            OnSoldierSpawn.Invoke(newFoxSoldier);
        }

        public override void Active()
        {
            base.Active();
            SpawnFoxSoldier();
            switch (useType)
            {
                case StackUseType.Once:
                    break;
                case StackUseType.All:
                    Sequence seq = DOTween.Sequence();
                    for (int i = 0; i < CurStack; i++)
                    {
                        seq.AppendInterval(0.1f);
                        seq.AppendCallback(SpawnFoxSoldier);
                    }

                    CurStack = 0;
                    break;
            }
        }

        private EventParameters _parameters;

        void Use(EventParameters x)
        {
            _parameters = x;
            Use();
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);
            eventUser?.EventManager.AddEvent(EventType.OnBasicAttack, Use);
            patternType = FoxSoldier.PatternType.Normal;
            useType = StackUseType.Once;
            spawnedSoldiers ??= new();
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            eventUser?.EventManager.RemoveEvent(EventType.OnBasicAttack, Use);
            FindObjectsByType<FoxSoldier>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .ForEach(x => x.Die());
        }

        public override void Cancel()
        {
            base.Cancel();
            FindObjectsByType<FoxSoldier>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .ForEach(x => x.Die());
            
        }

        public void ChangePatternType(FoxSoldier.PatternType type)
        {
            patternType = type;
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Soldier;

        protected override float TagIncrement => GameManager.Tag.Data.SoldierIncrement;
    }
}