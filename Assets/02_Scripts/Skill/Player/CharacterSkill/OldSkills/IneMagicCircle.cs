// using System;
// using System.Collections.Generic;
// using Apis;
// using chamwhy;
// using chamwhy.DataType;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using ValueType = Apis.ValueType;
//
// public class IneMagicCircle : AttackObject
// {
//     private Buff buff;
//
//     private DelayContinuousAttack _attackType;
//     
//     private List<Actor> targets = new();
//     [HideInInspector] public float atkRatio;
//
//     public override void Init(AttackObjectInfo attackObjectInfo)
//     {
//         base.Init(attackObjectInfo);
//
//         BuffDataType data = new()
//         {
//             buffMainType = 2,buffSubType = 0,buffPower1 = atkRatio,buffCategory = 1,buffDuration = 0,buffDispellType = 1,
//             buffMaxStack = 1,valueType = ValueType.Ratio,showIcon = true,
//         };
//         buff = new(data,_actor);
//         
//         AddEvent(BuffEventType.OnTriggerExit,Remove);
//     }
//
//     protected override void AttackInvoke(ActorInfo info)
//     {
//         base.AttackInvoke(info);
//         
//         if (info?.target is not Actor a) return;
//         
//         targets.Add(a);
//         buff.AddSubBuff(a,null);
//     }
//
//     void Remove(ActorInfo info)
//     {
//         if (info?.target is Actor actor)
//         {
//             targets.Remove(actor);
//             actor.RemoveBuff(buff);
//         }
//     }
//
//     protected override void OnDisable()
//     {
//         base.OnDisable();
//         targets.ForEach(x => x.RemoveBuff(buff));
//         targets.Clear();
//     }
// }
