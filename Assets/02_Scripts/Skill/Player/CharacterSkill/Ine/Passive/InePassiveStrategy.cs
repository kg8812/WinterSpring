using chamwhy;
using UnityEngine;

namespace Apis
{
    public interface IPassiveFire
    {
        public void Fire(IneFeather feather,float angle = 0,float move = 0,float scale = 0);
    }

    public class FireFeather : IPassiveFire
    {
        private InePassiveSkill passive;

        public FireFeather(InePassiveSkill _passive)
        {
            passive = _passive;
        }
        public void Fire(IneFeather feather , float angle = 0,float move = 0,float scale = 0)
        {
            feather.AddEventUntilInitOrDestroy(_ =>
            {
                passive?.OnFeatherAtk.Invoke();
            });
            feather.Fire(false);
        }
    }
    
    public class MoonSlash : IPassiveFire
    {
        public ProjectileInfo info;
        public int groggy;
        public float radius;

        private InePassiveSkill passive;
        
        public MoonSlash(InePassiveSkill passive, ProjectileInfo info,int groggy,float radius)
        {
            this.passive = passive;
            this.info = info;
            this.groggy = groggy;
            this.radius = radius;
        }
        public void Fire(IneFeather feather , float angle = 0,float move = 0,float scale = 0)
        {
            feather.Destroy();
            AttackObject slash = GameManager.Factory.Get<AttackObject>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.Ine_MoonSlash,
                GameManager.instance.ControllingEntity.Position + Vector3.right * (passive.direction != null ? (int)passive.direction.Direction : 1));
            
            slash.transform.localScale = Vector3.one * (radius * 2);
            slash.transform.localScale = new Vector3(slash.transform.localScale.x * (passive.direction != null ?(int)passive.direction.Direction : 1) * scale,
                slash.transform.localScale.y, 1);
            slash.transform.rotation = Quaternion.Euler(Vector3.zero);
            slash.transform.Rotate(angle,0,0);
            slash.transform.Translate(move, 0, 0);
            slash.Init(passive.attacker, new AtkItemCalculation(passive.user as Actor, passive, info.dmg), 1);
            slash.Init(info);
            slash.Init(groggy);
            slash.AddEventUntilInitOrDestroy(_ =>
            {
                passive?.OnFeatherAtk.Invoke();
            });
        }
    }
}