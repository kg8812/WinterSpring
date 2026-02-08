using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    public class DesireSword : Weapon
    {
        public class DesireSwordData : EquipmentSaveData
        {
            public Evolution _curEvolution;
            public float curStack;
            private readonly DesireSword _sword;
            
            public DesireSwordData(DesireSword sword)
            {
                _sword = sword;
            }
            public override void OnItemSave()
            {
                base.OnItemSave();
            }

            public override void OnItemLoad()
            {
                base.OnItemLoad();
                if (_sword.IsEquip)
                {
                    _sword.Player._overrider.SetAnimation(_sword.MotionIndex, _sword.Player);
                }
                _sword.SetEvolution();
            }
        }

        DesireSwordData _desireSwordData;
        private DesireSwordData data => _desireSwordData ??= new(this);

        public override EquipmentSaveData SaveData => data;

        [Title("욕망의 검")] [LabelText("2단계 필요원념")]
        public float gold2;

        [LabelText("3단계 필요원념")] public float gold3;

        public enum Evolution
        {
            First,Second,Third
        }
        
        public Evolution CurEvolution
        {
            get => data._curEvolution;
            set
            {
                data._curEvolution = value;

                if (IsEquip)
                {
                    Player._overrider.SetAnimation(MotionIndex, Player);
                }
            }
        }

        public override int MotionIndex
        {
            get
            {
                return CurEvolution switch
                {
                    Evolution.Second => 30301,
                    Evolution.Third => 30302,
                    _ => dataId
                };
            }
        }

        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            GameManager.instance.OnSoulChange.AddListener(AddStack);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            GameManager.instance.OnSoulChange.RemoveListener(AddStack);
        }

        void AddStack(int gold)
        {
            if (gold > 0)
            {
                data.curStack += gold;
            }
            SetEvolution();
        }
        void SetEvolution()
        {
            switch (CurEvolution)
            {
                case Evolution.First:
                    if (data.curStack >= gold2)
                    {
                        CurEvolution = Evolution.Second;
                    }
                    break;
                case Evolution.Second:
                    if (data.curStack >= gold3)
                    {
                        CurEvolution = Evolution.Third;
                    }
                    break;
            }
        }
    }
}