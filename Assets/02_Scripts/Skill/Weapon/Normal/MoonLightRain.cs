using System.Collections;
using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Apis
{
    public class MoonLightRain : MagicSkill
    {
        [Title("권총 스킬")] [LabelText("발사 개수")] public int count;
        [LabelText("발사 텀")] public float term;
        [LabelText("발사 높이")] [Tooltip("플레이어 발밑 기준")]
        public float height;
        [LabelText("발사 속도 (시간)")] public float speed;
        [LabelText("발사 Ease")] public Ease ease1;
        
        [LabelText("낙하 텀")] public float fireTerm;
        [LabelText("낙하 거리")] [Tooltip("플레이어 기준")] public float distance;

        [LabelText("낙하 속도")]
        public float speed2;

        public ProjectileInfo info;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        public override void Active()
        {
            base.Active();
            
            GameManager.instance.Player.SetState(EPlayerState.Skill);
            actionList.Clear();
            actionList.Add(FireBullets);
        }

        public void FireBullets()
        {
            GameManager.instance.StartCoroutine(SpawnBullets());
        }

        IEnumerator SpawnBullets()
        {
            Vector2 endPos = (Vector2)user.transform.position + Vector2.right * (direction != null ? (int)direction.Direction : 1 * distance);
            Vector2 movePos = (Vector2)user.transform.position + Vector2.up * height;

            float d = Vector2.Distance(movePos, endPos) * 2;
            

            for (int i = 0; i < count; i++)
            {
                Projectile obj = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.AttackObject, "bullet",
                    user.Position);
                
                obj.Init(attacker,new AtkBase(attacker,Atk));
                Vector2 rand = new Vector2(Random.Range(-0.75f, 0.75f), Random.Range(-0.2f, 0.2f));
                Vector2 dir = endPos + Vector2.right * Random.Range(-1f,1f) - movePos;
                obj.Init(info);
                dir.Normalize();

                Sequence seq = DOTween.Sequence();
                seq.Append(obj.transform.DOMove(movePos + rand, speed).SetEase(ease1));
                
                seq.AppendInterval(fireTerm);
                seq.AppendCallback(() =>
                {
                    obj.firstVelocity = dir * speed2;
                    obj.maxDistance = d;
                    obj.Fire(false);
                });

                yield return new WaitForSeconds(term);
            }
        }
    }

    
}