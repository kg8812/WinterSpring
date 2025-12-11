using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "IneCircle1", menuName = "Scriptable/Skill/Ine/Circle1")]
    public class IneCircle1 : IneCircleMagic
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override float Mana => 0;

        protected override int circle => 1;
        
        protected override void Magic()
        {
            GameManager.instance.StartCoroutine(Circle1());
        }

        IEnumerator Circle1()
        {
            eventUser.EventManager.ExecuteEvent(EventType.OnAttack, new(eventUser));

            for (int i = 0; i < playerSkill.Circle1Count; i++)
            {
                float _curTime = 0;
                Vector3 pos = playerSkill.circle1Pos;
                pos.x *= Player.DirectionScale;
                SpawnEffect(Define.PlayerEffect.Ine_Magic_1Circle_Shoot, 0.5f, user.Position + pos,false);
                Projectile moon = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.Ine_Magic_1Circle_Shard, user.Position + pos);
                moon.transform.localScale = Vector3.one * (playerSkill.Circle1Radius * 2);
                moon.Init(attacker, new AtkItemCalculation(user as Actor, this, playerSkill.circle1Info.dmg));
                moon.Init(playerSkill.circle1Info);
                moon.Init(playerSkill.Circle1groggy);
                moon.AddEventUntilInitOrDestroy(AddMana);
                moon.AddEventUntilInitOrDestroy(x => SpawnEffect(Define.PlayerEffect.IneFeatherHit,
                    playerSkill.circle1Radius, x.user.transform.position,false), EventType.OnDestroy);
                moon.Fire();
                while (_curTime < 0.1f)
                {
                    _curTime += Time.deltaTime;
                    yield return null;
                }
            }

            void AddMana(EventParameters _)
            {
                playerSkill.mana += playerSkill.ManaGain;
            }
        }
    }
}