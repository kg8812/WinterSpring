// using UnityEngine;
//
// namespace chamwhy
// {
//     public class SMDeath : IState<CommonMonster>
//     {
//         private CommonMonster _cM;
//
//
//         public void OnEnter(CommonMonster monster)
//         { 
//             _cM = monster;
//             // Debug.LogError("monster death enter");
//             _cM.IsDead = true;
//             _cM.HitCollider.enabled = false;
//             _cM.Rb.bodyType = RigidbodyType2D.Static;
//             _cM.animator.SetTrigger("dead");
//             if (!ReferenceEquals(_cM.ItemDropper, null))
//             {
//                 _cM.ItemDropper.Drop();
//             }
//             GameManager.UI.CloseUI(_cM.hpBarUi);
//
//             GameManager.instance.BattleStateClass.RemoveRecogMonster(_cM);
//         }
//
//         public void Update()
//         {
//             // death state logic
//         }
//
//         public void FixedUpdate()
//         {
//             // physics logic for death state
//         }
//
//         public void OnExit()
//         {
//             // Debug.Log("monster exit");
//             _cM.gameObject.SetActive(false);
//             // MonsterManager.Instance.monster.Return(this.monster);
//         }
//     }
// }