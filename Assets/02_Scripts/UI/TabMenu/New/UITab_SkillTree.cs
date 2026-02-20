using System.Collections.Generic;
using System.Linq;
using Apis;
using Apis.SkillTree;
using chamwhy.UI.Focus;
using Sirenix.Utilities;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace chamwhy
{
    public class UITab_SkillTree: UI_FocusContent
    {
        [SerializeField] GameObject[] _onObjects;
        [SerializeField] private GameObject[] _OffObjects;
        
        public TextMeshProUGUI[] categoryNames;
        public SkillTreeSlot[] lowerSlots;
        public SkillTreeSlot[] higherSlots;
        public List<SkillTreeSlot> inven = new();
        public UI_SkillTreeInfo skillTreeInfo;
        public Image playerIcon;
        public TextMeshProUGUI playerName;

        public Sprite[] playerIconSprites;
        UI_NavigationController navigation;
        public UI_DragItem dragItem;
        
        public override void Init()
        {
            base.Init();

            navigation = GetComponent<UI_NavigationController>();
            SkillTreeSlot.DragImg = dragItem;
            
            lowerSlots.ForEach(x =>
            {
                x.InitCheck();
                x.OnValueChanged.AddListener(selected =>
                {
                    if (selected)
                    {
                        skillTreeInfo.Set(x.CurSkillTree, x);
                    }
                });
            });
            higherSlots.ForEach(x =>
            {
                x.InitCheck();
                x.OnValueChanged.AddListener(selected =>
                {
                    if (selected)
                    {
                        skillTreeInfo.Set(x.CurSkillTree, x);
                    }
                });
            });
            inven.ForEach(x =>
            {
                x.InitCheck();
                x.OnValueChanged.AddListener(selected =>
                {
                    if (selected)
                    {
                        skillTreeInfo.Set(x.CurSkillTree, x);
                    }
                });
            });
            
        }

        public override void OnOpen()
        {
            base.OnOpen();


            Player player = GameManager.instance.Player;

            foreach (var onObject in _onObjects)
            {
                onObject.SetActive(PlayerActiveSkill.IsActivated() && PlayerPassiveSkill.IsActivated());
            }

            foreach (var offObj in _OffObjects)
            {
                offObj.SetActive(!(PlayerActiveSkill.IsActivated() && PlayerPassiveSkill.IsActivated()));
            }
            
            
            categoryNames[0].text = StrUtil.GetTagName(101 + 3 * (int)player.playerType);
            categoryNames[1].text = StrUtil.GetTagName(102 + 3 * (int)player.playerType);
            categoryNames[2].text = StrUtil.GetTagName(103 + 3 * (int)player.playerType);
            
            ResetEquipSlots();
            SetSlots();
            playerIcon.sprite = playerIconSprites[(int)player.playerType];
            navigation.Activate();
            playerName.text = StrUtil.GetPlayerName(player.playerType);
        }

        public override void OnClose()
        {
            base.OnClose();
            skillTreeInfo.gameObject.SetActive(false);
            navigation.Deactivate();
        }

        void ResetEquipSlots()
        {
            foreach (var x in lowerSlots)
            {
                x.OnSlotChanged(null);
            }

            foreach (var x in higherSlots)
            {
                x.OnSlotChanged(null);
            }
        }
        
        public void SetSlots()
        {
            var equippedSkillTrees = GameManager.Save.currentSlotData.TempSaveData.SkillTreeData.equippedSkillTrees;

            var keys = equippedSkillTrees.Keys.ToList();
            
            foreach (var key in keys)
            {
                var slotData = equippedSkillTrees[key];
                var tree = SkillTreeDatas.GetSkillTree(key);
                
                if (slotData.slotType == SkillTreeSlot.SlotType.High)
                {
                    foreach (var slot in higherSlots)
                    {
                        if (slot.index == slotData.slotIndex &&
                            tree.PlayerType == GameManager.instance.Player.playerType)
                        {
                            slot.OnSlotChanged(tree);
                            break;
                        }
                    }
                }
                else if (slotData.slotType == SkillTreeSlot.SlotType.Low)
                {
                    foreach (var slot in lowerSlots)
                    {
                        if (slot.index == slotData.slotIndex &&
                            tree.PlayerType == GameManager.instance.Player.playerType)
                        {
                            slot.OnSlotChanged(tree);
                            break;
                        }
                    }
                }
            }
            
            var skillTrees = SkillTreeDatas.GetAvailableSkillTrees();
            
            for (int i = 0; i < inven.Count; i++)
            {
                inven[i].OnSlotChanged(skillTrees.Find(x => x.Index == inven[i].skillTreeIndex) ? skillTrees[i] : null);
            }
        }

        public override void KeyControl()
        {
            base.KeyControl();
            navigation.KeyControl();
        }

        public override void GamePadControl()
        {
            base.GamePadControl();
            navigation.GamePadControl();
        }
    }
}