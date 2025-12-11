using System;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Apis
{
    [CreateAssetMenu(fileName = "GoseguPassive", menuName = "Scriptable/Skill/GoseguPassive")]
    public class GoseguPassive : PlayerPassiveSkill
    {
        protected override bool UseGroggyRatio => false;

        [InfoBox("드론을 클릭하여 선택한 후 드론 프리팹 인스펙터창에서 스탯값을 수정해주세요")]
        [TitleGroup("스탯값")] [LabelText("메인 드론")] [SerializeField] private GoseguMainDrone mainDrone;

        [HideInInspector] public List<GoseguDrone.DroneInfo> _droneInfos = new();
        
        public override float Atk =>
            (baseDmg + stats.Stat.dmg) *
            (1 + stats.Stat.dmgRatio / 100 +
             GameManager.Tag.GetTagCount(TagManager.SkillTreeTag.Drone) *
             GameManager.Tag.Data.DroneIncrement / 100);

        [HideInInspector] Dictionary<DroneType,GoseguDrone> drones;

        private UnityEvent _onDroneKill;
        public UnityEvent OnDroneKill => _onDroneKill ??= new();
        public int DroneCount => drones.Values.Count;

        [HideInInspector] public Action<AttackObject> OnDroneAttack;
        [HideInInspector] public Action OnDroneChange;
        
        public enum DroneType
        {
            Main,Missile,Laser,Heal,Freeze,
        }

        protected override TagManager.SkillTreeTag Tag => TagManager.SkillTreeTag.Drone;

        protected override float TagIncrement => GameManager.Tag.Data.DroneIncrement;

        public override void Init()
        {
            base.Init();
            drones ??= new();
            _droneInfos ??= new();
        }

        protected override void OnEquip(IMonoBehaviour owner)
        {
            base.OnEquip(owner);

            drones ??= new();
            CreateDrone(DroneType.Main); 
            TurnOnDrones();
            GoseguActiveSkill active = activeUser.ActiveSkill as GoseguActiveSkill;
            active?.SetPassive(this);
            isOn = true;
        }

        protected override void OnUnEquip()
        {
            base.OnUnEquip();
            RemoveDrone(DroneType.Main);
        }

        public override void Cancel()
        {
            base.Cancel();
        }

        public GoseguDrone GetDrone(DroneType droneType)
        {
            if (drones.TryGetValue(droneType, out var drone))
            {
                return drone;
            }

            return null;
        }

        public List<GoseguDrone> GetAllDrones()
        {
            return drones.Values.ToList();
        }

        private bool isOn = false;
        public void TurnOffDrones()
        {
            isOn = false;
            drones.ForEach(x => x.Value.gameObject.SetActive(false));
        }

        public void TurnOnDrones()
        {
            isOn = true;
            drones.ForEach(x =>
            {
                x.Value.PetFollower.SetPosition();
                x.Value.gameObject.SetActive(true);
                x.Value.SetMaster(GameManager.instance.Player);
            });
        }

        public void CreateDrone(DroneType droneType)
        {
            drones ??= new();
            
            if (drones.ContainsKey(droneType)) return;
            
            string address;
            Vector2 offset;
            
            switch (droneType)
            {
                case DroneType.Main:
                    address = Define.PlayerSkillObjects.GoseguMainDrone;
                    offset = new Vector2(0, 2f);
                    break;
                case DroneType.Missile:
                    address = Define.PlayerSkillObjects.GoseguMissileDrone;
                    offset = new Vector2(-1, 2f);
                    break;
                case DroneType.Laser:
                    address = Define.PlayerSkillObjects.GoseguLaserDrone;
                    offset = new Vector2(1, 2f);
                    break;
                case DroneType.Heal:
                    address = Define.PlayerSkillObjects.GoseguHealDrone;
                    offset = new Vector2(-0.5f, 3f);
                    break;
                case DroneType.Freeze:
                    address = Define.PlayerSkillObjects.GoseguFreezeDrone;
                    offset = new Vector2(0.5f, 3f);
                    break;
                default:
                    return;
            }
            
            GoseguDrone drone = GameManager.Factory.Get<GoseguDrone>(FactoryManager.FactoryType.Normal,
                address,(Vector2)user.Position + offset);
            drone.SetMaster(user as Actor);
            drone.Init(this, offset);
            drones.Add(droneType, drone);
            drone.gameObject.SetActive(isOn);
            OnDroneChange?.Invoke();
        }

        public void RemoveDrone(DroneType droneType)
        {
            if (drones.TryGetValue(droneType, out var drone))
            {
                drone.ReleaseMaster();
                GameManager.Factory.Return(drone.gameObject);
                drones.Remove(droneType);
                OnDroneChange?.Invoke();
            }
        }
    }
}