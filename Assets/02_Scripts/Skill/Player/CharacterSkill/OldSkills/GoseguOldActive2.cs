// using chamwhy;
// using UI;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace Apis
// {
//     [CreateAssetMenu(fileName = "GoseguActive2", menuName = "Scriptable/Skill/GoseguActive2")]
//     public class GoseguActive2 : PlayerActiveSkill
//     {
//         [Title("고세구 우클릭 스킬")] [LabelText("스킬 범위")]
//         public float radius;
//
//         [LabelText("2개 이하 데미지")] public float dmg1;
//         [LabelText("3개 이상 데미지")] public float dmg2;
//         [LabelText("폭발 반경")] public float radius2;
//         [LabelText("폭발 지속시간")] public float duration2;
//
//         protected override ActiveEnums _activeType => ActiveEnums.Instant;
//
//         public override bool Use()
//         {
//             if (TryUse())
//             {
//                 Player.PlayerActiveSkill.Use();
//             }
//             return base.Use();
//         }
//
//         public override void Active()
//         {
//             base.Active();
//
//             Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.Position, radius, LayerMasks.Enemy);
//
//             foreach (var x in colliders)
//             {
//                 if (x.TryGetComponent(out Actor target))
//                 {
//                     Attack(target);
//                 }
//             }
//         }
//
//         void Attack(Actor target)
//         {
//             int count = target.SubBuffCount(SubBuffType.RefinedAnger);
//
//             if(count == 0) return;
//             AttackObject explosion = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
//                 "AngerExplosion", target.Position);
//             
//             float value = (count <= 2 ? dmg1 : dmg2) * count;
//             explosion.transform.localScale = Vector2.one * (radius2 * 2);
//             explosion.Init(Player,new Actor.AtkBase(Player,value),Define.AttackType.SkillAttack,duration2);
//             target.RemoveType(SubBuffType.RefinedAnger);
//         }
//     }
// }