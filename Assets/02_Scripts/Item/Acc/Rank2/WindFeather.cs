using System.Collections;
using System.Collections.Generic;
using chamwhy;
using chamwhy.DataType;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class WindFeather : Accessory
    {
        [LabelText("쿨타임")] public float cd;
        [LabelText("깃털 발사 개수")] public int count;
        [LabelText("깃털 투사체 정보")] public ProjectileInfo projInfo;
        [LabelText("치명타 확률 증가량")] public float crit;
        [LabelText("치명타 증가 지속시간")] public float duration;
        private Buff _buff;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            BuffDataType data = new(SubBuffType.Buff_CritProb)
            {
                buffPower = new[] { crit }, valueType = ValueType.Value,
                buffCategory = 1, buffDuration = duration, buffMaxStack = 0,
                stackDecrease = 1, showIcon = false
            };
            _buff = new(data, user);
            user.AddEvent(EventType.OnHit,SpawnFeathers);
            isCd = false;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user?.RemoveEvent(EventType.OnHit,SpawnFeathers);
        }

        void SpawnFeathers(EventParameters _)
        {
            if (isCd) return;

            GameManager.instance.StartCoroutineWrapper(CDCoroutine(cd));
            float angle = 360f / count;
            
            for (int i = 0; i < count; i++)
            {
                float rad = Mathf.Deg2Rad * (angle * i);
                float x = Mathf.Sin(rad);
                float y = Mathf.Cos(rad);

                Vector2 pos = user.Position + new Vector3(x, y, 0);

                Projectile feather = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.IneFeather, pos);
                feather.Init(user,new FixedAmount(projInfo.dmg));
                feather.Init(projInfo);
                feather.LookAt(feather.Position - user.Position);
                feather.Fire(false);
                feather.AddAtkEventOnce(_ =>
                {
                    _buff.AddSubBuff(GameManager.instance.Player,null);
                });
            }
        }
    }
}