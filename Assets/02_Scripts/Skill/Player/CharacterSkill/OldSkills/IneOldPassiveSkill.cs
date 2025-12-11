// using System.Collections;
// using GGDok;
// using Sirenix.OdinInspector;
// using Spine.Unity;
// using UnityEngine;
//
// namespace Apis
// {
//     [CreateAssetMenu(fileName = "InePassive", menuName = "Scriptable/Skill/InePassive")]
//
//     public class IneOldPassiveSkill : PassiveSkill
//     {
//         [HideInInspector] public IneMagicCircle obj;
//         [HideInInspector] public IneMagicCircle obj2;
//
//         [Title("아이네 패시브")]
//         [LabelText("마법진 반경(m)")] public float radius;
//         [LabelText("공격력 감소량(백분율)")] public float atkRatio;
//         [LabelText("디버프 적용 필요시간")] public float delayTime;
//
//         public override void OnEquip(Actor _actor)
//         {
//             base.OnEquip(_actor);
//
//             obj = GameManager.Factory.Get(FactoryManager.FactoryType.Effect, Define.PlayerEffect.IneMagicCircleMoon, Player.Position)
//                 .GetComponent<IneMagicCircle>();
//             obj.DelayTime = delayTime;
//             obj.atkRatio = atkRatio;
//             obj.transform.SetParent(Player.SubParent.transform);
//             obj2 = GameManager.Factory.Get(FactoryManager.FactoryType.Effect, Define.PlayerEffect.IneMagicCircleWing, Player.Position)
//                 .GetComponent<IneMagicCircle>();
//             obj2.DelayTime = delayTime;
//             obj2.atkRatio = atkRatio;
//             obj2.transform.SetParent(Player.SubParent.transform);
//
//             Utils.AddBoneFollower(Player._mecanim, "ctrl", obj.gameObject);
//             Utils.AddBoneFollower(Player._mecanim, "ctrl", obj2.gameObject);
//
//             obj.transform.GetChild(0).localScale = new Vector3(radius * 2, radius * 2);
//             obj2.transform.GetChild(0).localScale = new Vector3(radius * 2, radius * 2);
//             
//             obj.Init(Player,new Actor.AtkBase(Player));
//             obj2.Init(Player,new Actor.AtkBase(Player));
//
//             obj2.gameObject.SetActive(false);
//         }
//
//         public override void OnUnEquip()
//         {
//             base.OnUnEquip();
//             if (obj != null)
//             {
//                 obj.Destroy();
//             }
//
//             if (obj2 != null)
//             {
//                 obj2.Destroy();
//             }
//         }
//     }
// }