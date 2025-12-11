using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "IneCircle2", menuName = "Scriptable/Skill/Ine/Circle2")]
    public class IneCircle2 : IneCircleMagic
    {
        private AttackObject area;

        protected override ActiveEnums _activeType => ActiveEnums.Instant;

        protected override float Mana => playerSkill.Circle2Mana;

        protected override int circle => 2;

        protected override void Magic()
        {
            Circle2();
        }

        void Circle2()
        {
            playerSkill.mana -= playerSkill.Circle2Mana;
            area?.Destroy();
            SpawnEffectInPos(Define.PlayerEffect.Ine_Magic_2Circle_Start, Define.PlayerEffect.Ine_Magic_2Circle_Loop,
                playerSkill.Circle2Radius, Player.transform.position, false, x =>
                {
                    area = x.GetComponent<AttackObject>();
                    area.transform.localScale = Vector3.one * playerSkill.Circle2Radius;
                    area.Init(attacker,new AtkItemCalculation(playerSkill.user as Actor, this,playerSkill.circle2Info.dmg),playerSkill.Circle2Duration);
                    area.Init(playerSkill.circle2Info);
                    area.Init(playerSkill.Circle2Groggy);
                    area.hitEffect = Define.PlayerEffect.Ine_Magic_2Circle_Hit;
                    area.Frequency = playerSkill.Circle2AtkFrequency;
                    playerSkill.OnCircle2Spawn?.Invoke(area);
                    playerSkill.AfterCircleUse.Invoke(2);
                });
        }
    }
}