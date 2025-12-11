using System.Collections.Generic;
using Apis.SkillTree;
using Command;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Apis
{
    public partial class JingburgerPassiveSkill
    {
        public interface IFireStrategy
        {
            public void Fire();
            public int Count { get; }
        }

        public class FireNormal : IFireStrategy
        {
            private JingburgerPassiveSkill skill;

            public FireNormal(JingburgerPassiveSkill _skill)
            {
                skill = _skill;
            }

            void Spawn()
            {
                
                JingPaint paint = GameManager.Factory
                    .Get<JingPaint>(FactoryManager.FactoryType.Effect,
                        Define.PlayerEffect.JingPaint, skill.user.Position);
                paint.transform.localScale = new Vector3(skill.Radius * 2, skill.Radius * 2, 1);

                paint.Init(skill.attacker, new AtkItemCalculation(skill.user as Actor , skill, skill.info.dmg));
                paint.Init(skill.info);
                paint.Init(Mathf.RoundToInt(skill.BaseGroggyPower));
                paint.Fire();
                paint.hitEffect = Define.PlayerEffect.JingPaintHit;
                skill.ExecuteFireEvent(paint);
            }

            public void Fire()
            {
                Spawn();
            }

            public int Count => skill?.Count ?? 1;
        }

        public interface IFireEvent
        {
            public int Priority { get; }
            public List<UnityAction<JingPaint>> OnUse { get; }
            public bool Usable { get; }
            public int CurStack { get; set; }
            public int MaxStack { get; }
        }

        public class SpawnBird : IFireEvent
        {
            private JingburgerPassiveSkill skill;
            private BirdInfo _birdInfo;

            public List<UnityAction<JingPaint>> OnUse { get; }

            public bool Usable => CurStack >= _birdInfo.count;

            private int _curStack;

            public int CurStack
            {
                get => _curStack;
                set
                {
                    _curStack = Mathf.Clamp(value, 0, MaxStack);
                    skill.OnAnimalStackChange?.Invoke();
                }
            }

            public int MaxStack => _birdInfo.count;

            public int Priority => 3;

            public SpawnBird(JingburgerPassiveSkill skill, BirdInfo birdInfo)
            {
                this.skill = skill;
                _birdInfo = birdInfo;
                CurStack = 0;
                OnUse = new() { Invoke };
            }

            void Atk(EventParameters eventParameters)
            {
                EventParameters temp = new(eventParameters.user, eventParameters.target)
                {
                    atkData = new()
                    {
                        groggyAmount = _birdInfo.groggy,
                        atkStrategy = new AtkItemCalculation(skill.user as Actor ,skill, _birdInfo.dmg / 4f),
                        attackType = Define.AttackType.Extra
                    }
                };
                skill.attacker?.Attack(temp);
            }
            public Tween Spawn(EventParameters eventParameters)
            {
                if (eventParameters?.target == null) return null;

                Sequence seq = DOTween.Sequence();

                seq.AppendCallback(() =>
                {
                    GameManager.Factory.Get<ParticleDestroyer>(FactoryManager.FactoryType.Effect,
                        Define.PlayerEffect.JingAnimalBird,
                        eventParameters.target.Position);
                });
                seq.AppendInterval(0.51f);
                seq.AppendCallback(() => Atk(eventParameters));
                seq.AppendInterval(0.22f);
                seq.AppendCallback(() => Atk(eventParameters));
                seq.AppendInterval(0.2f);
                seq.AppendCallback(() => Atk(eventParameters));
                seq.AppendInterval(0.1f);
                seq.AppendCallback(() => Atk(eventParameters));
                
                return seq;
            }

            public void Invoke(JingPaint paint)
            {
                paint.AttackSequence.Add(Spawn);
            }
        }

        public class SpawnPuppy : IFireEvent
        {
            public int Priority => 2;

            public List<UnityAction<JingPaint>> OnUse { get; }

            public void Invoke(JingPaint paint)
            {
                paint.AttackSequence.Add(Spawn);
            }

            public bool Usable => CurStack >= _info.count;

            private int _curStack;

            public int CurStack
            {
                get => _curStack;
                set
                {
                    _curStack = Mathf.Clamp(value, 0, MaxStack);
                    skill.OnAnimalStackChange?.Invoke();
                }
            }

            public int MaxStack => _info.count;

            private JingburgerPassiveSkill skill;
            private DogInfo _info;

            public SpawnPuppy(JingburgerPassiveSkill skill, DogInfo _info)
            {
                this.skill = skill;
                this._info = _info;
                CurStack = 0;
                OnUse = new();
                OnUse.Add(Invoke);
            }

            Tween Spawn(EventParameters parameters)
            {
                if (parameters?.target == null)
                    return null;

                Sequence seq = DOTween.Sequence();
                Vector2 targetPos = parameters.target is Actor a ? a.Position : parameters.target.transform.position;

                GameObject puppy = GameManager.Factory.Get(FactoryManager.FactoryType.Normal,
                    Define.PlayerSkillObjects.JingPuppyPaint, targetPos + Vector2.left);

                puppy.SetActive(false);
                seq.AppendCallback(() => { puppy.SetActive(true); });

                seq.Append(puppy.transform.DOMoveX(targetPos.x + 1, 0.5f).SetEase(Ease.Linear));

                seq.onComplete += () => { GameManager.Factory.Return(puppy); };
                for (int i = 0; i < _info.atkCount; i++)
                {
                    seq.InsertCallback(0.25f + 0.05f * i, () =>
                    {
                        EventParameters temp = new EventParameters(parameters.user, parameters.target)
                        {
                            atkData = new()
                            {
                                groggyAmount = _info.groggy,
                                atkStrategy = new AtkItemCalculation(skill.user as Actor , skill , _info.dmg),
                                attackType = Define.AttackType.Extra
                            }
                        };
                        skill.attacker?.Attack(temp);
                    });
                }

                return seq;
            }
        }

        public class SpawnWhale : IFireEvent
        {
            private JingburgerPassiveSkill skill;
            private WhaleInfo whaleInfo;

            public List<UnityAction<JingPaint>> OnUse { get; }

            public bool Usable => CurStack >= whaleInfo.count;

            private int _curStack;

            public int CurStack
            {
                get => _curStack;
                set
                {
                    _curStack = Mathf.Clamp(value, 0, MaxStack);
                    skill.OnAnimalStackChange?.Invoke();
                }
            }

            public int MaxStack => whaleInfo.count;

            public int Priority => 1;

            public SpawnWhale(JingburgerPassiveSkill skill, WhaleInfo whaleInfo)
            {
                this.skill = skill;
                this.whaleInfo = whaleInfo;
                CurStack = 0;
                OnUse = new();
                OnUse.Add(Invoke);
            }

            public Tween Spawn(EventParameters eventParameters)
            {
                if (eventParameters?.target == null) return null;

                GameObject whale = GameManager.Factory.Get(FactoryManager.FactoryType.Normal,
                    Define.PlayerSkillObjects.JingWhalePaint, eventParameters.target.transform.position);
                whale.SetActive(false);

                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() => whale.SetActive(true));
                seq.InsertCallback(0.5f, () =>
                {
                    EventParameters temp = new EventParameters(eventParameters.user, eventParameters.target)
                    {
                        atkData = new()
                        {
                            groggyAmount = whaleInfo.groggy,
                            atkStrategy = new AtkItemCalculation(skill.user as Actor ,skill , whaleInfo.dmg),
                            attackType = Define.AttackType.Extra
                        }
                    };

                    skill.attacker?.Attack(temp);
                });
                seq.InsertCallback(1f, () => { GameManager.Factory.Return(whale); });

                return seq;
            }

            public void Invoke(JingPaint paint)
            {
                paint.AttackSequence.Add(Spawn);
            }
        }
    }
}