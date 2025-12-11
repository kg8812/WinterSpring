using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02_Scripts.Tester
{
    public class ActiveSkillPickUp_Tester: MonoBehaviour
    {
        public int itemId;

        [Button(ButtonSizes.Medium)]
        public void CreateActiveSkillPickup()
        {
            var pickUp = GameManager.Item.ActiveSkillPickUp.CreateNew(itemId);
            pickUp.transform.position = transform.position;
        }
        
        [Button(ButtonSizes.Medium)]
        public void CreateWeaponPickup()
        {
            var pickUp = GameManager.Item.WeaponPickUp.CreateNew(itemId);
            pickUp.transform.position = transform.position;
        }
        
        [Button(ButtonSizes.Medium)]
        public void CreateAccPickup()
        {
            var pickUp = GameManager.Item.AccPickUp.CreateNew(itemId);
            pickUp.transform.position = transform.position;
        }
    }
}