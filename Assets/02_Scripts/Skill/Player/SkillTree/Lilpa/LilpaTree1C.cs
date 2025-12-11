using System.Collections.Generic;
using Cinemachine;
using Default;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree1C : SkillTree
    {
        private LilpaPassiveAttachment attachment;
        private LilpaPassiveSkill skill;
        [LabelText("발동 필요 횟수")] public int needCount;
        [LabelText("회복 증가량(%)")] public float amount;
        [LabelText("이펙트 반경")] public float radius;
        [HideInInspector] public int count;

        private List<GameObject> bloods = new();
        
        void AddCount(int _)
        {
            if (count >= needCount)
            {
                count = 0;
                RemoveBloods();
                skill.isEnhanced = true;
                return;
            }
            count++;
            AddBlood();
        }

        private CircleEffect last;
        private Player player;
        void AddBlood()
        {
            GameObject appear = null;
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                appear =
                    player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaBlood_Appear, player.Position,false).gameObject;
                appear.transform.localPosition = Vector3.zero;
                appear.SetRadius(radius);
                SetCircleEffect(appear);
            });
            seq.AppendInterval(appear?.GetComponent<ParticleSystem>().main.duration ?? 0);
            seq.AppendCallback(() =>
            {
                GameObject loop = player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaBlood_Loop, appear.transform.position,false).gameObject;
                loop.SetRadius(radius);
                SetCircleEffect(loop);
                bloods.Add(loop);
                last = loop.GetComponent<CircleEffect>();
            });
            
        }

        void SetCircleEffect(GameObject obj)
        {
            CircleEffect blood = obj.GetOrAddComponent<CircleEffect>();
            blood.Init(skill.Player,1,50);
            
            if (last != null)
            {
                blood.move.Degree = last.move.Degree - 360f / needCount;
            }
            else
            {
                blood.move.Degree = 0;
            }
            blood.move.Update();
        }
        void RemoveBloods()
        {
            if (bloods == null) return;
            
            bloods.ForEach(x =>
            {
                player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaBlood_Dissolve, x.transform.position,false).gameObject.SetRadius(radius,player.Direction);
            });
            player.EffectSpawner.Remove(Define.PlayerEffect.LilpaBlood_Loop);
            bloods.Clear();
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            skill =  passive as LilpaPassiveSkill;
            player = passive.Player;
            bloods ??= new();
            if (skill == null) return;
            RemoveBloods();
            attachment ??= new LilpaTree1CAttachment(new(), this);
            skill.RemoveAttachment(attachment);
            skill.AddAttachment(attachment);
            skill.OnHeal.RemoveListener(AddCount);
            skill.OnHeal.AddListener(AddCount);
        }

        public override void DeActivate()
        {
            base.DeActivate();
            
            skill?.RemoveAttachment(attachment);
            skill?.OnHeal.RemoveListener(AddCount);
            RemoveBloods();
        }

        public class LilpaTree1CAttachment : LilpaPassiveAttachment
        {
            private LilpaTree1C tree;
            public LilpaTree1CAttachment(SkillStat stat,LilpaTree1C tree) : base(stat)
            {
                this.tree = tree;
            }

            private LilpaPassiveStat stat;
           
            public override LilpaPassiveStat LilpaStat
            {
                get
                {
                    stat ??= new();
                    stat.healRatio = tree.count >= tree.needCount ? tree.amount : 0;
                    return stat;
                }
            }
        }
    }
}