using System.Collections.Generic;
using chamwhy;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis
{
    [CreateAssetMenu(fileName = "MeteorSkill", menuName = "Scriptable/Skill/Weapon/MeteorSkill")]
    public class MeteorSkill : MagicSkill
    {
        protected override ActiveEnums _activeType => ActiveEnums.Charge;

        [TabGroup("사용 관련/Charge/설정", "차징 설정")]  [LabelText("메테오 차징 정보")]
        public List<MeteorInfo> meteorInfos;

        [TitleGroup("스탯값")] [LabelText("소환 각도 (우측 기준)")] [SerializeField]
        public float meteorAngle;

        [TitleGroup("스탯값")] [LabelText("떨어지는 속도")] [SerializeField]
        public float meteorSpeed;

        [TitleGroup("스탯값")] [LabelText("떨어지는 Ease")] [SerializeField]
        public Ease meteorEase;

        private float meteorRadius;
        private float expRadius;

        [System.Serializable]
        public struct MeteorInfo
        {
            [LabelText("메테오 반경")] public float radius;
            [LabelText("폭발 반경")] public float expRadius;
        }

        private Tween tween;

        protected override void ChargeInvoke(int idx)
        {
            base.ChargeInvoke(idx);
            meteorRadius = meteorInfos[idx].radius;
            expRadius = meteorInfos[idx].expRadius;
        }

        public override void Active()
        {
            base.Active();


            var cam = CameraManager.instance.PlayerCam;

            var ray = Physics2D.Raycast(user.transform.position, Vector2.down, 5,
                LayerMasks.GroundAndPlatform);
            float height = cam.m_Lens.OrthographicSize * 2;
            float width = height * cam.m_Lens.Aspect;
            Vector2 screenSize = new Vector2(width, height);
            float distance = screenSize.magnitude;

            Vector2 movePos = ray.collider != null ? ray.point : user.transform.position;
            float sin = Mathf.Sin(Mathf.Deg2Rad * meteorAngle);
            float cos = Mathf.Cos(Mathf.Deg2Rad * meteorAngle);


            float x = cos * distance;
            float y = sin * distance;
            Vector2 xy = new Vector2(x, y);
            Vector2 spawnPos = movePos + xy;

            GameObject meteor = GameManager.Factory.Get(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.Ine_Magic_3Circle_Moon, spawnPos);
            meteor.transform.localScale = Vector3.one * (2 * meteorRadius);

            meteor.transform.DOMove(movePos, meteorSpeed).SetEase(meteorEase).SetSpeedBased().onComplete += () =>
            {
                AttackObject exp = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                    Define.PlayerEffect.Ine_Magic_3Circle_MoonExplode, meteor.transform.position);
                exp.transform.localScale = Vector3.one * (expRadius * 2);
                exp.Init(attacker,
                    new AtkItemCalculation(attacker as Actor, this), 1);
                exp.Init(GameManager.instance.Player.atkInfo);
                exp.Init((int)BaseGroggyPower);

                GameManager.Factory.Return(meteor);
                tween?.Kill();
            };
        }
    }
}