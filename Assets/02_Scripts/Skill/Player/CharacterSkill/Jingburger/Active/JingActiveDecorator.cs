// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public interface IJingActive : ISkill
// {
//     public JingActiveStat JingStat { get; }
// }
//
// public class JingActiveStat : SkillStat
// {
//     public int _maxStack;
//     public int count;
//     public float radius;
//
//     public float _maxStackRatio;
//     public float countRatio;
//     public float radiusRatio;
//
//     public JingActiveStat()
//     {
//         
//     }
//     public JingActiveStat(SkillStat stat)
//     {
//         baseCd = stat.baseCd;
//         maxStack = stat.maxStack;
//         duration = stat.duration;
//         dmg = stat.dmg;
//         groggy = stat.groggy;
//         baseCdRatio = stat.baseCdRatio;
//         maxStackRatio = stat.baseCdRatio;
//         durationRatio = stat.baseCdRatio;
//         dmgRatio = stat.baseCdRatio;
//         groggyRatio = stat.baseCdRatio;
//     }
//
//     public override void Reset()
//     {
//         base.Reset();
//         _maxStack = 0;
//         count = 0;
//         radius = 0;
//         _maxStackRatio = 0;
//         countRatio = 0;
//         radiusRatio = 0;
//     }
//
//     public override SkillStat Combine(SkillStat other)
//     {
//         if (other is JingActiveStat stat)
//         {
//             JingActiveStat c = new()
//             {
//                 baseCd = baseCd + other.baseCd,
//                 maxStack = maxStack + other.maxStack,
//                 duration = duration + other.duration,
//                 dmg = dmg + other.dmg,
//                 groggy = groggy + other.groggy,
//                 baseCdRatio = baseCdRatio + other.maxStackRatio,
//                 maxStackRatio = baseCdRatio + other.maxStackRatio,
//                 durationRatio = baseCdRatio + other.maxStackRatio,
//                 dmgRatio = baseCdRatio + other.maxStackRatio,
//                 groggyRatio = baseCdRatio + other.maxStackRatio,
//                 
//                 _maxStack = _maxStack + stat._maxStack,
//                 count = count + stat.count,
//                 radius = radius + stat.radius,
//                 _maxStackRatio = _maxStackRatio + stat._maxStackRatio,
//                 countRatio = countRatio + stat.countRatio,
//                 radiusRatio = radiusRatio + stat.radiusRatio,
//             };
//
//             return c;
//         }
//         
//         return new JingActiveStat(base.Combine(other));
//     }
//
//     public override SkillStat Subtract(SkillStat other)
//     {
//         if (other is JingActiveStat stat)
//         {
//             JingActiveStat c = new()
//             {
//                 baseCd = baseCd - other.baseCd,
//                 maxStack = maxStack - other.maxStack,
//                 duration = duration - other.duration,
//                 dmg = dmg - other.dmg,
//                 groggy = groggy - other.groggy,
//                 baseCdRatio = baseCdRatio - other.maxStackRatio,
//                 maxStackRatio = baseCdRatio - other.maxStackRatio,
//                 durationRatio = baseCdRatio - other.maxStackRatio,
//                 dmgRatio = baseCdRatio - other.maxStackRatio,
//                 groggyRatio = baseCdRatio - other.maxStackRatio,
//                 
//                 _maxStack = _maxStack - stat._maxStack,
//                 count = count - stat.count,
//                 radius = radius - stat.radius,
//                 _maxStackRatio = _maxStackRatio - stat._maxStackRatio,
//                 countRatio = countRatio - stat.countRatio,
//                 radiusRatio = radiusRatio - stat.radiusRatio,
//             };
//
//             return c;
//         }
//         return new JingActiveStat(base.Subtract(other));
//     }
//
//     public static JingActiveStat operator +(JingActiveStat a, JingActiveStat b)
//     {
//         SkillStat c = a.Combine(b);
//         return c as JingActiveStat;
//     }
//     
//     public static JingActiveStat operator -(JingActiveStat a, JingActiveStat b)
//     {
//         SkillStat c = a.Subtract(b);
//         return c as JingActiveStat;
//     }
// }
//
// public class JingActiveAttachment : SkillAttachment,IJingActive
// {
//     public JingActiveAttachment(SkillStat stat) : base(stat)
//     {
//         JingStat = stat as JingActiveStat;
//     }
//
//     public JingActiveStat JingStat { get; }
// }
// public class JingActiveConfig : SkillConfig,IJingActive
// {
//     public JingActiveConfig(SkillStat stat) : base(stat)
//     {
//         JingStat = stat as JingActiveStat;
//     }
//
//     public JingActiveStat JingStat { get; }
// }
// public class JingActiveDecorator : SkillDecorator,IJingActive
// {
//     public JingActiveDecorator(ISkill skill, ISkill attachment) : base(skill, attachment)
//     {
//         config = skill as IJingActive;
//         this.attachment = attachment as IJingActive;
//     }
//     
//     private readonly IJingActive config;
//     private readonly IJingActive attachment;
//     public JingActiveStat JingStat => config?.JingStat + attachment?.JingStat;
//
// }