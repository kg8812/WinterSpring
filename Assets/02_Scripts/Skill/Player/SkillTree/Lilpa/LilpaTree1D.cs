using System.Collections;
using chamwhy;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaTree1D : SkillTree
    {
        private LilpaPassiveSkill skill;
        private float curTime;
        [LabelText("발동 필요 스택")] public int necessaryStack;
        [LabelText("혈기왕성 지속시간")] public float duration;
        [LabelText("혈기왕성 이펙트 반경")] public float auraRadius;
        [LabelText("혈기 공격설정")] public ProjectileInfo info;
        [LabelText("혈기 그로기 수치")] public int groggy;
        [LabelText("혈기 반경")] public float radius;
        [LabelText("이속 증가량(%)")] public float speed;
        [LabelText("공속 증가량(%)")] public float atkSpeed;

        private BonusStat stat;
        private Player player;
        
        BonusStat StatEvent()
        {
            return stat;
        }
        public override void Activate(PlayerPassiveSkill passive, int level)
        {
            base.Activate(passive,level);
            
            skill = passive as LilpaPassiveSkill;
            isBlood = false;
            if(skill == null) return;
            skill.OnHeal.RemoveListener(TurnOnBloodMode);
            skill.OnHeal.AddListener(TurnOnBloodMode);
            stat ??= new BonusStat();
            stat.Reset();
            stat.AddRatio(ActorStatType.MoveSpeed,speed);
            stat.AddValue(ActorStatType.AtkSpeed,atkSpeed);
            player = skill.Player;
        }

        public override void DeActivate()
        {
            base.DeActivate();
            skill?.OnHeal.RemoveListener(TurnOnBloodMode);
            EndBloodMode();
        }

        private Coroutine _coroutine;
        void TurnOnBloodMode(int stack)
        {
            if (stack >= necessaryStack)
            {
                curTime = duration;
                _coroutine = GameManager.instance.StartCoroutineWrapper(BloodModeCoroutine());
            }
        }

        void SpawnBloodSlash(EventParameters _)
        {
            if (skill == null) return;

            AttackObject slash = player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaBloodSlash, "center",false).GetComponent<AttackObject>();
            
            slash.gameObject.SetRadius(radius,player.Direction);
            slash.Init(skill.attacker, new AtkItemCalculation(skill.user as Actor , skill , info.dmg),1);
            slash.Init(info);
            slash.Init(groggy);
        }
        private bool isBlood;
        private ParticleDestroyer loopEffect;
        IEnumerator BloodModeCoroutine()
        {
            if (isBlood) yield break;
            player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaHighSpiritAppear, "center",false).gameObject.SetRadius(auraRadius);
            loopEffect = player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaHighSpiritLoop, "center",false);
            loopEffect.gameObject.SetRadius(auraRadius);
            skill.Player.AddEvent(EventType.OnAttack,SpawnBloodSlash);
            skill.Player.BonusStatEvent += StatEvent;
            isBlood = true;
            curTime = duration;
            while (curTime > 0)
            {
                curTime -= Time.deltaTime;
                yield return null;
            }
            
            EndBloodMode();
        }

        void EndBloodMode()
        {
            isBlood = false;
            skill.Player.RemoveEvent(EventType.OnAttack,SpawnBloodSlash);
            skill.Player.BonusStatEvent -= StatEvent;
            player.EffectSpawner.Remove(loopEffect);
            player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaHighSpiritDissolve, "center",false).gameObject.SetRadius(auraRadius);
            if (_coroutine != null)
            {
                GameManager.instance.StopCoroutineWrapper(_coroutine);
            }
            _coroutine = null;
        }
    }
}