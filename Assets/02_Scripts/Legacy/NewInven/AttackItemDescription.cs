using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Apis
{
    public class AttackItemDescription : ItemDescription
    {
        // public override AbsInvenManager Inven => InvenManager.instance.Wp;

        public GameObject tab;
        [SerializeField] GameObject baseStat;
        [SerializeField] GameObject skillSection;
        public GameObject selectText;

        [SerializeField] TextMeshProUGUI baseStatText;
        [SerializeField] TextMeshProUGUI skillText;

        public override void ChangeInfo(Item item)
        {
            base.ChangeInfo(item);
            if (item is not Weapon weapon)
            {
                tab.SetActive(false);
            }
            else
            {
                tab.SetActive(true);
                baseStat.SetActive(true);

                if (weapon.Skill != null)
                {
                    skillSection.SetActive(true);
                    skillText.text = "";
                    skillText.text += weapon.Skill.SkillName + "\n";
                    skillText.text += weapon.Skill.Desc;
                }
                else
                {
                    skillSection.SetActive(false);
                }
                baseStatText.text = "공격력 : " + weapon.Atk;
            }
        }
        
        
        public virtual void ChangeInfo(Skill skill)
        {
            // TODO: skill info 수정.
        }
    }
}