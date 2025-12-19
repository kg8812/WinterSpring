using Apis.SkillTree;
using chamwhy.UI;
using Save.Schema;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot : UIAsset_Toggle
{
    public enum SlotType
    {
        Low,High,Inven
    }
    
    public static bool IsDragging;
    public static UI_DragItem DragImg;
    public static SkillTreeSlot ToChangeSlot;
    private SkillTree _skillTree;
    public SkillTree CurSkillTree => _skillTree;
    public SlotType slotType;
    private TextMeshProUGUI _text;
    public Image skillIcon;
    public int index;
    public bool useDrag;
    
    public GameObject lockImage;
    [ShowIf("slotType",SlotType.Inven)]
    public GameObject levelTwo;
    [ShowIf("slotType",SlotType.Inven)]
    public GameObject lowLevel;
    [ShowIf("slotType",SlotType.Inven)]
    public Image[] highLevel;
    [ShowIf("slotType",SlotType.Inven)]
    public GameObject levelOne;
    [ShowIf("slotType",SlotType.Inven)]
    public Sprite[] playerHighIcons;
    public bool isLocked;
    
    private static bool TryDrag(SkillTree item)
    {
        if (IsDragging) return false;
        IsDragging = true;
        DragImg.TryActivated();
        return true;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!useDrag) return;

        base.OnBeginDrag(eventData);
        
        if (TryDrag(_skillTree))
        {
            IsDragging = true;
            ToChangeSlot = this;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!useDrag) return;

        base.OnEndDrag(eventData);
        IsDragging = false;
        DragImg.TryDeactivated();
        if (ToChangeSlot != null)
        {
            if (ToChangeSlot._skillTree != null)
            {
                Debug.Log("이미 장착중임");
                return;
            }

            if (_skillTree == null || !_skillTree.CheckEquipable(ToChangeSlot))
            {
                Debug.Log("슬롯이 맞지않음");
                return;
            }
            ToChangeSlot.OnSlotChanged(_skillTree);
            OnSlotChanged(null);
        }
        else
        {
            Debug.Log("No Slot");
        }
    }

    void Lock()
    {
        lockImage.SetActive(true);
        skillIcon.gameObject.SetActive(false);
    }
    public void OnSlotChanged(SkillTree skillTree)
    {
        skillIcon.gameObject.SetActive(true);
        lockImage.SetActive(false);

        if (!isLocked)
        {
            switch (slotType)
            {
                case SlotType.High:
                    SkillTreeDatas.highSlotOpened.Add(index);
                    break;
                case SlotType.Low:
                    SkillTreeDatas.lowSlotOpened.Add(index);
                    break;
            }
        }
        _skillTree = skillTree;
        if (skillTree == null)
        {
            skillIcon.gameObject.SetActive(false);
            if (slotType == SlotType.Inven)
            {
                Lock();
            }
            return;
        }
        
        var trees = GameManager.Save.currentSlotData.TempSaveData.SkillTreeData.equippedSkillTrees;
        switch (slotType)
        {
            case SlotType.Low:
                if (!SkillTreeDatas.lowSlotOpened.Contains(index))
                {
                    Lock();
                    return;
                }
                SkillTreeDatas.ApplySkillTree(skillTree.Index,1);
                trees.TryAdd(skillTree.Index, new SkillTreeSaveData.SkillTreeSlotData());
                trees[skillTree.Index] = new SkillTreeSaveData.SkillTreeSlotData()
                {
                    slotIndex = index,
                    slotType = SlotType.Low,
                };
                break;
            case SlotType.High:
                if (!SkillTreeDatas.highSlotOpened.Contains(index))
                {
                    Lock();
                    return;
                }
                SkillTreeDatas.ApplySkillTree(skillTree.Index, 2);
                trees.TryAdd(skillTree.Index, new SkillTreeSaveData.SkillTreeSlotData());
                trees[skillTree.Index] = new SkillTreeSaveData.SkillTreeSlotData()
                {
                    slotIndex = index,
                    slotType = SlotType.High,
                };
                break;
            case SlotType.Inven:
                SkillTreeDatas.DeApplySkillTree(skillTree.Index);
                trees.Remove(skillTree.Index);
                SetLevelIcon(skillTree);
                break;
        }
    }

    void SetLevelIcon(SkillTree tree)
    {
        switch (tree.SlotType)
        {
            case SkillTree.SlotTypeEnum.Low:
                levelTwo.SetActive(false);
                levelOne.SetActive(true);
                lowLevel.SetActive(true);
                highLevel.ForEach(x => x.gameObject.SetActive(false));
                break;
            case SkillTree.SlotTypeEnum.High:
                levelTwo.SetActive(false);
                levelOne.SetActive(true);
                lowLevel.SetActive(true);
                highLevel.ForEach(x => x.gameObject.SetActive(true));
                break;
            case SkillTree.SlotTypeEnum.Medium:
                levelTwo.SetActive(true);
                levelOne.SetActive(false);
                highLevel.ForEach(x => x.gameObject.SetActive(true));
                break;
        }
        highLevel.ForEach( x => x.sprite = playerHighIcons[(int)GameManager.instance.Player.playerType]);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!useDrag) return;

        if (IsDragging)
        {
            ToChangeSlot = this;
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (!useDrag) return;

        if (IsDragging && ReferenceEquals(this, ToChangeSlot))
        {
            ToChangeSlot = null;
        }
    }
}
