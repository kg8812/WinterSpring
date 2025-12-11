using chamwhy;
using chamwhy.DataType;
using UnityEngine;

namespace Apis
{
    public class GuardianSpearEffect : Projectile
    {
        private Buff buff;

        [HideInInspector] public int stack;
        [HideInInspector] public int maxStack;

        [HideInInspector] public float stackDuration;
        [HideInInspector] public float stackAmount;

        protected override void AttackInvoke(EventParameters parameters)
        {
            base.AttackInvoke(parameters);

            if (parameters.target is Actor act)
            {
                if (buff == null)
                {
                    BuffDataType data = new(SubBuffType.MarkStack)
                    {
                        buffDuration = stackDuration,
                        buffPower = new[] {stackAmount},
                        buffMaxStack = maxStack,
                        buffCategory = 1, buffDispellType = 1, stackDecrease = 0, showIcon = true,valueType = ValueType.Value
                    };
                    buff = new(data,_eventUser);
                }

                for (int i = 0; i < stack; i++)
                {
                    buff.AddSubBuff(act,null);
                }
            }

        }
    }
}