using System.Collections.Generic;
using Apis;
using GameStateSpace;
using Save.Schema;
using UI;

namespace chamwhy
{
    public class Nunna: Npc
    {
        private List<MenuItem> menuList = new();
        public override void OnInteract()
        {
            menuList.Clear();
            menuList.Add(new MenuItem("대화", () =>
            {
                _dialogue.SelectNpc();
            }));
            if (DataAccess.LobbyData.IsOpen(201))
            {
                menuList.Add(new MenuItem("영구성장", () =>
                {
                    GameManager.UI.CreateUI("UI_Growth", UIType.Scene);
                }));
            }
            if (DataAccess.LobbyData.IsOpen(202))
            {
                menuList.Add(new MenuItem("온기 공유", () =>
                {
                    if (BuffDatabase.DataLoad.TryGetBuff(510101, out var buff))
                    {
                        SubBuff sub = SubBuffResources.Get(buff);
                        GameManager.instance.Player.AddSubBuff(GameManager.instance.Player,buff,sub);
                    }
                    // TODO
                    // GameManager.instance.TryOffGameState(GameStateType.InteractionState);
                }));
            }
            menuList.Add(new MenuItem("튜토리얼 하기", () =>
            {
                _dialogue.PlayFixedScript(fixedScriptIndex, EndAction);
            }));
            
            UI_Choice uiChoice = GameManager.UI.CreateUI("UI_Choice", UIType.Ingame, withoutActivation:true) as UI_Choice;

            uiChoice.SetTrans(transform);
            uiChoice.SetMenu(menuList);
            uiChoice.TryActivated();
        }
    }
}