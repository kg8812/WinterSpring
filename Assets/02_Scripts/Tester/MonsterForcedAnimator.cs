// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace chamwhy
// {
//     [RequireComponent(typeof(CommonMonster))]
//     public class MonsterForcedAnimator: MonoBehaviour
//     {
//         private CommonMonster cm;
//         public int patternGroupId;
//
//         private void Awake()
//         {
//             cm = GetComponent<CommonMonster>();
//         }
//
//         [Button(ButtonSizes.Large)]
//         public void PlayPatternGroup()
//         {
//             cm.animator.enabled = true;
//             cm.animator.SetBool("recognized", true);
//             cm.animator.SetTrigger("exitDelay");
//             cm.animator.SetInteger("pGInd", patternGroupId);
//         }
//     }
// }