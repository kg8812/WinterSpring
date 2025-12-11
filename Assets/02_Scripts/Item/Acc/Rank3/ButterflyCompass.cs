using System.Collections;
using System.Collections.Generic;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class ButterflyCompass : Accessory
    {
        [LabelText("화살표 설정 시간")] public float time;
        [LabelText("공속 증가량")] public float atkSpeed;
        [LabelText("치명타 피해 증가량")] public float criticalDmg;

        private GameObject arrow;
        private EActorDirection _direction;

        private BonusStat stat;

        BonusStat StatEvent()
        {
            stat ??= new();
            if ((object)user == null) return stat;
            stat.SetValue(ActorStatType.AtkSpeed, user.Direction == _direction ? atkSpeed : 0);
            stat.SetValue(ActorStatType.CritDmg, user.Direction == _direction ? criticalDmg : 0);

            return stat;
        }
        void SetDirection()
        {
            int rand = Random.Range(0, 2);

            _direction = rand == 0 ? EActorDirection.Left : EActorDirection.Right;
            
            if (arrow != null)
            {
                arrow.transform.localScale = new Vector3((int)_direction, 1);
            }
        }
        
        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            arrow = GameManager.Factory.Get(FactoryManager.FactoryType.Normal,
                Define.AccessoryObjects.ButterflyCompassArrow, user.Position);
            CustomBoneFollower custom = SpineUtils.AddCustomBoneFollower(user.Mecanim,"ctrl",arrow);
            custom.offset = user.TopPivot;

            SetDirection();
            InvokeRepeating(nameof(SetDirection),time,time);
            user.BonusStatEvent += StatEvent;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            Destroy(arrow);
            CancelInvoke(nameof(SetDirection));
            user.BonusStatEvent -= StatEvent;
        }
        
        
    }
}