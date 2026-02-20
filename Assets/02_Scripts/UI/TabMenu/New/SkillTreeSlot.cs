using Apis.SkillTree;
using chamwhy;
using chamwhy.UI;
using Managers;
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

    public int skillTreeIndex => ((int)GameManager.instance.Player.playerType + 1) * 100 + index + 1;
    private static bool TryDrag(SkillTree item)
    {
        if (IsDragging || item == null) return false;
        DragImg.DragImg.sprite = item.icon;
        IsDragging = true;
        DragImg.TryActivated();
        return true;
    }

    public override void Init()
    {
        base.Init();

        if (slotType == SlotType.Inven)
        {
            var skillTree = SkillTreeDatas.GetSkillTree(skillTreeIndex);
            if (skillTree != null)
            {
                skillIcon.sprite = skillTree.icon;
            }
            SetLevelIcon(skillTree);
        }
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!useDrag) return;

        base.OnBeginDrag(eventData);
        
        if (TryDrag(_skillTree))
        {
            IsDragging = true;
            ToChangeSlot = this;
            skillIcon.color = Color.grey;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!useDrag || !IsDragging) return;

        base.OnEndDrag(eventData);
        IsDragging = false;
        DragImg.TryDeactivated();
        if (ToChangeSlot != null)
        {
            if (_skillTree == null) return;
            
            if (ToChangeSlot._skillTree != null)
            {
                SystemManager.SystemAlert("이미 장착중인 슬롯입니다",null);
                skillIcon.color = Color.white;
                return;
            }

            if (!_skillTree.CheckEquipable(ToChangeSlot))
            {
                SystemManager.SystemAlert("등급이 맞지 않습니다",null);
                skillIcon.color = Color.white;
                
                return;
            }

            if (!_skillTree.CheckSlotIndex(ToChangeSlot))
            {
                SystemManager.SystemAlert("원래 위치에 넣어주세요",null);
                skillIcon.color = Color.white;
                
                return;
            }
            ToChangeSlot.OnSlotChanged(_skillTree);
            OnSlotChanged(null);
        }
        else
        {
            Debug.Log("No Slot");
            skillIcon.color = Color.white;
        }
    }

    void Lock()
    {
        lockImage.SetActive(true);
        skillIcon.gameObject.SetActive(false);
    }

    bool IsSkillTreeOpened()
    {
        return SkillTreeDatas.activatedIndex.Contains(skillTreeIndex);
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
        skillIcon.color = Color.white;
        
        if (skillTree == null)
        {
            if (slotType == SlotType.Inven)
            {
                if (IsSkillTreeOpened())
                { 
                    skillIcon.color = Color.grey;
                }
                else
                {
                    Lock();
                }
            }
            else
            {
                skillIcon.gameObject.SetActive(false);
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
        
        skillIcon.sprite = skillTree.icon;
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
