using chamwhy.DataType;
using Sirenix.OdinInspector;

namespace Apis
{
    public class InvalidityOrder : Accessory
    {
        [LabelText("치명타확률 증가량(%)")] public float crit;
        [LabelText("지속시간")] public float duration;

        private Buff _buff;

        protected override void OnEquip(IMonoBehaviour user1)
        {
            base.OnEquip(user1);
            BuffDataType data = new(SubBuffType.Buff_CritProb)
            {
                buffPower = new[]
                {
                    crit
                },
                buffMaxStack = 1, buffDuration = duration, buffDispellType = 1,
                valueType = ValueType.Value,showIcon = false
            };
            _buff = new(data, user);
            user.AddEvent(EventType.OnSkill,AddCrit);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            user.RemoveEvent(EventType.OnSkill,AddCrit);

        }

        void AddCrit(EventParameters param)
        {
            _buff.AddSubBuff(user, null);
        }
    }
}