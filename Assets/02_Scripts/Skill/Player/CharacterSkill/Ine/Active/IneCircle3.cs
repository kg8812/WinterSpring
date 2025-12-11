using System.Collections;
using System.Collections.Generic;
using chamwhy;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "IneCircle3", menuName = "Scriptable/Skill/Ine/Circle3")]
    public class IneCircle3 : IneCircleMagic
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override float Mana => playerSkill.Circle3Mana;

        protected override int circle => 3;
        
        protected override void Magic()
        {
            Circle3();
        }

        void Circle3()
        {
            playerSkill.mana -= playerSkill.Circle3Mana;
            SpawnEffect(Define.PlayerEffect.Ine_Magic_3Circle_Shoot, 0.5f, user.Position,false);

            Projectile moon = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.Ine_Magic_3Circle_MoonShard, user.Position);
            moon.transform.localScale = Vector3.one * (playerSkill.Circle3Radius * 2);
            
            moon.Init(attacker,
                new AtkItemCalculation(playerSkill.user as Actor, this, playerSkill.circle3Info.dmg));
            moon.Init(playerSkill.circle3Info);
            moon.Init(playerSkill.Circle3groggy);
            moon.AddEventUntilInitOrDestroy(SpawnMoon);
            moon.Fire();
            moon.AddEventUntilInitOrDestroy(
                x => SpawnEffect(Define.PlayerEffect.IneFeatherHit, playerSkill.circle3Radius,
                    x.user.transform.position,false),
                EventType.OnDestroy);

            void SpawnMoon(EventParameters a)
            {
                var cam = CameraManager.instance.PlayerCam;

                if (a?.target == null || cam == null) return;

                GameObject totem = GameManager.Factory.Get(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.Ine_Magic_3Circle_MoonTotem, a.user.transform.position);

                var ray = Physics2D.Raycast(a.target.transform.position, Vector2.down, Mathf.Infinity,
                    LayerMasks.GroundAndPlatform);
                float height = cam.m_Lens.OrthographicSize * 2;
                float width = height * cam.m_Lens.Aspect;
                Vector2 screenSize = new Vector2(width, height);
                float distance = screenSize.magnitude;

                Vector2 movePos = ray.collider != null ? ray.point : a.target.transform.position;
                float sin = Mathf.Sin(Mathf.Deg2Rad * playerSkill.meteorAngle);
                float cos = Mathf.Cos(Mathf.Deg2Rad * playerSkill.meteorAngle);


                float x = cos * distance;
                float y = sin * distance;
                Vector2 xy = new Vector2(x, y);
                Vector2 spawnPos = movePos + xy;

                GameObject meteor = GameManager.Factory.Get(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.Ine_Magic_3Circle_Moon, spawnPos);
                meteor.transform.localScale = Vector3.one * (2 * playerSkill.MeteorRadius);

                Vector2 pos = totem.transform.position;
                var tween = DOTween.Sequence();
                tween.Append(totem.transform.DOMoveY(pos.y + 0.1f, 1f).SetEase(Ease.InOutSine));
                tween.Append(totem.transform.DOMoveY(pos.y - 0.1f, 1f).SetEase(Ease.InOutSine));

                tween.SetLoops(-1, LoopType.Yoyo);

                meteor.transform.DOMove(movePos, playerSkill.meteorSpeed).SetEase(playerSkill.meteorEase).SetSpeedBased().onComplete += () =>
                {
                    AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                        Define.PlayerEffect.Ine_Magic_3Circle_MoonExplode, meteor.transform.position);
                    exp.transform.localScale = Vector3.one * playerSkill.Circle3ExpRadius * 2;
                    exp.Init(attacker,
                        new AtkItemCalculation(playerSkill.user as Actor, this, playerSkill.meteorAtkInfo.dmg),1);
                    exp.Init(playerSkill.meteorAtkInfo);
                    exp.Init(playerSkill.Circle3ExpGroggy);
                    playerSkill.OnMeteorCollide.Invoke(movePos);

                    GameManager.Factory.Return(meteor);
                    GameManager.Factory.Return(totem);
                    tween?.Kill();
                    playerSkill.AfterCircleUse.Invoke(3);
                };
            }
        }
    }
}