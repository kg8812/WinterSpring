using Apis.SkillTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTreeInfo : MonoBehaviour
{
    public Image highIcon;
    public Image lowIcon;
    public Image unEquipIcon;
    public Image skillIcon;
    public GameObject[] numbers;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI highDesc;
    public TextMeshProUGUI lowDesc;
    public GameObject equipDisplay;
    public GameObject highEquipable;
    public GameObject lowEquipable;
    public TextMeshProUGUI typeText;
    

    public void Set(SkillTree skillTree,SkillTreeSlot slot)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (skillTree == null || slot == null)
        {
            gameObject.SetActive(false);
            return;
        }
        foreach (var number in numbers)
        {
            number.SetActive(false);
        }
        highIcon.gameObject.SetActive(false);
        lowIcon.gameObject.SetActive(false);
        unEquipIcon.gameObject.SetActive(false);

        switch (slot.slotType)
        {
            case SkillTreeSlot.SlotType.High:
                equipDisplay.SetActive(true);
                numbers[slot.index].SetActive(true);
                highIcon.gameObject.SetActive(true);
                break;
            case SkillTreeSlot.SlotType.Low:
                equipDisplay.SetActive(true);
                numbers[slot.index + 3].SetActive(true);
                lowIcon.gameObject.SetActive(true);
                break;
            case SkillTreeSlot.SlotType.Inven:
                equipDisplay.SetActive(false);
                unEquipIcon.gameObject.SetActive(true);
                break;
        }

        skillIcon.sprite = skillTree.icon;
        nameText.text = skillTree.Name;
        highDesc.text = skillTree.HighDescription;
        lowDesc.text = skillTree.LowDescription;

        switch (skillTree.SlotType)
        {
            case SkillTree.SlotTypeEnum.High:
                highEquipable.SetActive(true);
                lowEquipable.SetActive(false);
                break;
            case SkillTree.SlotTypeEnum.Low:
                highEquipable.SetActive(false);
                lowEquipable.SetActive(true);
                break;
            case SkillTree.SlotTypeEnum.Medium:
                highEquipable.SetActive(true);
                lowEquipable.SetActive(true);
                break;
        }

        switch (skillTree.TreeType)
        {
            case SkillTree.TreeTypeEnum.Active:
                typeText.text = "액티브";
                break;
            case SkillTree.TreeTypeEnum.Passive:
                typeText.text = "패시브";
                break;
            case SkillTree.TreeTypeEnum.Support:
                typeText.text = "서포트";
                break;
        }
    }

}
