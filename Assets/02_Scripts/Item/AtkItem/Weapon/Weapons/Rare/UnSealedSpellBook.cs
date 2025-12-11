using System.Collections.Generic;
using chamwhy;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Apis
{
    public class UnSealedSpellBook : MagicWeapon
    {
        private IWeaponAttack iatk;
        [LabelText("마법줄기 크기")] public Vector2 size;
        [LabelText("마법줄기 지속시간")] public float duration;
        protected virtual MagicStemAtk.StemType stemType => MagicStemAtk.StemType.Magic;
        public override IWeaponAttack IAttack => iatk ??= new WhipAtk(this, size, duration, stemType);

        public class WhipAtk : MagicStemAtk
        {
            private List<Transform> list = new();

            public override UnityEvent OnAfterAtk
            {
                get
                {
                    if (ev == null)
                    {
                        ev = new();
                        ev.AddListener(RemoveCircles);
                    }

                    return ev;
                }
            }

            private UnityEvent ev;

            private float distance;
            
            protected override void GroundAtk(int index)
            {
                switch (index)
                {
                    case 0:
                        list.Add(GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "curseCircle",
                            player.transform.position + Vector3.up * 0.3f +
                            Vector3.right * -(int)player.Direction * 0.5f).transform);
                        list[0].SetParent(player.transform);
                        base.GroundAtk(index);
                        distance = Mathf.Abs(weapon.transform.position.x - player.Position.x);
                        break;
                    case 1:
                        list.Add(GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "curseCircle",
                            player.transform.position + Vector3.up * 0.6f +
                            Vector3.right * -(int)player.Direction * 0.5f).transform);
                        list.Add(GameManager.Factory.Get(FactoryManager.FactoryType.Normal, "curseCircle",
                            player.transform.position + Vector3.up * 0.9f +
                            Vector3.right * -(int)player.Direction * 0.5f).transform);
                        list[1].SetParent(player.transform);
                        list[2].SetParent(player.transform);
                        base.GroundAtk(index);
                        distance = Mathf.Abs(weapon.transform.position.x - player.Position.x);
                        break;
                    case 2:
                        list.ForEach(x =>
                        {
                            AttackObject atk = GameManager.Factory.Get<AttackObject>(
                                FactoryManager.FactoryType.AttackObject,
                                stemName, x.transform.position + Vector3.right * (size.x / 2 + distance) * (int)player.Direction);
                            atk.Collider.enabled = false;
                            atk.Destroy(duration, new()
                            {
                                () => { atk.Collider.enabled = true; }
                            });
                            atk.transform.localScale = new Vector2(size.x + distance + 0.5f, size.y);
                            GameManager.Factory.Return(x.gameObject);
                        });
                        Vector2 pos =
                            new Vector2(list[0].transform.position.x + (size.x / 2 + distance) * (int)player.Direction,
                                (list[0].transform.position.y + list[^1].transform.position.y) / 2);

                        AttackObject atk =
                            GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.AttackObject,
                                "MagicStem", pos);
                        atk.transform.localScale = new Vector2(size.x + distance + 0.5f,
                            Mathf.Abs(list[0].transform.position.y - list[^1].transform.position.y) + size.y);
                        atk.Init(player, new AtkItemCalculation(player,weapon, weapon.groundAtkDmgs[2].dmg),
                            duration);
                        atk.Init(player.atkInfo);
                        atk.Init(Mathf.RoundToInt(weapon.BaseGroggyPower * weapon.groundAtkDmgs[2].groggy / 100f));
                        atk.AdditionalAtkCount = weapon.groundAtkDmgs[2].atkCount - 1;

                        list.Clear();

                        break;
                }
            }

            protected override void AirAtk(int index)
            {
                GroundAtk(index);
            }

            void RemoveCircles()
            {
                list.ForEach(x => { GameManager.Factory.Return(x.gameObject); });
                list.Clear();
            }

            public WhipAtk(Weapon weapon, Vector2 size, float duration, StemType type) : base(weapon, size, duration,
                type)
            {
            }
        }
    }
}