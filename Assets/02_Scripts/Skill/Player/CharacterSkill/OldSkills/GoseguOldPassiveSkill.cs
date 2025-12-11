// using GGDok;
// using Sirenix.OdinInspector;
// using Spine.Unity;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace Apis
// {
//     public class GoseguOldPassiveSkill : PassiveSkill
//     {
//        //  private GoseguSubDrone leftSub;
//        //  private GoseguSubDrone rightSub;
//        //  private GoseguMainDrone main;
//        //  
//        //  [Title("서브 드론")]
//        //  [LabelText("감지범위")] public float distance1;
//        //  [LabelText("데미지 %")] public float dmg1;
//        //  [LabelText("투사체 속도")] public float speed1;
//        //  [FormerlySerializedAs("size1")] [LabelText("투사체 크기")] public float radius1;
//        //  [LabelText("공격 쿨타임")] public float cd1;
//        //  [LabelText("공격력")] public float atk1;
//        //
//        //  public override void OnEquip(Actor _actor)
//        //  {
//        //      base.OnEquip(_actor);
//        //
//        //      main = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "GoseguMainDrone")
//        //          .GetComponent<GoseguMainDrone>();
//        //      leftSub = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "GoseguSubDrone")
//        //          .GetComponent<GoseguSubDrone>();
//        //      
//        //      rightSub = GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "GoseguSubDrone")
//        //          .GetComponent<GoseguSubDrone>();
//        //      
//        //      leftSub.distance = distance1;
//        //      leftSub.dmg = dmg1;
//        //      leftSub.speed = speed1;
//        //      leftSub.radius = radius1;
//        //      leftSub.cd = cd1;
//        //      leftSub.BaseStat.Set(ActorStatType.Atk,atk1);
//        //      
//        //      rightSub.distance = distance1;
//        //      rightSub.dmg = dmg1;
//        //      rightSub.speed = speed1;
//        //      rightSub.radius = radius1;
//        //      rightSub.cd = cd1;
//        //      rightSub.BaseStat.Set(ActorStatType.Atk,atk1);
//        //
//        //      leftSub.SetMaster(Player);
//        //      rightSub.SetMaster(Player);
//        //      main.SetMaster(Player);
//        //      
//        //      SetTransform(main.gameObject,Vector3.up * 0.75f);
//        //      SetTransform(leftSub.gameObject,new Vector3(1,0.5f,0));
//        //      SetTransform(rightSub.gameObject,new Vector3(-1,0.5f,0));
//        //      
//        //  }
//        //
//        //  public override void OnUnEquip()
//        //  {
//        //      base.OnUnEquip();
//        //
//        //      GameManager.Factory.Return(main.gameObject);
//        //      GameManager.Factory.Return(leftSub.gameObject);
//        //      GameManager.Factory.Return(rightSub.gameObject);
//        //  }
//        //
//        //  void SetTransform(GameObject obj,Vector3 pivot)
//        //  {
//        //      obj.transform.SetParent(Player.transform);
//        //      CustomBoneFollower boneFollower = Utils.GetOrAddComponent<CustomBoneFollower>(obj);
//        //      boneFollower.skeletonRenderer = Player._skeletonTrans.GetComponent<SkeletonMecanim>();
//        //
//        //      boneFollower.boneName = "center";
//        //      boneFollower.offset = Player.topPivot + pivot;
//        //      boneFollower.Initialize();
//        //  }
//        //  public override void Cancel()
//        //  {
//        //      base.Cancel();
//        //
//        //      GameManager.Factory.Return(main.gameObject);
//        //      GameManager.Factory.Return(leftSub.gameObject);
//        //      GameManager.Factory.Return(rightSub.gameObject);
//        //  }
//     }
// }