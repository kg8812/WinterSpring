using System.Collections;
using Apis;
using chamwhy;
using UnityEngine;

public partial class SeguMecha
{
    public interface IAtkStrategy
    {
        public void Attack();
        public void Update();
        public void FireOn();
        public void FireOff();

        public bool IsAtk { get;}
        public bool IsAbleToTurn { get; }
    }

    public class FireBullets : IAtkStrategy
    {
        private SeguMecha mecha;
        public FireBullets(SeguMecha mecha)
        {
            this.mecha = mecha;
            isAtk = false;
        }

        public void Attack()
        {
            Projectile bullet = GameManager.Factory.Get<Projectile>(FactoryManager.FactoryType.Effect,
                Define.PlayerEffect.GoseguMechaBullet, mecha.firePos.position);
            
            bullet.Init(mecha,new AtkBase(mecha , mecha.bulletInfo.dmg));
            bullet.Init(mecha.bulletInfo);
            bullet.Init(mecha.groggy);
            
            bullet.Fire();
        }

        float curTime = 0;
        bool isAtk;
        public bool IsAtk => isAtk;

        public bool IsAbleToTurn => false;

        private bool isCd;
        private float curCd = 0;
        
        public void Update()
        {
            if(isAtk)
            {
                curTime += Time.deltaTime;

                if (mecha.atkSpeed - mecha.baseAtkSpeed < mecha.maxIncrement)
                {
                    mecha.atkSpeed += mecha.atkSpeedIncrement * Time.deltaTime;
                }
                else
                {
                    mecha.atkSpeed = mecha.baseAtkSpeed + mecha.maxIncrement;
                }
            
                if (curTime > 100 / mecha.atkSpeed)
                {
                    Attack();
                    curTime = 0;
                }
            }
            else
            {
                curTime = 0;
                mecha.atkSpeed = mecha.baseAtkSpeed;
            }
        }

        public void FireOn()
        {
            isAtk = true;
            if (!isCd)
            {
                Attack();
                mecha.StartCoroutine(AttackCd());
                mecha.animator.SetInteger("AttackType",1);
                mecha.animator.SetTrigger("IsAttack");
            }
        }

        public void FireOff()
        {
            isAtk = false;
            mecha.animator.SetTrigger("AttackEnd");
        }

        IEnumerator AttackCd()
        {
            if (isCd) yield break;
            isCd = true;

            curCd = 100 / mecha.baseAtkSpeed;
            while (curCd > 0)
            {
                curCd -= Time.deltaTime;
                yield return null;
            }

            isCd = false;
        }
    }

    public class FireLaser : IAtkStrategy
    {
        private bool isCharge;
        private bool isAtk;
        
        private readonly SeguMecha mecha;
        readonly LaserInfo laserInfo;

        private float dmgRatio;
        private float chargeDistance;

        public bool IsAtk => isAtk;

        public bool IsAbleToTurn => laser == null;

        public FireLaser(SeguMecha mecha, LaserInfo info)
        {
            this.mecha = mecha;
            laserInfo = info;
            mecha.maxChargeTime = laserInfo.maxChargeTime;
        }

        private BeamEffect laser;
        
        public void Attack()
        {
            mecha.animator.SetInteger("AttackType",0);
            mecha.animator.SetTrigger("IsAttack");

            laser = GameManager.Factory.Get<BeamEffect>(FactoryManager.FactoryType.AttackObject,
                Define.PlayerSkillObjects.GoseguLaser, mecha.firePos.position);
            
            laser.transform.SetParent(mecha.transform);
            laser.Init(mecha, new AtkBase(mecha, laserInfo.atkInfo.dmg * dmgRatio / 100),
                laserInfo.duration);
            laser.Init(laserInfo.atkInfo);
            BeamEffect.BeamInfo tempInfo = new(laserInfo.beamInfo);
            tempInfo.distance += chargeDistance;
            laser.Init(tempInfo);
            laser.Init(laserInfo.groggy);
            laser.Fire();
            laser.AddEventUntilInitOrDestroy(x =>
            {
                if (x.target is Actor target)
                {
                    target.AddSubBuff(mecha.player,SubBuffType.Debuff_Chill);
                }
            },EventType.OnTargetFirstAttack);
        }
        
        private int count = 1;
        
        public void Update()
        {
            if (!isAtk) return;
            
            if (isCharge && mecha.curChargeTime < laserInfo.maxChargeTime)
            {
                mecha.curChargeTime += Time.deltaTime;

                if (mecha.curChargeTime > count)
                {
                    count++;
                    dmgRatio += laserInfo.chargeDmg;
                    chargeDistance += laserInfo.chargeDistance;
                }
            }
            else
            {
                FireOff();
            }
        }

        public void FireOn()
        {
            var effect = mecha.EffectSpawner.Spawn(Define.PlayerEffect.GoseguMechaCharge, mecha.firePos.position,false);
            effect.transform.SetParent(mecha.transform);
            mecha.curChargeTime = 0;
            count = 1;
            dmgRatio = 100;
            chargeDistance = 0;
            isCharge = true;
            isAtk = true;
            laser = null;
        }

        public void FireOff()
        {
            mecha.EffectSpawner.Remove(Define.PlayerEffect.GoseguMechaCharge);
            if (mecha.curChargeTime > laserInfo.minChargeTime)
            {
                Attack();
            }
            isCharge = false;
            if ((object)laser == null)
            {
                isAtk = false;
            }
            else
            {
                laser.AddEventUntilInitOrDestroy(AtkOff,EventType.OnDestroy);
            }

            mecha.curChargeTime = 0;
        }

        void AtkOff(EventParameters _)
        {
            isAtk = false;
        }
        
    }
}
