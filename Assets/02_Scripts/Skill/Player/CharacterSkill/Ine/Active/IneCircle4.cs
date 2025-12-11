using System.Collections;
using System.Collections.Generic;
using chamwhy;
using chamwhy.DataType;
using Default;
using EventData;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "IneCircle4", menuName = "Scriptable/Skill/Ine/Circle4")]
    public class IneCircle4 : IneCircleMagic
    {
        protected override ActiveEnums _activeType => ActiveEnums.Instant;
        private InePassiveSkill passive;
        private Coroutine circle4Coroutine;

        protected override float Mana => playerSkill.circle4Mana;

        protected override int circle => 4;

        protected override void Magic()
        {
            circle4Coroutine = GameManager.instance.StartCoroutine(Circle4());
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            EndCircle4();
        }

        IEnumerator Circle4()
        {
            if (playerSkill.isCircle4) yield break;

            playerSkill.Use();
            GameManager.instance.StartCoroutine(SpawnSpears());
            
            Utils.AttackAllScreen(playerSkill.Player, new AttackEventData()
            {
                dmg = playerSkill.circle4Dmg, atkStrategy = new AtkItemCalculation(playerSkill.Player,playerSkill,playerSkill.circle4Dmg),
                attackType = Define.AttackType.PlayerSkillAttack
            }, LayerMasks.Enemy);
            
            skeleton?.Mecanim.skeleton.SetSkin("player_ine_moon");
            passive ??= passiveUser?.PassiveSkill as InePassiveSkill;
            passive?.Disable();
            InvenManager.instance.PresetManager.ApplyPreset(11);
            playerSkill.isCircle4 = true;
            IsUse = true;
            
            SpawnEffect(Define.PlayerEffect.Ine_4CircleForm, 0.5f,true);
            while (playerSkill.mana > 0)
            {
                playerSkill.mana -= playerSkill.circle4ManaUse * Time.deltaTime;
                yield return null;
            }

            EndCircle4();
        }
        
        IEnumerator SpawnSpears()
        {
            if (playerSkill.spearCount == 0) yield break;

            yield return new WaitForSeconds(playerSkill.spearFrequency);

            while (playerSkill.isCircle4)
            {
                for (int i = 1; i <= playerSkill.spearCount; i++)
                {
                    float rand = Random.Range(0, 180f);

                    // ReSharper disable once PossibleLossOfFraction
                    float angle = rand + 180 * (i / 2);

                    Projectile spear = GameManager.Factory.Get<Projectile>(
                        FactoryManager.FactoryType.Effect,
                        Define.PlayerEffect.Ine_LightSpear,
                        user.Position +
                        new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0) *
                        playerSkill.spearRadius);

                    spear.transform.right = spear.transform.position - user.Position;
                    spear.transform.localScale = Vector3.one;

                    spear.Init(attacker,
                        new AtkItemCalculation(attacker as Actor, this, playerSkill.spearInfo.dmg));
                    spear.Init(playerSkill.spearInfo);
                    spear.Init(playerSkill.spearGroggy);
                    spear.firstVelocity = spear.transform.right.normalized * playerSkill.spearInfo.firstVelocity.magnitude;
                    spear.Fire(false);
                    spear.AddEventUntilInitOrDestroy(
                        x => SpawnEffect(Define.PlayerEffect.IneFeatherHit, playerSkill.spearRadius,
                            x.user.transform.position,false), EventType.OnDestroy);

                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(playerSkill.spearFrequency);
            }
        }

        void EndCircle4()
        {
            if (!playerSkill.isCircle4) return;
            
            playerSkill.Player.ApplyPlayerPreset();
            playerSkill.isCircle4 = false;
            IsUse = false;
            passive?.Enable();
            
            RemoveEffect(Define.PlayerEffect.Ine_4CircleForm);
            SpawnEffect(Define.PlayerEffect.Ine_CircleChange, 0.5f, user.Position,false);
            skeleton?.Mecanim.skeleton.SetSkin("player_ine");

            if (circle4Coroutine != null)
            {
                GameManager.instance.StopCoroutineWrapper(circle4Coroutine);
                circle4Coroutine = null;
            }

            playerSkill.AfterCircleUse.Invoke(4);
        }
    }
}