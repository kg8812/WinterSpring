// using chamwhy.DataType;
// using DG.Tweening;
// using Default;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace Apis
// {
//     public class ViichanOldPassiveSkill : PlayerPassiveSkill
//     {
//         [Title("비챤 패시브")] [LabelText("최대 게이지")]
//         public float maxGauge;
//
//         [LabelText("처치 게이지 획득량")] public float killGauge;
//         [LabelText("공격 게이지 획득량")] public float atkGauge;
//         [LabelText("피격 게이지 획득량")] public float hitGauge;
//
//         [Header("흑기사")]
//         [LabelText("흑기사 공격력 증가량")][Tooltip("백분율로 입력, 1 = 1% 증가")] public float atkRatio;
//         [LabelText("흑기사 이속 감소량")][Tooltip("백분율로 입력, 1 = 1% 증가")] public float moveRatio;
//         [LabelText("흑기사 쉴드 획득량")][Tooltip("백분율로 입력, 1 = 최대체력 1%")] public float shieldRatio1;
//         [LabelText("흑기사 데미지 반사량")][Tooltip("백분율로 입력, 1 = 1% 반사")] public float shieldRatio2;
//
//         [Header("성기사")]
//         [LabelText("방어 게이지 증가량")] [Tooltip("백분율로 입력, 1 = 1% 증가")]
//         public float gaugeRatio;
//         [LabelText("방어 게이지 회복 증가량")] [Tooltip("백분율로 입력, 1 = 1% 증가")]public float recoveryRatio;
//         [LabelText("성기사 방어력 증가량")] [Tooltip("절대값 입력")] public float defRatio2;
//         [LabelText("성기사 이속 증가량")] [Tooltip("백분율로 입력, 1 = 1% 증가")]public float moveRatio2;
//         
//         private ViichanActiveSkill activeSkill;
//         float curGauge;
//
//         private Buff blackKnightAtkBuff;
//         private Buff blackKnightMoveDebuff;
//         private Buff darkShieldBuff;
//
//         private Buff holyKnightDefBuff;
//         private Buff holyKnightMoveBuff;
//
//         private GameObject blackParticle;
//         private GameObject holyParticle;
//         
//         enum Knight
//         {
//             Black,Holy
//         }
//
//         private Knight knight;
//
//         public CommandExecutor blackKnightExecutor;
//         public CommandExecutor whiteKnightExecutor;
//
//         [HideInInspector] public UnityEvent OnBlackKnight = new();
//         [HideInInspector] public UnityEvent OnHolyKnight = new();
//         
//         public float CurGauge
//         {
//             get => curGauge;
//             private set
//             {
//                 if (IsUse) return;
//                 curGauge = Mathf.Clamp(value,0,maxGauge);
//             }
//         }
//
//         public override void AfterDuration()
//         {
//             base.AfterDuration();
//
//             switch (knight)
//             {
//                 case Knight.Black:
//                     //activeSkill.isBlackKnight = false;
//                     Player.RemoveEvent(BuffEventType.OnHit, Reflect);
//                     Player.RemoveEvent(BuffEventType.OnBarrierChange, RemoveEvent);
//                     GameManager.Factory.Return(blackParticle);
//                     GameManager.Sound.Play("viichanSkill_darkoff");
//                     break;
//                 case Knight.Holy:
//                     //activeSkill.MaxGauge /= 1 + gaugeRatio / 100;
//                     activeSkill.recover /= 1 + recoveryRatio / 100;
//                     activeSkill.CurGauge = Mathf.Clamp(activeSkill.CurGauge, 0, activeSkill.MaxGauge);
//                     GameManager.Factory.Return(holyParticle);
//                     GameManager.Sound.Play("viichanSkill_holyoff");
//                     break;
//             }
//         }
//
//         void Reflect(ActorInfo info)
//         {
//             if (info?.user is Actor target)
//             {
//                 ActorInfo x = new(Player, target)
//                 {
//                     dmg = info.dmg * shieldRatio2 / 100f,
//                 };
//
//                 target.OnHit(x);
//             }
//         }
//
//         public override void Cancel()
//         {
//             base.Cancel();
//             Player.RemoveBuff(blackKnightAtkBuff);
//             Player.RemoveBuff(blackKnightMoveDebuff);
//             Player.RemoveBuff(darkShieldBuff);
//             Player.RemoveBuff(holyKnightDefBuff);
//             Player.RemoveBuff(holyKnightMoveBuff);
//             gaugeTween?.Kill();
//             if (blackParticle != null && blackParticle.activeSelf)
//             {
//                 GameManager.Factory.Return(blackParticle);
//             }
//             if (holyParticle != null && holyParticle.activeSelf)
//             {
//                 GameManager.Factory.Return(holyParticle);
//             }
//
//             curGauge = 0;
//         }
//
//         public override bool TryUse()
//         {
//             return Mathf.Approximately(curGauge, maxGauge);
//         }
//
//         void AddKillGauge(ActorInfo _)
//         {
//             CurGauge += killGauge;
//         }
//
//         void AddAtkGauge(ActorInfo _)
//         {
//             CurGauge += atkGauge;
//         }
//
//         void AddHitGauge(ActorInfo info)
//         {
//             if (info.takenDmg > 0.02f)
//             {
//                 CurGauge += hitGauge;
//             }
//         }
//
//         private Tween gaugeTween;
//         public void BlackKnight(Player player)
//         {
//             if (Mathf.Approximately(CurGauge, maxGauge))
//             {
//                 Use();
//                 
//                 OnBlackKnight.Invoke();
//                 GameManager.Sound.Play("viichanSkill_darkon");
//                 blackParticle = GameManager.Factory.Get(FactoryManager.FactoryType.Effect,
//                     Define.PlayerEffect.ViichanThornForm,player.Position);
//                 Utils.AddBoneFollower(player._mecanim, "center", blackParticle);
//                 player.AddEvent(BuffEventType.OnHit,Reflect);
//                 gaugeTween = DOTween.To(() => curGauge, x => curGauge = x, 0, duration).SetEase(Ease.Linear);
//                 
//                 blackKnightAtkBuff.AddSubBuff(player,null);
//                 blackKnightMoveDebuff.AddSubBuff(player,null);
//                 darkShieldBuff.AddSubBuff(player,null);
//                 
//                 player.AddEvent(BuffEventType.OnBarrierChange,RemoveEvent);
//                    
//                 //activeSkill.BlackKnight();
//                 knight = Knight.Black;
//             }
//         }
//         void RemoveEvent(ActorInfo info)
//         {
//             if (Mathf.Approximately(Player.Barrier, 0))
//             {
//                 Player.RemoveEvent(BuffEventType.OnHit, Reflect);
//                 Player.RemoveEvent(BuffEventType.OnBarrierChange,RemoveEvent);
//             }
//         }
//         public void HolyKnight(Player player)
//         {
//             if (Mathf.Approximately(CurGauge, maxGauge))
//             {
//                 Use();
//
//                 OnHolyKnight.Invoke();
//                 GameManager.Sound.Play("viichanSkill_holyon");
//
//                 holyParticle = GameManager.Factory.Get(FactoryManager.FactoryType.Effect,
//                     Define.PlayerEffect.ViichanFlowerForm,player.Position);
//                 Utils.AddBoneFollower(player._mecanim, "center", holyParticle);
//                 gaugeTween = DOTween.To(() => curGauge, x => curGauge = x, 0, duration).SetEase(Ease.Linear);
//
//                 holyKnightDefBuff.AddSubBuff(player,null);
//                 holyKnightMoveBuff.AddSubBuff(player,null);
//
//                 //activeSkill.MaxGauge *= 1 + gaugeRatio / 100;
//                 activeSkill.CurGauge *= 1 + gaugeRatio / 100;
//                 activeSkill.recover *= 1 + recoveryRatio / 100;
//                 knight = Knight.Holy;
//             }
//         }
//         public override void Init()
//         {
//             base.Init();
//             
//             if (blackKnightAtkBuff == null)
//             {
//                 BuffDataType data1 = new()
//                 {
//                     buffMainType = 1, buffSubType = 0, buffPower1 = atkRatio, buffCategory = 1, buffDuration = duration,
//                     buffDispellType = 1, buffMaxStack = 1, valueType = ValueType.Ratio,showIcon = false
//                 };
//                 blackKnightAtkBuff = new Buff(data1, Player);
//             }
//
//             if (blackKnightMoveDebuff == null)
//             {
//                 BuffDataType data2 = new()
//                 {
//                     buffMainType = 2, buffSubType = 3, buffPower1 = moveRatio, buffCategory = 1, buffDuration = duration,
//                     buffDispellType = 1, buffMaxStack = 1, valueType = ValueType.Ratio,showIcon = false
//                 };
//                 blackKnightMoveDebuff = new Buff(data2, Player);
//             }
//             
//             if (darkShieldBuff == null)
//             {
//                 BuffDataType data3 = new()
//                 {
//                     buffMainType = 8, buffSubType = 0, buffPower1 = shieldRatio1,buffPower2 = shieldRatio2,buffCategory = 1, buffDuration = duration,
//                     buffDispellType = 1, buffMaxStack = 1, valueType = ValueType.Ratio,showIcon = false,applyStrategy = 0
//                 };
//                 darkShieldBuff = new Buff(data3, Player);
//             }
//
//             if (holyKnightDefBuff == null)
//             {
//                 BuffDataType data4 = new()
//                 {
//                     buffMainType = 1, buffSubType = 1, buffPower1 = defRatio2, buffCategory = 1, buffDuration = duration,
//                     buffDispellType = 1, buffMaxStack = 1, valueType = ValueType.Value, showIcon = false
//                 };
//                 holyKnightDefBuff = new Buff(data4, Player);
//             }
//
//             if (holyKnightMoveBuff == null)
//             {
//                 BuffDataType data5 = new()
//                 {
//                     buffMainType = 1, buffSubType = 3, buffPower1 = moveRatio2, buffCategory = 1, buffDuration = duration,
//                     buffDispellType = 1, buffMaxStack = 1, valueType = ValueType.Ratio, showIcon = false
//                 };
//
//                 holyKnightMoveBuff = new Buff(data5, Player);
//             }
//
//             activeSkill = Player.PlayerActiveSkill as ViichanActiveSkill;
//         }
//
//         protected override void OnEquip(Actor _actor)
//         {
//             base.OnEquip(_actor);
//             Player.AddEvent(BuffEventType.OnKill, AddKillGauge);
//             Player.AddEvent(BuffEventType.OnAttackSuccess, AddAtkGauge);
//             Player.AddEvent(BuffEventType.OnAfterHit, AddHitGauge);
//             //Player.Controller.inputSMouseL = blackKnightExecutor;
//             //Player.Controller.inputWMouseL = whiteKnightExecutor;
//         }
//
//         protected override void OnUnEquip()
//         {
//             base.OnUnEquip();
//             Cancel();
//             Player.RemoveEvent(BuffEventType.OnKill, AddKillGauge);
//             Player.RemoveEvent(BuffEventType.OnAttackSuccess, AddAtkGauge);
//             Player.RemoveEvent(BuffEventType.OnAfterHit, AddHitGauge);
//             //Player.Controller.inputSMouseL = null;
//             //Player.Controller.inputWMouseL = null;
//         }
//     }
// }