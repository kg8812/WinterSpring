using System.Collections.Generic;
using System.Linq;
using chamwhy;
using chamwhy.Managers;
using Sirenix.OdinInspector;

namespace Apis
{
    public class BoneHarpoon : ProjectileWeapon
    {
        private HarpoonAtk iAttack;
        [LabelText("최대 배치 개수")] public int maxCount;
        [LabelText("최대 배치 거리")] public float maxDistance;
        [LabelText("돌아오는 속도")] public float speed;
        [LabelText("회수 데미지(%)")] public float dmgRatio;
        public override IWeaponAttack IAttack => iAttack ??= new HarpoonAtk(this);

        public CustomQueue<Harpoon> harpoons => iAttack.queue;

        protected override void OnEquip(IMonoBehaviour user)
        {
            base.OnEquip(user);
            GameManager.Scene.WhenSceneLoaded.RemoveListener(Clear);
            GameManager.Scene.WhenSceneLoaded.AddListener(Clear);
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            while (harpoons.Count > 0)
            {
                harpoons.Dequeue()?.Destroy();
            }
            GameManager.Scene.WhenSceneLoaded.RemoveListener(Clear);
        }

        void Clear(SceneData _)
        {
            while (harpoons.Count > 0)
            {
                var x = harpoons.Dequeue();
                if (x != null) x.Destroy();
            }
        }

        public void CollectHarpoons()
        {
            while (harpoons.Count > 0)
            {
                var h = harpoons.Dequeue();
                h.speed2 = speed;
                h.DmgRatio = dmgRatio;
                h.Init(CalculateGroggy(BaseGroggyPower));
                h.ReturnToActor();
            }
        }
        public class HarpoonAtk : ProjectileAttack
        {
            private int maxCount => harpoon.maxCount;
            private float maxDistance => harpoon.maxDistance;
            public CustomQueue<Harpoon> queue = new();
            private BoneHarpoon harpoon;
            public HarpoonAtk(BoneHarpoon weapon) : base(weapon)
            {
                harpoon = weapon;
            }

            protected override FiredInfo FireGroundProjectile(int idx)
            {
                var list = base.FireGroundProjectile(idx);
                List<Harpoon> projs = list.projectiles.Select(x => x as Harpoon).Where(x => x != null).Distinct()
                    .ToList();

                foreach (var proj in projs)
                {
                    proj.Init(harpoon);
                    proj.OnStop.RemoveListener(PlaceNewHarpoon);
                    proj.OnStop.AddListener(PlaceNewHarpoon);
                }
                return list;
            }

            protected override FiredInfo FireAirProjectile(int idx)
            {
                var list = base.FireAirProjectile(idx);
                List<Harpoon> projs = list.projectiles.Select(x => x as Harpoon).Where(x => x != null).Distinct()
                    .ToList();

                foreach (var proj in projs)
                {
                    proj.Init(harpoon);
                    proj.OnStop.RemoveListener(PlaceNewHarpoon);
                    proj.OnStop.AddListener(PlaceNewHarpoon);
                }
                return list;
            }

            void PlaceNewHarpoon(Boomerang boo)
            {
                boo.OnStop.RemoveListener(PlaceNewHarpoon);

                if (boo is not Harpoon prj) return;
                
                queue.Enqueue(prj);

                if (queue.Count >= maxCount)
                {
                    harpoon.CollectHarpoons();
                }
            }
        }
    }
}