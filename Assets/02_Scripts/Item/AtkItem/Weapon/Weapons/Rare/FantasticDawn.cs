using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Apis
{
    public class FantasticDawn : Weapon
    {
        public class BladeInfo
        {
            public GameObject blade;
            public Sprite[] sprites;
            public Material[] materials;
            public string[] slots;
        }
        //[Title("판타스틱 던")] [LabelText("검날 변경시간")]
        //public float time;

        //[LabelText("화상 확률")] public float burnProb;
        //[LabelText("냉기 확률")] public float chillProb;

        //private bool isStop;
        public override AttackCategory Category => AttackCategory.Sword;
        enum BladeType
        {
            Fire,Night,Dawn
        }

        private BladeType bladeType;

        //private float curTime;

        [SerializeField] Dictionary<BladeType, BladeInfo> blades;
        public override void Init()
        {
            base.Init();
            //curTime = 0;
        }
        
        protected override void UpdateFunc(EventParameters _)
        {
            base.UpdateFunc(_);
            // if (!isStop)
            // {
            //     if (curTime < time)
            //     {
            //         curTime += Time.deltaTime;
            //     }
            //     else
            //     {
            //         ChangeType();
            //         curTime = 0;
            //     }
            // }
        }

        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            // Player.AddEvent(EventType.OnBasicAttack,Invoke);
            // ChangeType(BladeType.Fire);

            //isStop = false;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            // Player.RemoveEvent(EventType.OnBasicAttack,Invoke);
            // isStop = true;
        }

        void SetBlade()
        {
            blades.Keys.ForEach(x =>
            {
                blades[x].blade.SetActive(x == bladeType);
            });
            if (Player.Attacher != null)
            {
                Player.Attacher.RemoveAll();

                Player.Attacher.sprites = blades[bladeType].sprites;
                Player.Attacher.materials = blades[bladeType].materials;
                Player.Attacher.slots = blades[bladeType].slots;
                
                Player.Attacher.Initialize();
                Player.Attacher.Attach();
            }
        }
        void ChangeType()
        {
            switch (bladeType)
            {
                case BladeType.Fire:
                    bladeType = BladeType.Night;
                    break;
                case BladeType.Night:
                    bladeType = BladeType.Fire;
                    break;
            }
            SetBlade();
        }

        void ChangeType(BladeType type)
        {
            bladeType = type;
            SetBlade();
        }
        
        BladeType temp;
        public void TurnOnBoth()
        {
            temp = bladeType;
            ChangeType(BladeType.Dawn);
            //isStop = true;
        }

        public void TurnOffBoth()
        {
            ChangeType(temp);
            //isStop = false;
        }
        // void Invoke(EventParameters parameters)
        // {
        //     if (parameters.target is Actor target)
        //     {
        //         switch (bladeType)
        //         {
        //             case BladeType.Fire:
        //                 ApplyBurn(target);
        //                 break;
        //             case BladeType.Night:
        //                 ApplyChill(target);
        //                 break;
        //             case BladeType.Dawn:
        //                 ApplyBurn(target);
        //                 ApplyChill(target);
        //                 break;
        //             default:
        //                 return;
        //         }
        //     }
        // }

        // void ApplyBurn(Actor target)
        // {
        //     float rand = Random.Range(0, 100f);
        //
        //     if (rand <= burnProb)
        //     {
        //         target.AddSubBuff(Player, SubBuffType.Debuff_Burn);
        //     } 
        // }
        //
        // void ApplyChill(Actor target)
        // {
        //     float rand = Random.Range(0, 100f);
        //
        //     if (rand <= chillProb)
        //     {
        //         target.AddSubBuff(Player, SubBuffType.Debuff_Chill);
        //     } 
        // }
    }
}